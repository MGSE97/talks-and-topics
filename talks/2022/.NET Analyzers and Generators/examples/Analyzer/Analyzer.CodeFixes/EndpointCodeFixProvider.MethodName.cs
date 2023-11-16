using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Rename;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer
{
    public partial class EndpointCodeFixProvider
    {
        /*
         * Fix Endpoint class Get method name to "Get"
         */
        private static CodeAction RegisterMethodNameFixes(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic)
        {
            return CodeAction.Create(
                        title: CodeFixResources.AG3_CodeFixTitle,
                        createChangedSolution: c => FixNameAsync(context, root, model, diagnostic, c),
                        equivalenceKey: diagnostic.Id
                    );
        }

        private static async Task<Solution> FixNameAsync(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            // Get token from root
            var token = root.FindToken(diagnostic.Location.SourceSpan.Start);

            // Get the symbol representing the type to be renamed.
            var typeSymbol = model.GetDeclaredSymbol(token.Parent, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var solution = context.Document.Project.Solution;
            var optionSet = solution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(solution, typeSymbol, "Get", optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the renamed method Get.
            return newSolution;
        }
    }
}
