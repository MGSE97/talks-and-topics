using Microsoft.CodeAnalysis;

namespace Analyzer
{
    internal class DiagnosticDescriptorBuilder
    {
        public string Id { get; }
        public LocalizableString Category { get; }
        public LocalizableString Title { get; }
        public LocalizableString MessageFormat { get; }
        public LocalizableString Description { get; }
        public DiagnosticSeverity Severity { get; }

        public DiagnosticDescriptorBuilder(int id, DiagnosticSeverity severity)
        {
            var ResourceId = $"AG.{id}.";
            Id = $"AG{id}";
            Severity = severity;
            Category = new LocalizableResourceString($"{ResourceId}Category", Resources.ResourceManager, typeof(Resources));
            Title = new LocalizableResourceString($"{ResourceId}Title", Resources.ResourceManager, typeof(Resources));
            MessageFormat = new LocalizableResourceString($"{ResourceId}MessageFormat", Resources.ResourceManager, typeof(Resources));
            Description = new LocalizableResourceString($"{ResourceId}Description", Resources.ResourceManager, typeof(Resources));
        }

        public DiagnosticDescriptor Build() => new DiagnosticDescriptor(Id, Title, MessageFormat, Category.ToString(), Severity, isEnabledByDefault: true, description: Description);
    }
}
