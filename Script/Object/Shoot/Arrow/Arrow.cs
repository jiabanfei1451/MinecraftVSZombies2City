using System;
using Godot;
namespace Level.Object.Bullet;
/// <summary>
/// 箭矢
/// </summary>
public partial class Arrow : Level.Object.BulletData
{
    public override void _Ready() {
        base._Ready();
    }
    public override void _Process(double delta) {
        base._Process(delta);
        Position += Get_Rotation_Vector(Rotation / 3.14f * 180) * 60 * Speed * (float)delta;
    }
    public void Reset()
    {
        Node2D s = this;
        if (s is Area2D)
        {
            ((Area2D)s).BodyEntered += Object_Join;
            ((Area2D)s).BodyExited += Object_Exit;
        }
    }
    public void Object_Join(Node2D node)
    {
        if (node is Level.Object.LevelObject){
            Add_Check_Object((Level.Object.LevelObject)node);
        }
        if (Check_Object.Count > 0)
        {
            Check_Object[0].Reduce_Health(Damage);
        }
    }
    public void Object_Exit(Node2D node)
    {
        if (node is Level.Object.LevelObject)
        {
            Remove_Check_Object((Level.Object.LevelObject)node);
        }
    }
}
