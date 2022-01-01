using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using VsTools.Projects;

namespace GoLive.Generator.StronglyTypedEmbeddedResources
{

    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDir))
            {
                return; // todo some logging
            }

            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace))
            {
                return; // again, logging perchance?
            }

            var enumerateFiles = Directory.EnumerateFiles(projectDir, "*.csproj");
            
            if (!enumerateFiles.Any() || enumerateFiles.Count() > 1)
            {
                return; // again logging
            }

            string csProjFile = enumerateFiles.FirstOrDefault();

            var project = VsTools.Projects.Project.Load(csProjFile);
            
            var itemGroups = project.ItemGroups.Where(e=>e.Items.OfType<EmbeddedResource>().Any());

            if (itemGroups.Any())
            {
                SourceStringBuilder builder = new();
                
                builder.AppendLine($"namespace {rootNamespace}");
                builder.AppendOpenCurlyBracketLine();
                
                builder.AppendLine("public static class EmbeddedResources");
                builder.AppendOpenCurlyBracketLine();
                
                foreach (var itemGroup in itemGroups)
                {
                    var items = itemGroup.Items.OfType<EmbeddedResource>();
                    foreach (var embeddedResource in items)
                    {
                        string name = $"{embeddedResource.Include.Replace("\\", ".")}";
                    
                        builder.AppendLine($"public static string {name.Replace(".","_")} = \"{rootNamespace}.{name}\";");
                        
                    }
                }
                
                
                builder.AppendCloseCurlyBracketLine();
                builder.AppendCloseCurlyBracketLine();
                File.WriteAllText(Path.Combine(projectDir, "EmbeddedResource.g.cs"),builder.ToString());
            }
        }
    }
}