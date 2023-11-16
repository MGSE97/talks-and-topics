using Microsoft.CodeAnalysis.Testing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text;

namespace Analyzer.Test.Verifiers
{
    internal static class Packages
    {
        // include any nuget packages to reduce the number of errors
        private static readonly PackageIdentity[] packages = new[] {
            new PackageIdentity("Microsoft.AspNetCore.App", "2.2.8"),
        };

        public static readonly ReferenceAssemblies ReferenceAssemblies = new ReferenceAssemblies(
                "net6.0",
                new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
                Path.Combine("ref", "net6.0")
            );
            //.AddPackages(packages.ToImmutableArray());
    }
}
