﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Path = AndrealImageGenerator.Common.Path;

namespace AndrealImageGenerator.Graphics;

#pragma warning disable CA1416

internal class Image : IDisposable
{
    protected readonly Bitmap Bitmap;

    private protected bool Alreadydisposed;

    internal Image(Path path)
    {
        Bitmap = new(path);
        Bitmap.SetResolution(96, 96);
    }

    internal Image(int width, int height)
    {
        Bitmap = new(width, height);
        Bitmap.SetResolution(96, 96);
    }

    internal Image(Bitmap bitmap)
    {
        lock (bitmap) Bitmap = bitmap;
        Bitmap.SetResolution(96, 96);
    }

    internal Image(Image origin, int width, int height)
    {
        lock (origin) Bitmap = new(origin.Bitmap, width, height);
        Bitmap.SetResolution(96, 96);
    }

    internal Image(Stream stream)
    {
        lock (stream) Bitmap = (Bitmap)System.Drawing.Image.FromStream(stream);
        Bitmap.SetResolution(96, 96);
    }

    internal int Width => Bitmap.Width;

    internal int Height => Bitmap.Height;

    internal System.Drawing.Color MainColor => DeserializeColor();

    public void Dispose()
    {
        if (Alreadydisposed) return;
        Bitmap.Dispose();
        GC.SuppressFinalize(this);
        Alreadydisposed = true;
    }

    ~Image()
    {
        Bitmap.Dispose();
    }

    private System.Drawing.Color DeserializeColor()
    {
        var bm = Bitmap.LockBits(new(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        var ptr = bm.Scan0;
        var bytes = Math.Abs(bm.Stride) * Bitmap.Height;
        var rgbValues = new byte[bytes];
        Marshal.Copy(ptr, rgbValues, 0, bytes);
        Bitmap.UnlockBits(bm);
        long red = 0, green = 0, blue = 0, count = 10;
        for (var counter = 0; counter < rgbValues.Length; counter += 12)
        {
            blue += rgbValues[counter];
            green += rgbValues[counter + 1];
            red += rgbValues[counter + 2];
            ++count;
        }

        count = count / 2 * 3;
        var col = System.Drawing.Color.FromArgb((byte)(red / count), (byte)(green / count), (byte)(blue / count));

        return col;
    }

    internal Image Cut(Rectangle rectangle) => new(Bitmap.Clone(rectangle, PixelFormat.Format32bppArgb));

    internal void SaveAsPng(Path path) => Bitmap.Save(path, ImageFormat.Png);

    internal void SaveAsPng(Stream stream) => Bitmap.Save(stream, ImageFormat.Png);

    internal void SaveAsJpgWithQuality(Stream stream, int quality = 50) => ImageExtend.SaveAsJpeg(Bitmap, stream, quality);

    internal static class ImageExtend
    {
        private static ImageCodecInfo? _imageCodecInfo;
        private static ImageCodecInfo ImageCodecInfo => _imageCodecInfo ??= GetCodecInfo("image/jpeg")!;

        internal static void PngWithWhiteBg(Path originPath, Path newPath)
        {
            using Image img = new(originPath);
            using var bmpTemp = new Bitmap(img.Width, img.Height);
            using var g = System.Drawing.Graphics.FromImage(bmpTemp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(System.Drawing.Color.White);
            DrawImage(g, img, 0, 0, img.Width, img.Height);
            bmpTemp.Save(newPath, ImageFormat.Png);
        }

        internal static void DrawImage(
            System.Drawing.Graphics g,
            Image image,
            int posX,
            int posY)
            => g.DrawImage(image.Bitmap, posX, posY);

        internal static void DrawImage(
            System.Drawing.Graphics g,
            Image image,
            int posX,
            int posY,
            int newWidth,
            int newHeight)
            => g.DrawImage(image.Bitmap, posX, posY, newWidth, newHeight);

        private static ImageCodecInfo? GetCodecInfo(string mimeType)
            => ImageCodecInfo.GetImageEncoders().FirstOrDefault(ici => ici.MimeType == mimeType);

        public static void SaveAsJpeg(Bitmap bmp, Stream stream, int quality)
        {
            var ps = new EncoderParameters { Param = { [0] = new(Encoder.Quality, quality) } };
            bmp.Save(stream, ImageCodecInfo, ps);
        }
    }
}
