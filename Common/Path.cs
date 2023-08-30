using AndrealImageGenerator.Beans;

namespace AndrealImageGenerator.Common;

[Serializable]
public class Path
{
    private static readonly string BaseDirectory = AppContext.BaseDirectory;

    public static readonly string ArcaeaBackgroundRoot = System.IO.Path.Join(BaseDirectory, "Andreal", "Background"),
                                  ArcaeaImageRoot = System.IO.Path.Join(BaseDirectory, "Andreal", "Arcaea"),
                                  ArcaeaSourceRoot = System.IO.Path.Join(BaseDirectory, "Andreal", "Source"),
                                  ArcaeaFontRoot = System.IO.Path.Join(BaseDirectory, "Andreal", "Fonts"),
                                  AndreaConfigRoot = System.IO.Path.Join(BaseDirectory, "Andreal", "Config");

    public static readonly Path PartnerConfig = new(System.IO.Path.Join(AndreaConfigRoot, "positioninfo.json")),
                                ArcSong = new(System.IO.Path.Join(AndreaConfigRoot, "arcsong.json")),
                                TmpSongList = new(System.IO.Path.Join(AndreaConfigRoot, "tempsonglist.json")),
                                ArcaeaDivider = new(System.IO.Path.Join(ArcaeaSourceRoot, "Divider.png")),
                                ArcaeaGlass = new(System.IO.Path.Join(ArcaeaSourceRoot, "Glass.png")),
                                ArcaeaBest30Bg = new(System.IO.Path.Join(ArcaeaSourceRoot, "B30.png")),
                                ArcaeaBest40Bg = new(System.IO.Path.Join(ArcaeaSourceRoot, "B40.png")),
                                ArcaeaBg1Mask = new(System.IO.Path.Join(ArcaeaSourceRoot, "Mask.png")),
                                ExceptionReport = new(System.IO.Path.Join(BaseDirectory, "Andreal", "ExceptionReport.log"));

    private readonly string _rawpath;

    private FileInfo? _fileInfo;

    static Path()
    {
        Directory.CreateDirectory(ArcaeaBackgroundRoot);
        Directory.CreateDirectory(ArcaeaSourceRoot);
        Directory.CreateDirectory(ArcaeaFontRoot);
        Directory.CreateDirectory(System.IO.Path.Join(ArcaeaImageRoot, "Song"));
        Directory.CreateDirectory(System.IO.Path.Join(ArcaeaImageRoot, "Char"));
        Directory.CreateDirectory(System.IO.Path.Join(ArcaeaImageRoot, "Icon"));
    }

    private Path(string rawpath)
    {
        _rawpath = rawpath;
    }

    public FileInfo FileInfo => _fileInfo ??= new(this);

    public static Path ArcaeaBackground(int version, ArcaeaChart chart) => new(System.IO.Path.Join(ArcaeaBackgroundRoot, $"V{version}_{ArcaeaTempSong(chart)}.png"));

    public static Path ArcaeaBg3Mask(Side side) => new(System.IO.Path.Join(ArcaeaSourceRoot, $"RawV3Bg_{(int)side}.png"));

    public static async Task<Path> ArcaeaSong(ArcaeaChart chart)
    {
        var song = ArcaeaTempSong(chart);

        var pth = new Path(System.IO.Path.Join(ArcaeaImageRoot, "Song", $"{song}.jpg"));

        if (pth.FileInfo.Exists)
        {
            if (pth.FileInfo.Length > 10240) return pth;
            pth.FileInfo.Delete();
        }

        // await ArcaeaUnlimitedAPI.SongAssets(chart.SongID, chart.RatingClass, pth);

        return pth;
    }

    private static string ArcaeaTempSong(ArcaeaChart chart)
    {
        var song = chart switch
        {
            _ when chart.JacketOverride => $"{chart.SongID}_{chart.RatingClass}",
            _ when chart.SongID == "melodyoflove" => $"melodyoflove{(DateTime.Now.Hour is > 19 or < 6 ? "_night" : "")}",
            _ => chart.SongID
        };
        return song;
    }

    public static Path ArcaeaRating(short potential)
    {
        var img = potential switch
        {
            >= 1300 => "7",
            >= 1250 => "6",
            >= 1200 => "5",
            >= 1100 => "4",
            >= 1000 => "3",
            >= 700 => "2",
            >= 350 => "1",
            >= 0 => "0",
            < 0 => "off"
        };
        return new(System.IO.Path.Join(ArcaeaSourceRoot, $"rating_{img}.png"));
    }

    public static async Task<Path> ArcaeaPartner(int partner, bool awakened)
    {
        var pth = new Path(System.IO.Path.Join(ArcaeaImageRoot, "Char", $"{partner}{(awakened ? "u" : "")}.png"));

        if (pth.FileInfo.Exists)
        {
            if (pth.FileInfo.Length > 10240) return pth;
            pth.FileInfo.Delete();
        }

        // await ArcaeaUnlimitedAPI.CharAssets(partner, awakened, pth);

        return pth;
    }

    public static async Task<Path> ArcaeaPartnerIcon(int partner, bool awakened)
    {
        var pth = new Path(System.IO.Path.Join(ArcaeaImageRoot, "Icon", $"{partner}{(awakened ? "u" : "")}.png"));

        if (pth.FileInfo.Exists)
        {
            if (pth.FileInfo.Length > 10240) return pth;
            pth.FileInfo.Delete();
        }

        // await ArcaeaUnlimitedAPI.IconAssets(partner, awakened, pth);

        return pth;
    }

    public static Path ArcaeaCleartypeV1(sbyte cleartype) => new(System.IO.Path.Join(ArcaeaSourceRoot, $"end_{cleartype}.png"));

    public static Path ArcaeaCleartypeV3(sbyte cleartype) => new(System.IO.Path.Join(ArcaeaSourceRoot, $"clear_{cleartype}.png"));

    public static Path ArcaeaDifficultyForV1(int difficulty) => new(System.IO.Path.Join(ArcaeaSourceRoot, $"con_{difficulty}.png"));

    public static implicit operator string(Path path) => path._rawpath;
}
