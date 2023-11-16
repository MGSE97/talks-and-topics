using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer
{
    public partial class EndpointCodeFixProvider
    {
        /*
         * Add "/{argument}" to route attributes if its missing in Endpoint class routes
         */
        private static CodeAction RegisterArgumentsFixes(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic)
        {
            // Get node
            var node = root.FindNode(diagnostic.Location.SourceSpan);

            // Get argument name
            var argName = node.ChildTokens().First(t => t.Kind() == SyntaxKind.IdentifierToken).Text;

            // Get class declaration from root
            var classDeclaration = node.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

            // Get route from Route attributes
            (AttributeSyntax attr, string[] args, string route) route = classDeclaration.AttributeLists
                .SelectMany(list => list.ChildNodes().OfType<AttributeSyntax>())
                .Where(attr => attr.GetLocation() == diagnostic.AdditionalLocations.First())
                .Where(attr => attr.Name.ToString() == "Route" && attr.ArgumentList.Arguments.Count > 0)
                .Select(attr =>
                {
                    var r = attr.ArgumentList.Arguments[0].GetText().ToString().Trim('"');
                    return (attr, ParseRouteParams(r), r);
                })
                .First();

            
            // If all routes contains argument param, skip rest
            if (route.args.Contains(argName)) return null;

            // Create fix if not
            return CodeAction.Create(
                title: string.Format(CodeFixResources.AG4_CodeFixTitle, argName, route.route),
                createChangedDocument: c => CreateRouteParamAsync(context, root, model, diagnostic, route.attr, route.route, argName, c),
                equivalenceKey: diagnostic.Id
            );
        }

        private static string[] ParseRouteParams(string route)
        {
            return EndpointAnalyzer.FindRouteParams.Matches(route)
               .Cast<Match>()
               .Select(m => m.Groups[1].Value.ToLowerInvariant())
               .ToArray();
        }

        private static Task<Document> CreateRouteParamAsync(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic, AttributeSyntax attr, string route, string argName, CancellationToken cancellationToken)
        {
            // Add /{arg_name} to route strings
            var newArgs = attr.ArgumentList.Arguments.Replace(
                    attr.ArgumentList.Arguments.First(),
                    SyntaxFactory.AttributeArgument(SyntaxFactory.ParseExpression($"\"{route}/{{{argName}}}\""))
                );

            // Replace in root
            var newRoot = root.ReplaceNode(attr, attr.WithArgumentList(SyntaxFactory.AttributeArgumentList(newArgs)));
        
            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }
    }
}
