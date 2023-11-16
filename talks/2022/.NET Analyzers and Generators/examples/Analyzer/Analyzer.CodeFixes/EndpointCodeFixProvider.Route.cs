using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer
{
    public partial class EndpointCodeFixProvider
    {
        /*
         * Add [Route("/EndpointName")] if missing in Endpoint class
         */
        private static CodeAction RegisterRouteFixes(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic)
        {
            return CodeAction.Create(
                        title: CodeFixResources.AG1_CodeFixTitle,
                        createChangedDocument: c => FixRouteAsync(context, root, model, diagnostic, c),
                        equivalenceKey: diagnostic.Id
                    );
        }

        private static Task<Document> FixRouteAsync(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            // Get class declaration from root
            var classDeclaration = root.FindToken(diagnostic.Location.SourceSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

            // Create [Route("/EndpointName")]
            var route = Regex.Replace(classDeclaration.Identifier.Text.Replace("Endpoint", ""), "([A-Z0-9])", "-$1").TrimStart('-');
            var attributes = classDeclaration.AttributeLists.Add(
                SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                    SyntaxFactory.Attribute(
                        SyntaxFactory.IdentifierName("Route"),
                        SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList<AttributeArgumentSyntax>(new[] {
                            SyntaxFactory.AttributeArgument(SyntaxFactory.ParseExpression($"\"/{route.ToLowerInvariant()}\""))
                        }))
                )))
            );

            return Task.FromResult(
                    context.Document.WithSyntaxRoot(
                        root.ReplaceNode(
                            classDeclaration,
                            classDeclaration.WithAttributeLists(attributes)
                        ))
                );
        }
    }
}
