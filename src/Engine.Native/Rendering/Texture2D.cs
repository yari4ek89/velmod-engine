using Silk.NET.OpenGL;
using StbImageSharp;

namespace Engine.Native.Rendering;

public class Texture2D
{
    private GL _gl;
    private uint _glTexID;
    public int Width { get; }
    public int Height { get; }
    
    public unsafe Texture2D(GL openGL, string path)
    {
        _gl = openGL;
        _glTexID = _gl.GenTexture();
        
        using FileStream stream = File.OpenRead(path);
        ImageResult image = ImageResult.FromStream(
            stream,
            ColorComponents.RedGreenBlueAlpha);
        Width = image.Width;
        Height = image.Height;
        Bind();
        _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)GLEnum.Nearest);
        _gl.TexParameter(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)GLEnum.Nearest);
        fixed (byte* pixels = image.Data)
        {
            _gl.TexImage2D(
                GLEnum.Texture2D, 0, (int)GLEnum.Rgba, (uint)Width, (uint)Height, 
                0, GLEnum.Rgba, GLEnum.UnsignedByte, pixels);
        }
    }

    public void Bind()
    {
        _gl.BindTexture(TextureTarget.Texture2D, _glTexID);
    }
}