using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Pan.Avalonia.OpenGL.Models;

public class Texture
{
    enum ImageType
    {
        Rgba,
        Rgb
    }
    
    public Texture() 
    {
        Handle = GL.GenTexture();
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
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, image.Width, image.Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels.ToArray());
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
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
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
    public int Handle { get; set; }

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}