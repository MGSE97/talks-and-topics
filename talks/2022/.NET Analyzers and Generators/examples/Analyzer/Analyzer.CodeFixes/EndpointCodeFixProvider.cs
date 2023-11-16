using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EndpointCodeFixProvider)), Shared]
    public partial class EndpointCodeFixProvider : CodeFixProvider
    {
        private ImmutableDictionary<string, Func<CodeFixContext, SyntaxNode, SemanticModel, Diagnostic, CodeAction>> _fixes = 
            new Dictionary<string, Func<CodeFixContext, SyntaxNode, SemanticModel, Diagnostic, CodeAction>>(){
                { EndpointAnalyzer.RouteRule.Id, RegisterRouteFixes },
                { EndpointAnalyzer.MethodMissingRule.Id, RegisterMethodMissingFixes },
                { EndpointAnalyzer.MethodNameRule.Id, RegisterMethodNameFixes },
                { EndpointAnalyzer.ArgumentsRule.Id, RegisterArgumentsFixes }
            }.ToImmutableDictionary();

        public sealed override ImmutableArray<string> FixableDiagnosticIds => 
            ImmutableArray.Create(
                EndpointAnalyzer.RouteRule.Id, 
                EndpointAnalyzer.MethodMissingRule.Id,
                EndpointAnalyzer.MethodNameRule.Id,
                EndpointAnalyzer.ArgumentsRule.Id
            );

        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            // Check for diagnostics
            if (!context.Diagnostics.Any()) return;

            // Prepare shared values
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

            // Gather fixes
            var actions = new List<(Diagnostic diagnostic, CodeAction action)>();
            foreach (var diagnostic in context.Diagnostics)
                actions.Add((diagnostic, _fixes[diagnostic.Id](context, root, semanticModel, diagnostic)));

            // Show each action separated
            /*foreach (var (diagnostic, action) in actions)
                context.RegisterCodeFix(action, diagnostic);*/

            // Group multiple actions
            if (actions.Count == 1)
                context.RegisterCodeFix(actions[0].action, actions[0].diagnostic);
            else
                context.RegisterCodeFix(
                        CodeAction.Create(
                            CodeFixResources.EndpointIssuesTitle, 
                            actions.Select(a => a.action).ToImmutableArray(), 
                            isInlinable: true
                        ), 
                        actions.Select(a => a.diagnostic).ToImmutableArray()
                    );
        }
    }
}
