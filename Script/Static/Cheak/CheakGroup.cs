using System;
using Godot;
using Microsoft.VisualBasic;
namespace Game.Cheak;
/// <summary>
/// 检测阵营
/// </summary>
public static class CheakGroup
{
    /// <summary>
    /// 相当于同时执行了
    /// <para>Whether_Include_Group()</para>
    /// <para>Exclude_Group()</para>
    /// <para>如果存在被排除阵营返回false,如果不存在需要被检测的阵营返回false</para>
    /// </summary>
    /// <param name="Cheak_Node">需要检测的物体节点</param>
    /// <param name="Groups">需要检测的阵营</param>
    /// <param name="Exclude">需要排除的阵营</param>
    /// <returns></returns>
    public static bool Cheak_Object_Group(Node Cheak_Node,Godot.Collections.Array<StringName> Groups,Godot.Collections.Array<StringName> Exclude)
    {
        Godot.Collections.Array<StringName> GetGroups = Cheak_Node.GetGroups();
        if (Exclude_Group(GetGroups, Exclude))
        {
            return false;
        }
        if (!Whether_Include_Group(Cheak_Node,Groups)){return false;}
        return true;
    }
    /// <summary>
    /// 当如果包含阵营时返回true 否则返回 false
    /// </summary>
    /// <param name="Cheak_Node">需要检测的物体节点</param>
    /// <param name="Groups">检测阵营列表</param>
    /// <returns></returns>
    public static bool Whether_Include_Group(Node Cheak_Node,Godot.Collections.Array<StringName> Groups)
    {
        foreach(StringName Node_Group in Cheak_Node.GetGroups())
        {
            if (Groups.IndexOf(Node_Group) != -1)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 如果包含排除阵营时返回true 否则 false
    /// </summary>
    /// <param name="Exclude">需要的排除阵营</param>
    /// <param name="Groups">要用于检测的阵营</param>
    /// <returns></returns>
    public static bool Exclude_Group(Godot.Collections.Array<StringName> Exclude,Godot.Collections.Array<StringName> Groups)
    {
        foreach(StringName Group in Groups)
        {
            if (Exclude.IndexOf(Group) != -1)
            {
                return true;
            }
        }
        return false;
    }
}