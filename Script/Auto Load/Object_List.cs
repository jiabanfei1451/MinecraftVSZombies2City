using Godot;
using MVZ2_City.Type;
using System;
namespace MVZ2_City;
public partial class Object_List : Node
{
    /// <summary>
    /// 实例ID
    /// </summary>
    public Godot.Collections.Array<String> Object_Name_ID = new(){};
    /// <summary>
    /// 中文名称
    /// </summary>
    public Godot.Collections.Array<String> Object_CH_Name = new(){};
    /// <summary>
    /// 实例场景
    /// </summary>
    public Godot.Collections.Array<PackedScene> Object_PackedScene = new(){};
    public override void _Ready() {
        base._Ready();
        Game.Get_GlobalNode.object_List = this;
        add_Object_Packed(Game.ResourceScene.LoadScene("uid://bx78lmkp8si7e"),"MVZ2:Zombies","僵尸");
        add_Object_Packed(Game.ResourceScene.LoadScene("res://Object/Equipment/Transmitter.tscn"),"MVZ2:Transmitter","发射器");
        add_Object_Packed(Game.ResourceScene.LoadScene("res://Object/Equipment/Furnace.tscn"),"MVZ2:Furnace","熔炉");
        add_Object_Packed(Game.ResourceScene.LoadScene("res://Object/Equipment/Obsidian.tscn"),"MVZ2:Obsidian","黑曜石");

    }
    public void add_Object_Packed(PackedScene ObjectScene,String name,String CH_Name = "null")
    {
        Object_CH_Name.Add(CH_Name);
        Object_Name_ID.Add(name);
        Object_PackedScene.Add(ObjectScene);
    }
    /// <summary>
    /// 获取场景
    /// </summary>
    /// <param name="ID_Object">实例ID</param>
    /// <returns></returns>
    public PackedScene Get_Packed(MVZ2_City.Type.ID ID_Object)
    {
        int Index = -1;
        if (ID_Object.Index_Mode == IndexMode.Name)
        {
            Index = Object_Name_ID.IndexOf(ID_Object.Object_Name_ID);
        }else if(ID_Object.Index_Mode == IndexMode.CH_Name)
        {
            
        }
        else
        {
            Index = ID_Object.Object_ID;
        }
        return Object_PackedScene[Index];
    }
    /// <summary>
    /// 获取ID
    /// </summary>
    /// <param name="Index"></param>
    /// <param name="indexMode"></param>
    /// <returns></returns>
    public MVZ2_City.Type.ID Get_ID(String Index = "0",MVZ2_City.Type.IndexMode indexMode = IndexMode.index)
    {
        MVZ2_City.Type.ID iD = new(-1,"-1");
        // 转化格式(假)
        if (indexMode == IndexMode.Name)
        {
            if (Index.IndexOf("Str:") == -1)
            {
                string Temp = Index;
                Index = "Str:" + Temp;
            }
        }
        if (indexMode == IndexMode.CH_Name)
        {
            if (Index.IndexOf("CH:") == -1)
            {
                string Temp = Index;
                Index = "CH:" + Temp;
            }
        }

        if (Index.Length > 4 && Index[0..4] == "Str:")
        {
            iD.Object_ID = Object_Name_ID.IndexOf(Index[5..]);
            iD.Object_Name_ID = Object_Name_ID[iD.Object_ID];
            iD.CH_Name = Object_CH_Name[iD.Object_ID];
        }
        else if (Index.Length > 4 && Index[0..3] == "CH:")
        {
            iD.Object_ID = Object_CH_Name.IndexOf(Index[4..]);
            iD.Object_Name_ID = Object_Name_ID[iD.Object_ID];
            iD.CH_Name = Object_CH_Name[iD.Object_ID];
        }
        else
        {
            iD.Object_ID = int.Parse(Index);
            iD.Object_Name_ID = Object_Name_ID[iD.Object_ID];
            iD.CH_Name = Object_CH_Name[iD.Object_ID];
        }
        iD.Index_Mode = indexMode;
        if (iD.Object_ID == -1)
        {
            return null;
        }
        return iD;
    }
}