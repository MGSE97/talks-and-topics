using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analyzer
{
    public partial class EndpointCodeFixProvider
    {
        /*
         * Create default Get method if missing in Endpoint class
         */
        private static CodeAction RegisterMethodMissingFixes(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic)
        {
            return CodeAction.Create(
                        title: CodeFixResources.AG2_CodeFixTitle,
                        createChangedDocument: c => CreateGetMethodAsync(context, root, model, diagnostic, c),
                        equivalenceKey: diagnostic.Id
                    );
        }

        private static Task<Document> CreateGetMethodAsync(CodeFixContext context, SyntaxNode root, SemanticModel model, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            // Get class declaration from root
            var classDeclaration = root.FindToken(diagnostic.Location.SourceSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

            // Create method
            // public string Get()
            // {
            //  return "Hello There!";
            // }
            var newClassDeclaration = classDeclaration.AddMembers(
                SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("string"), "Get")
                    .AddBodyStatements(
                        SyntaxFactory.ReturnStatement(SyntaxFactory.ParseExpression("\"Hello there!\""))
                    )
                    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    .WithAdditionalAnnotations(Formatter.Annotation)
                );

            return Task.FromResult(
                context.Document.WithSyntaxRoot(
                    root.ReplaceNode(
                        classDeclaration,
                        newClassDeclaration
                    )
                ));
        }
    }
}
