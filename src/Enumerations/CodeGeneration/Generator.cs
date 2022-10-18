namespace JustinWritesCode.Enumerations.CodeGeneration;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System;
using static JustinWritesCode.Enumerations.CodeGeneration.Constants;

[Generator]
public class Generator : IIncrementalGenerator
{
    public const string GenerateEnumerationClassAttributeName = nameof(GenerateEnumerationClassAttributeName);
    public const string EnumerationClassName = nameof(EnumerationClassName);
    public const string FieldName = nameof(FieldName);
    public const string Namespace = nameof(Namespace);
    public const string Fields = nameof(Fields);
    public const string Values = nameof(Values);
    public const string Filename = nameof(Filename);
    public const string AuthorEmail = nameof(AuthorEmail);
    public const string AuthorName = nameof(AuthorName);
    public const string EnumType = nameof(EnumType);
    public const string Timestamp = nameof(Timestamp);
    public const string Year = nameof(Year);
    public const string Name = nameof(Name);
    public const string Id = nameof(Id);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var enumDeclarations = context.SyntaxProvider.CreateSyntaxProvider(
           predicate: (s, t) => s is EnumDeclarationSyntax,
           transform: GetTypeSymbols).Collect();

        context.RegisterSourceOutput(enumDeclarations, GenerateSource);
    }

    private void GenerateSource(SourceProductionContext context, ImmutableArray<ITypeSymbol> typeSymbols)
    {
        var codeHeader = CodeHeader.Replace($"{{{AuthorName}}}", "JustinWritesCode")
            .Replace($"{{{AuthorEmail}}}", "justin@justinwritescode.com")
            .Replace($"{{{Year}}}", DateTime.Now.Year.ToString())
            .Replace($"{{{Timestamp}}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"));

        var sb = new StringBuilder();
        foreach (var symbol in typeSymbols)
        {
            if (symbol is null)
                continue;
            var generateEnumClassAttributeDecoration = symbol.GetAttributes().FirstOrDefault(a => a.AttributeClass.Name == GenerateEnumerationClassAttributeName);
            if (generateEnumClassAttributeDecoration != null)
            {
                var enumerationClassName = generateEnumClassAttributeDecoration.ConstructorArguments.ToArray()[0].Value.ToString();
                var @namespace = generateEnumClassAttributeDecoration.ConstructorArguments.ToArray()[1].Value.ToString();
                var toStringProperty = generateEnumClassAttributeDecoration.ConstructorArguments.ToArray()[2].Value.ToString();
                var comparisonProperty = generateEnumClassAttributeDecoration.ConstructorArguments.ToArray()[3].Value.ToString();
                var enumType = symbol.Name;

                var enumFields = symbol.GetMembers().Where(member => member.Kind == SymbolKind.Field);
                var enumClassDeclaration =
                    EnumerationClassDeclaration
                    .Replace($"{{{EnumerationClassName}}}", enumerationClassName)
                    .Replace($"{{{Namespace}}}", @namespace)
                    .Replace($"{{{EnumType}}}", enumType)
                    .Replace($"{{{Values}}}", string.Join(", ", enumFields.Select(f => $"{{{enumerationClassName}}}.{{{f.Name}}}")))
                    .Replace($"{{{Fields}}}",
                        string.Join("; ", enumFields.Select(f =>
                            EnumerationFieldDeclaration
                            .Replace($"{{{EnumerationClassName}}}", enumerationClassName)
                            .Replace($"{{{FieldName}}}", f.Name)
                            .Replace($"{{{Id}}}", ((IFieldSymbol)f).ConstantValue.ToString())
                            .Replace($"{{{Name}}}", $"\"{((IFieldSymbol)f).ConstantValue.ToString()}\""))));
                var enumClassFile = codeHeader + enumClassDeclaration;
                context.AddSource($"{enumerationClassName}.g.cs", SourceText.From(enumClassFile, Encoding.UTF8));
            }

            context.AddSource(EnumerationClassDeclaration, SourceText.From(EnumerationClassDeclaration, Encoding.UTF8));
        }

        context.AddSource(GenerateEnumerationClassAttributeName, SourceText.From(codeHeader + "\n" + GenerateEnumerationClassAttributeDeclaration, Encoding.UTF8));
    }

    private ITypeSymbol GetTypeSymbols(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        var decl = (EnumDeclarationSyntax)context.Node;

        if (context.SemanticModel.GetDeclaredSymbol(decl, cancellationToken) is ITypeSymbol typeSymbol)
        {
            return typeSymbol;
        }

        return null;
    }
}
