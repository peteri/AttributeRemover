# AttributeRemover
Removes Attributes from .net Assemblies.

Adds an msbuild target to allow removal of Attributes during the MSBuild process.

Designed to allow stripping of InternalsVisibleTo attributes from an assembly before signing 
so the InternalsVisibleTo Attributes don't need a public key.