---
Title: Adding assembly references in Roslyn
Date: 2014-04-15
Tags: .net, c#, roslyn 
---

In the Roslyn preview that was released at Build 2014 the way references to global assmeblies are added was changed. Before the preview I could use code like this:

```csharp
var compilation = Compilation.Create(assemblyName, new CompilationOptions(OutputKind.DynamicallyLinkedLibrary))
	.AddReferences(MetadataReference.CreateAssemblyReference("mscorlib"))
	.AddReferences(MetadataReference.CreateAssemblyReference("System"))
	.AddReferences(MetadataReference.CreateAssemblyReference("System.Core"))
	.AddReferences(new MetadataFileReference(this.GetType().Assembly.Location))
	.AddSyntaxTrees(syntaxTree);
```

The static factory method "MetadataReference.CreateAssemblyReference" added a reference to global assemblies such as "mscorlib.dll" or "System.dll". In the Roslyn preview,
the same be achieved like so:

```csharp
var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

var compilation = CSharpCompilation.Create(assemblyName)
	.WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
	.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "mscorlib.dll")))
	.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.dll")))
	.AddReferences(new MetadataFileReference(Path.Combine(assemblyPath, "System.Core.dll")))
	.AddReferences(new MetadataFileReference(Assembly.GetEntryAssembly().Location))
	.AddSyntaxTrees(syntaxTree);
```
