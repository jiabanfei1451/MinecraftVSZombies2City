using System;
using System.IO;

namespace Godot.IO;

public static class LoadText
{
    /// <summary>
    /// 读取内部文本
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static String Get_ResourceText(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        var File = ResourceLoader.Load<Json>(ProjectPath);
        GD.Print(File.Data.AsString());
        return File.Data.AsString();
    }
    /// <summary>
    /// 获取文本内容
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static String Get_FileText(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        var texts = File.ReadAllText(ProjectPath);
        return texts;
    }
    /// <summary>
    /// 获取文件流
    /// </summary>
    /// <param name="Path"></param>
    /// <param name="fileMode"></param>
    /// <returns></returns>
    public static FileStream GetFileStream(String Path,FileMode fileMode = FileMode.OpenOrCreate)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        return File.Open(ProjectPath, fileMode);
    }
    /// <summary>
    /// 获取文本
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static StreamReader GetStreamReader(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        return File.OpenText(Path);
    }
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static FileStream Create_File(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        return File.Create(ProjectPath);
    }
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static DirectoryInfo Create_Directory(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        return Directory.CreateDirectory(ProjectPath);
    }
    public static bool Has_Directory(String Path)
    {
        String ProjectPath = ProjectSettings.GlobalizePath(Path);
        try
        {
            Directory.GetDirectories(ProjectPath);
        }catch
        {
            return false;
            
        }
        finally
        {
            
        }
        return true;
    }
}