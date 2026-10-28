using PVZEngine.Object.Module;
using Godot;
using PVZEngine.Static;
namespace Level.Object.Bullet;
/// <summary>
/// 箭矢
/// </summary>
public partial class Arrow : PVZEngine.Object.Module.BulletData
{
    [Export] Area2D area = null;
    bool c = false;
    public override void _Ready() {
        base._Ready();
    }
    public override void _Process(double delta) {
        base._Process(delta);
        practical_Position += Get_Rotation_Vector(Rotation / 3.14f * 180) * 80 * Speed * (float)delta;
        SetPhysics_Position(delta);
        Get_Level();
        ReSet_Index();
    }
    public void Reset()
    {
        Reset_Position();
        Object_Join += Damage_Node;
        area.BodyEntered += Join;
        area.BodyExited += Exit;
        Node2D node2D = NodeData.Get_Node<Node2D>("Shoot");
        if (node2D != null)
        {
            ZIndex = node2D.ZIndex;
        }
    }
    public void Damage_Node(Node2D node)
    {
        if (node == this){return;}
        if (node is LevelObject)
        {
            bool check = Game.Cheak.CheakGroup.Cheak_Object_Group(node,detection_Group,Exclude_Group);
            if (!check){return;}
                if (c == false){
                c = true;
                Check_Object[0].Reduce_Health(Damage,this);
                QueueFree();
            }
        }
    }
}
