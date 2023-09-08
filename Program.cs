using System.Text;
using Mono.Options;
using AndrealImageGenerator.Api;
using Path = AndrealImageGenerator.Common.Path;

var path = "";
var jsonStr = "";
ImgFormat imgFormat = ImgFormat.Jpg;
int imgQuality = 80;
var imgVersion = 0;
var showHelp = false;
var options = new OptionSet {
    { "p|path=", "Andreal data path", p => path = p },
    { "jb|json-base64=", "base64 encoded JSON string", jsonBase64Str => jsonStr = Encoding.UTF8.GetString(Convert.FromBase64String(jsonBase64Str)) },
    { "jf|json-file=", "JSON file", file => jsonStr = File.ReadAllText(file, Encoding.UTF8) },
    { "it|img-format=", "jpg | png", t => imgFormat = (ImgFormat)Enum.Parse(typeof(ImgFormat), t, true) },
    { "iq|img-quality=", "(JPG only) JPG image quality", (int q) => imgQuality = q },
    { "iv|img-version=", "image version", (int v) => imgVersion = v },
    { "h|help", "show this message and exit", h => showHelp = h != null },
};

List<string> extra;
try
{
    extra = options.Parse(args);

    if (showHelp)
    {
        Console.WriteLine("[AndrealImageGenerator] Andreal ImageGenerator cli");
        Console.WriteLine("Options:");
        options.WriteOptionDescriptions(Console.Out);
        return;
    }

    if (extra.Count < 1)
    {
        throw new Exception("The first argument should be one of [info, best, best30].");
    }

    string type = extra[0];

    if (!(type == "info" || type == "best" || type == "best30"))
    {
        throw new Exception($"Unknown type \"{type}\", expecting one of [info, best, best30].");
    }

    if (path.Length > 0) {
        if (System.IO.Path.IsPathRooted(path)) { Path.AndrealDirectory = path; }
        else { Path.AndrealDirectory = System.IO.Path.GetFullPath(path); }
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

    string base64Type = imgFormat == ImgFormat.Png ? "png" : "jpeg";
    Console.WriteLine($"data:image/{base64Type};base64,{Convert.ToBase64String(imgBytes)}");
    return;
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e.Message}");
    Console.WriteLine("Try `--help` for more information.");
    throw;
}
