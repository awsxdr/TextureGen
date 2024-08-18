namespace TextureGen.SourceGenerators
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator]
    public class GenerateSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var generatorInfo = 
                context.Compilation
                    .GetSymbolsWithName(_ => true)
                    .OfType<INamedTypeSymbol>()
                    .Where(s => !s.IsAbstract && s.AllInterfaces.Any(i => i.Name == "IGenerator"))
                    .Select(GetGeneratorTypeInfo)
                    .ToArray();

            var source = $@"
namespace TextureGen;

using Generation;

public static partial class Generate
{{
    {string.Join("\r\n", generatorInfo.Select(GetGeneratorMethod))}
}}";

            context.AddSource("Generate.g.cs", source);
        }

        private static string GetGeneratorMethod(GeneratorTypeInfo generator) =>
            $@"
    public static Texture {generator.GeneratedName}({string.Join(", ", generator.Parameters.OrderBy(p => p.DefaultValue).Select(GetParameter).Prepend("ImageSize size"))}) =>
        {generator.GeneratedName}(size, new {generator.ParameterTypeName} 
        {{
            {string.Join(",\r\n            ", generator.Parameters.Select(p => $"{p.Name} = {CamelCase(p.Name)}"))}
        }});

    public static Texture {generator.GeneratedName}(ImageSize size, {generator.ParameterTypeName} parameters) =>
        new {generator.ClassName}(size).Generate(parameters);
";

        private static string GetParameter(ParameterInfo parameter) =>
            parameter.DefaultValue == null
                ? $"{parameter.TypeName} {CamelCase(parameter.Name)}"
                : $"{parameter.TypeName} {CamelCase(parameter.Name)} = {parameter.DefaultValue}";

        private static string CamelCase(string value) =>
            string.IsNullOrEmpty(value)
                ? value
                : value[0].ToString().ToLower() + value.Substring(1);

        private GeneratorTypeInfo GetGeneratorTypeInfo(INamedTypeSymbol symbol)
        {
            var parametersType =
                symbol.AllInterfaces.Single(i => i.Name == "IGenerator")
                    .TypeArguments.Single();

            var parameterMembers = parametersType.GetMembers().OfType<IPropertySymbol>().ToArray();

            return new GeneratorTypeInfo
            {
                ClassName = symbol.Name,
                GeneratedName = symbol.Name.Substring(0, symbol.Name.Length - "Generator".Length),
                Parameters = parameterMembers.Select(GetParameterInfo).ToArray(),
                ParameterTypeName = parametersType.ToString()
            };
        }

        private ParameterInfo GetParameterInfo(IPropertySymbol parameter)
        {
            var syntax = (PropertyDeclarationSyntax)parameter.DeclaringSyntaxReferences.First().GetSyntax();
            var initializer = syntax.Initializer;

            return new ParameterInfo
            {
                Name = parameter.Name,
                TypeName = parameter.Type.ToString(),
                DefaultValue = initializer?.Value.ToFullString(),
            };
        }

        private class GeneratorTypeInfo
        {
            public string ClassName { get; set; }
            public string GeneratedName { get; set; }
            public ParameterInfo[] Parameters { get; set; }
            public string ParameterTypeName { get; set; }
        }

        public class ParameterInfo
        {
            public string Name { get; set; }
            public string TypeName { get; set; }
            public string DefaultValue { get; set; }
        }
    }
}
