using System;
using Godot;
using Level.Module;

namespace Level.Object;
/// <summary>
/// 射弹基础数据类型
/// </summary>
public partial class BulletData : ObjectPhysics
{
    [Signal] public delegate void Object_JoinEventHandler(Node2D node);
    [Signal] public delegate void Object_ExitEventHandler(Node2D node);
    [ExportGroup("Cheak_Group")]
    [Export] public Godot.Collections.Array<StringName> detection_Group = new Godot.Collections.Array<StringName>();
    [Export] public Godot.Collections.Array<StringName> Exclude_Group = new Godot.Collections.Array<StringName>(){"Bullet","Flight"}; 
    [ExportGroup("Array")]
    /// <summary>
    /// 已检测到的物体
    /// </summary>
    [Export] internal Godot.Collections.Array<Level.Object.LevelObject> Check_Object = new Godot.Collections.Array<LevelObject>();
    /// <summary>
    /// 伤害
    /// </summary>
    [ExportGroup("Variant")] [Export] public int Damage = 20;
    /// <summary>
    /// 攻击完成自动移除物体
    /// </summary>
    [Export] public bool Damage_End_AutoRemove_Object = false;
    /// <summary>
    /// 速度
    /// </summary>
    [ExportGroup("Vector")]
    [Export] public Godot.Vector2 Speed = new Vector2(1,1);
    public Vector2 Get_Rotation_Vector(float Rotation = 0)
    {
        Vector2 Temp_Vector;
        Temp_Vector = Vector2.FromAngle(3.14f / 180 * Rotation);
        return Temp_Vector;
    }
    public float GEt_Rotation(Vector2 vector)
    {
        Vector2 temp_Postiion = Position - vector;
        return MathF.Atan2(temp_Postiion.Y,temp_Postiion.X); 
    }
    public void Damage_Object(int Array_Index)
    {
        Check_Object[Array_Index].Reduce_Health(10,this);
    }
    public void Add_Check_Object(Level.Object.LevelObject data_object)
    {
        if (Check_Object.IndexOf(data_object) != -1){return;}
        Check_Object.Add(data_object);
    }
    public void Remove_Check_Object(Level.Object.LevelObject data_object)
    {
        Check_Object.Remove(data_object);
    }
    internal void Join(Node2D node)
    {
        if (node is Level.Object.LevelObject){
            bool Cheak = Game.Cheak.CheakGroup.Cheak_Object_Group(node,detection_Group,Exclude_Group);
            if (!Cheak){return;}
            Add_Check_Object((Level.Object.LevelObject)node);
            EmitSignal("Object_Join",node);
        }
    }
    internal void Exit(Node2D node)
    {
        if (node is Level.Object.LevelObject)
        {
            Remove_Check_Object((Level.Object.LevelObject)node);
            EmitSignal("Object_Exit",node);
        }
    }
}
