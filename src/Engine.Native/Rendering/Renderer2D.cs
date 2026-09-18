using Engine.Core;
using Silk.NET.OpenGL;

namespace Engine.Native.Rendering;

public class Renderer2D
{
    private GL _gl;
    private uint _vao;
    private ShaderProgram _shaderProgram;
    private readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
    
    public Renderer2D(GL openGL, ShaderProgram shaderProgram)
    {
        _gl = openGL;
        _shaderProgram = shaderProgram;
        GeometryProcessing();
    }

    public void Render(double width, double height, Scene scene)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _shaderProgram.Use();
        
        foreach (var gameObject in scene.GameObjects)
        {
            SpriteRenderer? currentSR = gameObject.GetComponent<SpriteRenderer>();
            if (currentSR != null && currentSR.Visible)
            {
                Texture2D texture;
                if (!_textures.TryGetValue(currentSR.SpritePath, out texture)) 
                { 
                    texture = new Texture2D(_gl, currentSR.SpritePath); 
                    _textures.Add(currentSR.SpritePath, texture);
                }
                
                double xNdc = (2 * gameObject.Transform.X) / width - 1;
                double yNdc = 1 - (2 * gameObject.Transform.Y) / height;
                
                _shaderProgram.SetVector2(
                    "uPosition",
                    (float)xNdc,
                    (float)yNdc
                );
                
                texture.Bind();
                _gl.BindVertexArray(_vao);
                _gl.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
        }
    }
    
    private unsafe void GeometryProcessing()
    {
        float[] vertices =
        {
            0.0f, 0.0f, 0.0f, 0.0f,
            0.0f, -0.5f, 0.0f, 1.0f,
            0.5f, -0.5f, 1.0f, 1.0f,
            0.0f, 0.0f, 0.0f, 0.0f,
            0.5f, -0.5f, 1.0f, 1.0f,
            0.5f, 0.0f, 1.0f, 0.0f
        };

        uint vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, BufferUsageARB.StaticDraw);

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        _gl.VertexAttribPointer(0u, 2, VertexAttribPointerType.Float, false, (uint)(4 * sizeof(float)), (void*)(0));
        _gl.VertexAttribPointer(1u, 2, VertexAttribPointerType.Float, false, (uint)(4 * sizeof(float)), (void*)(2 * sizeof(float)));
        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);
    }
}