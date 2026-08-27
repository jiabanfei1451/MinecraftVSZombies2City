using System;
using Godot;
namespace Level.Object.Bullet;
/// <summary>
/// 箭矢
/// </summary>
public partial class Arrow : Level.Object.BulletData
{
    [Export] Area2D area = null;
    public override void _Ready() {
        base._Ready();
        Reset();
        Node2D node2D = Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Shoot");
        if (node2D != null)
        {
            ZIndex = node2D.ZIndex;
        }
    }
    public override void _Process(double delta) {
        base._Process(delta);
        Position += Get_Rotation_Vector(Rotation / 3.14f * 180) * 80 * Speed * (float)delta;
    }
    public void Reset()
    {
        Object_Join += Damage_Node;
        area.BodyEntered += Join;
        area.BodyExited += Exit;
    }
    public void Damage_Node(Node2D node)
    {
        if (node is Level.Object.LevelObject)
        {
            bool Cheak = Game.Cheak.CheakGroup.Cheak_Object_Group(node,detection_Group,Exclude_Group);
            if (!Cheak){return;}
            ((Level.Object.LevelObject)node).Reduce_Health(Damage,this);
            QueueFree();
        }
    }
}
