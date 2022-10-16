using System;
using System.Diagnostics;
using CodeGeneration.Roslyn;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    [CodeGenerationAttribute("Attributes.Generators.HelloWorldGenerator, Attributes.Generators")]
    [Conditional("CodeGeneration")]
    public class HelloWorldAttribute : Attribute
    {
    }
}

