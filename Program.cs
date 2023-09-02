using System.Text;
using Mono.Options;
using AndrealImageGenerator.Api;

var type = "unknown";
var jsonStr = "";
var imgVersion = 0;
var showHelp = false;
var options = new OptionSet {
    { "t|type=", "info | best | best30", t => type = t },
    { "jb|json-base64=", "base64 encoded JSON string", jsonBase64Str => jsonStr = Encoding.UTF8.GetString(Convert.FromBase64String(jsonBase64Str)) },
    { "jf|json-file=", "JSON file", file => jsonStr = File.ReadAllText(file, Encoding.UTF8) },
    { "v|imgVersion=", "image version", (int v) => imgVersion = v },
    { "h|help", "show this message and exit", h => showHelp = h != null },
};

List<string> extra;
try
{
    extra = options.Parse(args);

    if (showHelp)
    {
        Console.WriteLine("Andreal ImageGenerator cli");
        Console.WriteLine("Options:");
        options.WriteOptionDescriptions(Console.Out);
        return;
    }

    if (!(type == "info" || type == "best" || type == "best30"))
    {
        throw new Exception("Unknown type.");
    }

    if (imgVersion == 0)
    {
        if (type == "info" || type == "best") { imgVersion = 3; }
        else if (type == "best30") { imgVersion = 2; }
        else { imgVersion = 1; }
    }

    byte[] imgBytes = Array.Empty<byte>();
    if (type == "info") { imgBytes = Api.GetUserInfo(jsonStr, imgVersion); }
    else if (type == "best") { imgBytes = Api.GetUserBest(jsonStr, imgVersion); }
    else if (type == "best30") { imgBytes = Api.GetUserBest30(jsonStr, imgVersion); }

    Console.WriteLine($"data:image/png;base64,{Convert.ToBase64String(imgBytes)}");
    return;
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
    Console.WriteLine("Try `--help` for more information.");
    throw;
}
