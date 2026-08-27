using Godot;
using DEBUG;
using System.Threading.Tasks;
namespace Level.Object.Equipment;
/// <summary>
/// 发射器
/// </summary>
public partial class Transmitter : Level.Object.LevelObject
{
    /// <summary>
    /// 射弹场景
    /// </summary>
    [ExportGroup("Summand_Node")]
    [Export] public PackedScene Shoot = Game.ResourceTool.LoadScene("uid://caymc0p7rsog");
    /// <summary>
    /// 箭矢生成坐标
    /// </summary>
    [Export] public Marker2D Summand_shoot_Position = null;
    /// <summary>
    /// 计时器节点
    /// </summary>
    [ExportGroup("Node")]
    [Export] public Godot.Timer Timer = null;
    /// <summary>
    /// 动画处理节点
    /// </summary>
    [Export] public Godot.AnimationPlayer AnimationPlayer = null;
    /// <summary>
    /// 发射音效节点
    /// </summary>
    [Export] public Godot.AudioStreamPlayer Shoot_Sound = null;
    public override void _Ready() {
        base._Ready();
        if (!Enable){return;}
        this.Timer = GetNode<Timer>("Timer");
        Shoot_Sound = GetNode<AudioStreamPlayer>("Souds");
        this.AnimationPlayer = GetNode<AnimationPlayer>("Animation");
        if (Area == null){return;}
        Area.BodyEntered += Object_join;
        Area.BodyExited += Object_Exit;
        var @r = Reset_Area();
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        if (Timer == null || Shoot_Sound == null){return;}
        if (this.Timer.TimeLeft == 0 && Current_detection_object.Count > 0)
        {
            this.AnimationPlayer.Play("Shoot");
            Shoot_Sound.Play();
            double Time = Game.Get.Random.NextFloat_64(1.4,1.6);
            this.Timer.Start(Time);
        }
    }
    public void Check_Change_line(Level.Object.LevelObject LevelObject)
    {
        if (!Game.Cheak.CheakGroup.Cheak_Object_Group(LevelObject,detection_Group,Exclude_Group)){return;}
        ReEnable_Area();
    }
    public void ReEnable_Area()
    {
        Area.Monitoring = false;
        Area.Monitoring = true;

    } 
    public void Object_join (Node2D Node)
    {
        if (Node is Level.Object.LevelObject){
            bool Check = Game.Cheak.CheakGroup.Cheak_Object_Group(Node,detection_Group,Exclude_Group);
            if (Check == true && ((Level.Object.LevelObject)Node).Lawn_Index == this.Lawn_Index)
            {
                Add_Object(Node);
            }
        }
    }
    public void Object_Exit (Node2D Node)
    {
        Remove_Object(Node);
        Remove_Null_Object();
    }
}
