using Game;
using Game.AutoLoad;
using Godot;
using System;
using Touch;
namespace MVZ2.Object.Interactive_Objects;
public partial class RedStone : MVZ2.Object.Particie
{
    [Export] public TouchPad pad = null;
    [Export] public short add_Equipment_Capable = 25;
    [Export] public AudioStreamPlayer Souds = null;
    [Export] AnimationPlayer anima = null;
    public override void _Ready() {
        base._Ready();
        Initialization += st2;
    }
    public void st2()
    {
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode += Key_Down;
        Max_Rise_Strength = Game.Get.Random.NextFloat_32(5,15);
        Rise_Strength_Enhance_Speed = Game.Get.Random.NextFloat_32(3f,4);
        Multiplication = new Vector2(Game.Get.Random.NextFloat_32(5,6f),Game.Get.Random.NextFloat_32(2,2.5f));
        Max_bounce_Number = new Random().Next(1,3);
        Set_Position_Multiplication = Game.Get.Random.NextFloat_32(1.5f,2);
        MAX_PositionX_Offset = Game.Get.Random.NextFloat_32(15,45);
        MaxHeight = new Vector2(Game.Get.Random.NextFloat_32(10,30),Game.Get.Random.NextFloat_32(10,30));
        if (pad == null){return;}
        pad.Button_Pressedvoid += pressed;
        pad.Drag_Ingvoid += pressed;
        if (anima != null)
        {
            anima.AnimationFinished += finale;
        }
    }
    public void finale(StringName s)
    {
        QueueFree();
    }
    public void Key_Down(Key @Key)
    {
        if (@Key == Key.Space)
        {
            sp();
            
        }
    }
    public void pressed()
    {
        sp();
    }
    /// <summary>
    /// 拾取
    /// </summary>
    public void sp()
    {
        Game.Get_GlobalNode.GetKey.Key_DownKeyCode -= Key_Down;
        pad.Enable = false;
        if (Souds != null)
        {
            float s = (float)Level_Script.audio_Scale;
            if (s > 8)
            {
                s = 8;
            }
            Souds.PitchScale = 1 + (s / 4);
            Souds.Play();
        }
        Level_Script.audio_Scale += 1;
        if (anima != null)
        {
            anima.Play("pressed");
        }
        Level_Script.Equipment_Capable += add_Equipment_Capable;
    }
}
