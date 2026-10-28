using System.Threading.Tasks;
using Godot;
using MVZ2.Object.Interactive_Objects;
using PVZEngine;
using PVZEngine.Module;
using PVZEngine.Static;
namespace MVZ2.Object.Equipment;
public partial class Furnace : PVZEngine.Object.Module.Equipment
{
    [Export] internal Timer @Timer = null;
    [Export] internal AnimationPlayer animationPlayer = null;
    [Export] internal Audio_Plus Ultimate_move = null;
    [Export] internal Audio_Plus RedStone_Output = null;
    public string[] Animations = new string[]{};
    public override void _Ready() {
        base._Ready();
        if (!Enable){return;}
        if (animationPlayer != null){
            Animations = animationPlayer.GetAnimationList();
        }
        Activate_Equipment += Ultimate_moveEvene;
        if (@Timer != null)
        {
            @Timer.Start();
            @Timer.Timeout += Timeout;
        }
    }
    public async void Ultimate_moveEvene()
    {
        Activate = true;
        for (byte b = 0;b < 6; b++)
        {
            if (!(Ultimate_move is null))
            {
                Ultimate_move.Play();
            }
            Summand_RedStone();
            await ToSignal(GetTree().CreateTimer(0.3f),SceneTreeTimer.SignalName.Timeout);
        }
        Activate = false;
    }
    public void Summand_RedStone()
    {
        PackedScene scene = ResourceScene.LoadScene("res://Object/Interactive Objects/Red_Stone.tscn");
        MVZ2.Object.Interactive_Objects.RedStone redStone = scene.Instantiate<RedStone>();
        NodeData.Get_Node<Node2D>("Item").AddChild(redStone);
        redStone.GlobalPosition = GlobalPosition;
        redStone.Reset_Position = Position;
        redStone.EmitSignal("Initialization");
    }
    public async void Timeout()
    {
        if (!Enable){return;}
        animationPlayer.Play(Animations[2]);
        await ToSignal(animationPlayer,AnimationPlayer.SignalName.AnimationFinished);
        if (!(RedStone_Output is null))
        {
            RedStone_Output.Play();
        }
        Summand_RedStone();
        animationPlayer.Play(Animations[1]);
        @Timer.WaitTime = Random.NextFloat_32(15,12);
        @Timer.Start();
    }
}
