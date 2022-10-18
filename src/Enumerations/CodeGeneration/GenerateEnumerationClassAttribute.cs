namespace JustinWritesCode.Enumerations.CodeGeneration;

public static partial class Constants
{
    public const string GenerateEnumerationClassAttributeDeclaration =
@"[AttributeUsage(AttributeTargets.Class)] internal class GenerateEnumerationClassAttribute : Attribute 
{
    public string EnumerationClassName { get; }
    public string Namespace { get; }
    public string ToStringProperty { get; }
    public string ComparisonProperty { get; }

    public GenerateEnumerationClassAttribute(string EnumerationClassName = null, string Namespace =  null, string ToStringProperty =  null, string ComparisonProperty = null) 
    {
        this.EnumerationClassName = EnumerationClassName;
        this.Namespace = Namespace;
        this.ToStringProperty = ToStringProperty;
        this.ComparisonProperty = ComparisonProperty;
     } 
}
";
}
