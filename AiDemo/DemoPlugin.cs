using Microsoft.SemanticKernel;
using System.ComponentModel;

internal class DemoPlugin()
{
    [KernelFunction, Description("Gets your full name.")]
    [return: Description("Returns your full name.")]
    public string GetFullName()
    {
        return "AI Demo";
    }
}