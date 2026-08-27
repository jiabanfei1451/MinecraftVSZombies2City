using Godot;
using Level;
using System;
namespace Level.Object.Monster;
/// <summary>
/// 普通僵尸
/// </summary>
public partial class Zombies : Level.Object.LevelObject
{
    [ExportGroup("Animation")][Export] public String Current_Hand_Animation = "";
    [Export] public String Current_Leg_Animation = "";
    [Export] public Godot.Collections.Array<String> Leg_AnimationList = [];
    [Export] public Godot.Collections.Array<String> Hand_AnimationList = [];
    [Export] public AnimationPlayer Leg_Play;
    [Export] public AnimationPlayer Hand_Play;
    [ExportGroup("Bool")][Export] public bool Lag_Move_ing = false;
    [Export] public bool attack_ing = false;
    [Export] public bool attack = false;
    public override void _Ready() {
        base._Ready();
        var @re = Reset_Area();
        Area.BodyEntered += Object_join;
        Area.BodyExited += Object_Exit;
        Leg_Play = GetNode<AnimationPlayer>("Lag_Animation");
        Hand_Play = GetNode<AnimationPlayer>("Hand_Animation");
        Random random = new Random();
        Speed_Multiplication = Game.Get.Random.NextFloat_32(0.75f,1.5f);
        Leg_Play.SpeedScale = Speed_Multiplication;
        Hand_Play.SpeedScale = Speed_Multiplication;
        foreach (String s in Leg_Play.GetAnimationList())
        {
            Leg_AnimationList.Add(s);
        }
        foreach (String s in Hand_Play.GetAnimationList())
        {
            Hand_AnimationList.Add(s);
        }
    }
    public override void _Process(double delta) {
        base._Process(delta);
        if (Current_detection_object.Count > 0)
        {
            Move_Ing = false;
            attack_ing = true;
        }
        else
        {
            Move_Ing = true;
            attack_ing = false;
        }
        if (Leg_Play == null || Hand_Play == null){return;}
        if (Move_Ing == true && attack_ing == false){
            if (Lag_Move_ing == false)
            {
                Current_Leg_Animation = "Lag_Start";
            }
            else
            {
                Current_Leg_Animation = "Lag_Ing";
            }
        }
        else
        {
            if (Lag_Move_ing == true)
            {
                Current_Leg_Animation = "Lag_End";
            }
            else
            {
                Current_Leg_Animation = "RESET";
            }
        }
        if (attack_ing == false)
        {
            Current_Hand_Animation = Hand_AnimationList[2];
        }
        else
        {
            Current_Hand_Animation = Hand_AnimationList[0];
        }
        if (Hand_Play.IsPlaying() == false || Hand_Play.CurrentAnimation != Current_Hand_Animation && Hand_Play.HasAnimation(Current_Hand_Animation))
        {
            if (Current_Hand_Animation != "null"){
            Hand_Play.Play(Current_Hand_Animation);
            }
            else
            {
                Hand_Play.Stop();
            }
        }
        if (Leg_Play.IsPlaying() == false || Leg_Play.CurrentAnimation != Current_Leg_Animation && Leg_Play.HasAnimation(Current_Leg_Animation))
        {
            if (Current_Hand_Animation != "null"){
            Leg_Play.Play(Current_Leg_Animation);
            }
            else
            {
                Leg_Play.Stop();
            }
        }
    }
    public void Object_join(Node2D Node)
    {
        bool Check = Game.Cheak.CheakGroup.Cheak_Object_Group(Node,detection_Group,Exclude_Group);
        if (Check == true)
        {
            Add_Object(Node);
        }
    }
    public void Object_Exit(Node2D Node)
    {
        Remove_Object(Node);
        Remove_Null_Object();
    }
}
