using Godot;
using System;
namespace Touch;
public static class Touch_Index
{
    public static Godot.Collections.Array<int> TouchIndex = new Godot.Collections.Array<int>(){};
    public static Godot.Collections.Array<bool> TouchIndex_Enable = new Godot.Collections.Array<bool>(){};
   
    /// <summary>
    /// 清除索引
    /// 请谨慎使用，这会导致某些场景的本来不开启的UI启用
    /// </summary>
    public static void clear()
    {
        TouchIndex.Clear();
        TouchIndex_Enable.Clear();
    }
    /// <summary>
    /// 设置索引是否启用
    /// 当检测到空索引时，会自动添加索引并设置为启用
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="Enable"></param>

    public static void Set_Index_Enable(int Index,bool Enable)
    {
        if (TouchIndex.IndexOf(Index) == -1)
        {
            TouchIndex.Add(Index);
            TouchIndex_Enable.Add(true);
        }
        int GetIndex = TouchIndex.IndexOf(Index);
        TouchIndex_Enable[GetIndex] = Enable;

    }
    /// <summary>
    /// 获取索引是否启用
    /// 当检测到空索引时，会自动添加索引并设置为启用
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>

    public static bool Get_Index(int Index)
    {
        if (TouchIndex.IndexOf(Index) == -1)
        {
            TouchIndex.Add(Index);
            TouchIndex_Enable.Add(true);
        }

        int GetIndex = TouchIndex.IndexOf(Index);
        bool GetEnable = TouchIndex_Enable[GetIndex];

        return GetEnable;

    }
}
