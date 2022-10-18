namespace JustinWritesCode.Enumerations.CodeGeneration;

public static partial class Constants
{
    public const string EnumerationFieldDeclaration = @"
public static readonly {EnumerationClassName} {FieldName} = new {EnumerationClassName}({Id}, {Name});
";
}