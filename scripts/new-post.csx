#! "netcoreapp2.1"

var now = DateTime.Now;

string source = Path.Combine("templates", "new-post.md");

string template = File.ReadAllText(source);

template = template
    .Replace("{{title}}", Args[0])
    .Replace("{{date}}", $"{now.Year}-{now.Month.ToString("0#")}-{now.Day.ToString("0#")}");

string directory = Path.Combine("source", "blog", now.Year.ToString());

if (!Directory.Exists(directory))
{
    Console.WriteLine($"Creating directory \"{directory}\".");
    Directory.CreateDirectory(directory);
}

string fileName = Args[0].ToLower().Replace(" ", "-") + ".md";

var namesToReplace = new Dictionary<string, string>()
{
    ["c#"] = "csharp"
};

foreach (var nameToReplace in namesToReplace)
    fileName = fileName.Replace(nameToReplace.Key, nameToReplace.Value);

string destination = Path.Combine(directory, fileName);

Console.WriteLine($"Writing \"{destination}\"...");
File.WriteAllText(destination, template);
