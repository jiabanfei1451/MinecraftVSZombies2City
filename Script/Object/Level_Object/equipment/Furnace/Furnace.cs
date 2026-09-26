using Godot;
using MVZ2.Object.Interactive_Objects;
using System;
namespace MVZ2.Object.Equipment;
public partial class Furnace : MVZ2.Object.Equipment.Equipment
{
    [Export] internal Timer @Timer = null;
    [Export] internal AnimationPlayer animationPlayer = null;
    public String[] Animations = new string[]{};
    public override void _Ready() {
        base._Ready();
        if (!Enable){return;}
        if (animationPlayer != null){
            Animations = animationPlayer.GetAnimationList();
        }
        GD.Print(Animations);
        if (@Timer != null)
        {
            @Timer.Start();
            @Timer.Timeout += Timeout;
        }
    }
    public void Summand_RedStone()
    {
        PackedScene scene = Game.ResourceScene.LoadScene("res://Object/Interactive Objects/Red_Stone.tscn");
        MVZ2.Object.Interactive_Objects.RedStone redStone = scene.Instantiate<RedStone>();
        Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("item").AddChild(redStone);
        redStone.GlobalPosition = GlobalPosition;
        redStone.Reset_Position = Position;
        redStone.EmitSignal("Initialization");
    }
    public async void Timeout()
    {
        if (!Enable){return;}
        animationPlayer.Play(Animations[2]);
        await ToSignal(animationPlayer,AnimationPlayer.SignalName.AnimationFinished);
        Summand_RedStone();
        animationPlayer.Play(Animations[1]);
        @Timer.WaitTime = Game.Get.Random.NextFloat_32(15,12);
        @Timer.Start();
    }
}
