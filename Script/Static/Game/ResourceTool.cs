using DEBUG;
using Godot;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Game;
/// <summary>
/// 场景资源加载器
/// </summary>
public static class ResourceTool
{
    /// <summary>
    /// 已加载的资源
    /// </summary>
    public static Godot.Collections.Array<PackedScene> Load_Scene = new Godot.Collections.Array<PackedScene>();
    /// <summary>
    /// 资源路径
    /// </summary>
    public static Godot.Collections.Array<String> Scene_Path = new Godot.Collections.Array<string>();
    /// <summary>
    /// 资源Uid
    /// </summary>
    public static Godot.Collections.Array<String> Scene_UID = new Godot.Collections.Array<string>();
    public static Godot.Collections.Array<String> Load_List = new Godot.Collections.Array<string>();
    /// <summary>
    /// 搜索模式
    /// </summary>
    public enum Search_Mode
    {
        /// <summary>
        /// 自动搜索
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Uid搜索
        /// </summary>
        Uid = 1,
        /// <summary>
        /// 路径搜索
        /// </summary>
        Path = 2,
    }
    /// <summary>
    /// 加载已完成索引的路径
    /// </summary>
    /// <param name="Path"></param>
    /// <returns></returns>
    public static PackedScene LoadScene(String Path)
    {
        if (Path[0..3] == "res")
        {
            if (Scene_Path.IndexOf(Path) == -1)
            {
                PackedScene scene = GD.Load<PackedScene>(Path);
                if (Load_Scene.IndexOf(scene) == -1){
                    Load_Scene.Add(scene);
                    Scene_Path.Add(Path);
                    Scene_UID.Add("");
                    Info.Print(Scene_Path);
                    Info.Print("路径加载完成:",Path);
                }
                else
                {
                    Scene_Path[Load_Scene.IndexOf(scene)] = Path;
                }
                
            }
            return Load_Scene[Scene_Path.IndexOf(Path)];
        }
        else if (Path[0..3] == "uid")
        {
            if (Scene_Path.IndexOf(Path) == -1)
            {
                PackedScene scene = GD.Load<PackedScene>(Path);
                if (Load_Scene.IndexOf(scene) == -1){
                    Load_Scene.Add(scene);
                    Scene_UID.Add(Path);
                    Scene_Path.Add("");
                    Info.Print(Scene_UID);
                    Info.Print("UID索引加载完成:",Path);
                }
                else
                {
                    Scene_UID[Load_Scene.IndexOf(scene)] = Path;
                }
            }
            return Load_Scene[Scene_UID.IndexOf(Path)];
        }
        return null;
        
    }
}