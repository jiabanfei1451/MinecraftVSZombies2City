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
    [Export] public PackedScene Shoot = Game.ResourceScene.LoadScene("uid://caymc0p7rsog");
    /// <summary>
    /// 箭矢生成坐标
    /// </summary>
    [Export] public Marker2D Summand_shoot_Position = null;
    /// <summary>
    /// 生成高度
    /// </summary>
    [Export] public float Summand_Height = 0;
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
    [Export] public bool This_Initialization = false;
    public override void _Ready() {
        base._Ready();
        if (!Enable){return;}
        this.Timer = GetNode<Timer>("Timer");
        Shoot_Sound = GetNode<AudioStreamPlayer>("Souds");
        this.AnimationPlayer = GetNode<AnimationPlayer>("Animation");
        if (Area == null){return;}
        Area.BodyEntered += Add_Object;
        Area.BodyExited += ObjectExit;
        var @r = Reset_Area();
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        if (Timer == null || Shoot_Sound == null){return;}
        if (level != null)
        {
            if (This_Initialization == false)
            {
                level.Object_Kill += ObjectExit;
            }
        }
        if (this.Timer.TimeLeft == 0 && Current_detection_object.Count > 0)
        {
            Summand_Shoot();
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
    /// <summary>
    /// 重启检测器
    /// </summary>
    public void ReEnable_Area()
    {
        Area.Monitoring = false;
        Area.Monitoring = true;

    } 
    /// <summary>
    /// 生成射弹
    /// </summary>
    public void Summand_Shoot()
    {
        PackedScene Shoot_Scene = Game.ResourceScene.LoadScene("uid://caymc0p7rsog");
        Node2D shoot = Shoot_Scene.Instantiate<Node2D>();
        Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("Shoot").AddChild(shoot);
        shoot.GlobalPosition = Summand_shoot_Position.GlobalPosition;
    }
    /// <summary>
    /// 剔除不存在或死亡的物体
    /// </summary>
    /// <param name="Node"></param>
    public void ObjectExit (Node2D Node)
    {
        if (Node is Level.Object.LevelObject){
            if (Current_detection_object.IndexOf((Level.Object.LevelObject)Node) != -1){
                Remove_Object(Node);
                Remove_Null_Object();
            }
        }
    }
}
