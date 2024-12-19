using System.Collections.Generic;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BatchProcess.Models.OpenGL;

public class Texture
{
    private readonly GL _gl;

    enum ImageType
    {
        Rgba,
        Rgb
    }
    
    public Texture(GL gl)
    {
        _gl = gl;
        Handle = _gl.GenTexture();
    }

    public void LoadFromImageRGB(string imagePath)
    {
        var image = Image.Load<Rgb24>(imagePath);
        //ImageSharp counts (0, 0) as top-left, OpenGL wants it to be bottom-left. fix.
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
        List<byte> pixels = new(3 * image.Width * image.Height);
        image.ProcessPixelRows(pixelAccessor =>
        {
            for (var col = 0; col < pixelAccessor.Width; col++)
            {
                var pixelRow = pixelAccessor.GetRowSpan(col);
                for (var row = 0; row < pixelAccessor.Height; row++)
                {
                    pixels.Add(pixelRow[row].R);
                    pixels.Add(pixelRow[row].G);
                    pixels.Add(pixelRow[row].B);
                }
            }
        });
        
        _gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.Rgb, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels.ToArray());
        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }

    public void LoadFromImageRGBA(string imagePath)
    {
        var image = Image.Load<Rgba32>(imagePath);
        //ImageSharp counts (0, 0) as top-left, OpenGL wants it to be bottom-left. fix.
        image.Mutate(x => x.Flip(FlipMode.Vertical));

        //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
        List<byte> pixels = new(4 * image.Width * image.Height);
        image.ProcessPixelRows(pixelAccessor =>
        {
            for (var col = 0; col < pixelAccessor.Width; col++)
            {
                var pixelRow = pixelAccessor.GetRowSpan(col);
                for (var row = 0; row < pixelAccessor.Height; row++)
                {
                    pixels.Add(pixelRow[row].R);
                    pixels.Add(pixelRow[row].G);
                    pixels.Add(pixelRow[row].B);
                    pixels.Add(pixelRow[row].A);
                }
            }
        });
        
        _gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }
    public uint Handle { get; set; }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(unit);
        _gl.BindTexture(TextureTarget.Texture2D, Handle);
    }
}