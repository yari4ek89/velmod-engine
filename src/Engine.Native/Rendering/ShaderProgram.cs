using Silk.NET.OpenGL;

namespace Engine.Native.Rendering;

public class ShaderProgram
{
    private GL _gl;
    private uint _shaderProgram;
    
    public ShaderProgram(GL openGL)
    {
        _gl = openGL;
    }

    public void ShaderProcess(string vertexPath, string fragmentPath)
    {
        // GLSL reading
        string vertexSource = ReadGLSL(vertexPath);
        string fragmentSource = ReadGLSL(fragmentPath);
        
        // Shader CC (creating and compilling)
        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        ShaderSource(vertexShader, vertexSource);
        ShaderSource(fragmentShader, fragmentSource);
        ShaderCompile(vertexShader);
        ShaderCompile(fragmentShader);
        
        // Shader program creating, attaching, linking
        _shaderProgram = _gl.CreateProgram();
        ShaderProgramAttaching(vertexShader);
        ShaderProgramAttaching(fragmentShader);
        _gl.LinkProgram(_shaderProgram);
        ShaderProgramChecking();
        
        // Removing shader objects
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
    }

    public void Use()
    {
        _gl.UseProgram(_shaderProgram);
    }

    public void SetVector2(string name, float firstCoord, float secondCoord)
    {
        int uniformLoc = _gl.GetUniformLocation(_shaderProgram, name);
        _gl.Uniform2(uniformLoc, firstCoord, secondCoord);
    }

    private string ReadGLSL(string path)
    {
        return File.ReadAllText(path);
    }

    private void ShaderSource(uint shader, string source)
    {
        _gl.ShaderSource(shader, source);
    }
    
    private void ShaderCompile(uint shader)
    {
        _gl.CompileShader(shader);
        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out int status);
        CompilingChecking(shader, status);
    }

    private void CompilingChecking(uint shader, int status)
    {
        if (status == 0)
        {
            string log = _gl.GetShaderInfoLog(shader);
            Console.WriteLine(log);
        }
    }

    private void ShaderProgramAttaching(uint shader)
    {
        _gl.AttachShader(_shaderProgram, shader);
    }

    private void ShaderProgramChecking()
    {
        _gl.GetProgram(_shaderProgram, GLEnum.LinkStatus, out int status);
        if (status == 0)
        {
            string log = _gl.GetProgramInfoLog(_shaderProgram);
            Console.WriteLine(log);
        }
    }
}