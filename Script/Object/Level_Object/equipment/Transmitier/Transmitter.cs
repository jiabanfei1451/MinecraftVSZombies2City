using Godot;
using DEBUG;

public partial class Transmitter : Level.Object.Data
{
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
    public void Object_join (Node2D Node)
    {
        bool Check = Check_Object_Group(Node);
        if (Check == true)
        {
            Add_Object(Node);
        }
    }
    public void Object_Exit (Node2D Node)
    {
        Remove_Object(Node);
        Remove_Null_Object();
    }
}
