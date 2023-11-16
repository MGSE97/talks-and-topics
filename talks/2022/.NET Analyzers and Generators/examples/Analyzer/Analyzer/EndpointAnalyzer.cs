using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EndpointAnalyzer : DiagnosticAnalyzer
    {
        public static Regex FindRouteParams = new Regex("{([^}]+)}");

        public static readonly DiagnosticDescriptor RouteRule = new DiagnosticDescriptorBuilder(1, DiagnosticSeverity.Error).Build();
        public static readonly DiagnosticDescriptor MethodMissingRule = new DiagnosticDescriptorBuilder(2, DiagnosticSeverity.Error).Build();
        public static readonly DiagnosticDescriptor MethodNameRule = new DiagnosticDescriptorBuilder(3, DiagnosticSeverity.Info).Build();
        public static readonly DiagnosticDescriptor ArgumentsRule = new DiagnosticDescriptorBuilder(4, DiagnosticSeverity.Warning).Build();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
            ImmutableArray.Create(
                RouteRule, 
                MethodMissingRule, 
                MethodNameRule, 
                ArgumentsRule
            );

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSemanticModelAction(AnalyzeModel);
        }

        private void AnalyzeModel(SemanticModelAnalysisContext context)
        {
            // Stop if not in Endpoints directory
#if RELEASE
            if (!context.SemanticModel.SyntaxTree.FilePath.Replace("\\", "/").Contains("/Endpoints/")) return;
#endif

            // Find all classes in file
            var classes = context.SemanticModel.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var classDeclaration in classes)
            {
                // Ignore classes that don't end with Endpoint
                if (!classDeclaration.Identifier.Text.EndsWith("Endpoint")) continue;

                // Run checks on class
                VerifyRouteAttribute(context, classDeclaration);
                VerifyGetMethod(context, classDeclaration);
            }
        }

        private void VerifyRouteAttribute(SemanticModelAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            // Find route attributes
            if (!classDeclaration.AttributeLists
                .SelectMany(list => list.ChildNodes().OfType<AttributeSyntax>())
                .Any(attr => attr.Name.ToString() == "Route"))
            {
                // Produce a diagnostic if missing
                context.ReportDiagnostic(Diagnostic.Create(RouteRule,
                                                           classDeclaration.Identifier.GetLocation(),
                                                           classDeclaration.Identifier.Text.Replace("Endpoint", "")));
            }
        }

        private void VerifyGetMethod(SemanticModelAnalysisContext context, ClassDeclarationSyntax classDeclaration)
        {
            // Find Get method
            var getMethod = classDeclaration
                .ChildNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text.Equals("Get", StringComparison.InvariantCultureIgnoreCase));

            if (getMethod == null)
            {
                // Produce a diagnostic if missing
                context.ReportDiagnostic(Diagnostic.Create(MethodMissingRule,
                                                           classDeclaration.Identifier.GetLocation(),
                                                           classDeclaration.Identifier.Text.Replace("Endpoint", "")));
                return;
            }

            // Check naming conventions
            if (getMethod.Identifier.Text != "Get")
            {
                // Produce a diagnostic if wrong
                context.ReportDiagnostic(Diagnostic.Create(MethodNameRule,
                                                           getMethod.Identifier.GetLocation(),
                                                           classDeclaration.Identifier.Text.Replace("Endpoint", ""),
                                                           getMethod.Identifier.Text,
                                                           "Get"));
            }

            // Get all routes endpoint uses
            var routes = classDeclaration.AttributeLists
                .SelectMany(list => list.ChildNodes().OfType<AttributeSyntax>())
                .Where(attr => attr.Name.ToString() == "Route" && attr.ArgumentList.Arguments.Count > 0)
                .Select(attr => (attr, attr.ArgumentList.Arguments[0].GetText().ToString().Trim('"')))
                .ToList();

            // Get all arguments of Get method
            List<(string name, Location location)> arguments = getMethod.ParameterList
                .ChildNodes()
                .OfType<ParameterSyntax>()
                .Select(p => (p.Identifier.Text.ToLowerInvariant(), p.GetLocation()))
                .ToList();

            // Find differences in arguments
            foreach (var (attr, route) in routes)
            {
                var routeParams = FindRouteParams.Matches(route)
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value.ToLowerInvariant())
                    .ToList();
                
                foreach (var (name, location) in arguments)
                {
                    // Produce diagnostic if missing
                    if (!routeParams.Contains(name))
                        context.ReportDiagnostic(Diagnostic.Create(ArgumentsRule,
                                                                   location,
                                                                   new [] { attr.GetLocation() },
                                                                   classDeclaration.Identifier.Text.Replace("Endpoint", ""),
                                                                   getMethod.Identifier.Text,
                                                                   name,
                                                                   route));
                }
            }
        }
    }
}
