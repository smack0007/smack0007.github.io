#! "netcoreapp2.0"

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

string destination = Path.Combine(directory, fileName);

Console.WriteLine($"Writing \"{destination}\"...");
File.WriteAllText(destination, template);
