using Mono.Cecil;
using System;
using System.Linq;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = AssemblyDefinition.ReadAssembly("C:\\Users\\peter\\source\\repos\\peteri\\AttributeRemover\\AttributeRemover.TestAssembly\\bin\\Debug\\publish\\netcoreapp3.1\\AttributeRemover.TestAssembly.dll");
            var attributesToRemove = assembly.CustomAttributes
                .Where(ca => ca.AttributeType.FullName == "System.Runtime.CompilerServices.InternalsVisibleToAttribute")
                .ToArray();
            foreach (var attr in attributesToRemove)
                assembly.CustomAttributes.Remove(attr);
            assembly.Write();
        }
    }
}
