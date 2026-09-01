using Godot;
using Level;
using My_Csharp_Node;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace Level.Object.Monster;
/// <summary>
/// 普通僵尸
/// </summary>
public partial class Zombies : Level.Object.LevelObject
{
    [Export] internal Node2D ShaderNode = null;
    /// <summary>
    /// 死亡音效
    /// </summary>
    [Export] My_Csharp_Node.Audio_Plus Kill_Souds = null;
    /// <summary>
    /// 攻击音效
    /// </summary>
    [Export] Temp_Object.Damage Damage_Souds = null;
    /// <summary>
    /// 攻击渐变
    /// </summary>
    [Export] Tween Damage_Tween = null;
    /// <summary>
    /// 当前手部动画
    /// </summary>
    [ExportGroup("Animation")][Export] public String Current_Hand_Animation = "";
    /// <summary>
    /// 当前腿部动画
    /// </summary>
    [Export] public String Current_Leg_Animation = "";
    /// <summary>
    /// 腿部动画数组
    /// </summary>
    [Export] public Godot.Collections.Array<String> Leg_AnimationList = [];
    /// <summary>
    /// 手部动画数组
    /// </summary>
    [Export] public Godot.Collections.Array<String> Hand_AnimationList = [];
    /// <summary>
    /// 腿部动画管理
    /// </summary>
    [Export] public AnimationPlayer Leg_Play;
    /// <summary>
    /// 手部动画管理器
    /// </summary>
    [Export] public AnimationPlayer Hand_Play;
    /// <summary>
    /// 腿是否正在移动中
    /// </summary>
    [ExportGroup("Bool")][Export] public bool Lag_Move_ing = false;
    /// <summary>
    /// 攻击中
    /// </summary>
    [Export] public bool attack_ing = false;
    /// <summary>
    /// 攻击
    /// </summary>
    [Export] public bool attack = false;
    [Export]public Color Temp_Color = new Color(1,0,0,0);
    bool This_Initialization = false;
    public override void _Ready() {
        base._Ready();
        ShaderMaterial shader = new ShaderMaterial();
        shader.Shader = Game.ResourceShader.LoadShader("uid://cph5hxe55k3bo");
        ShaderNode.Material = shader;
        if (!Enable){return;}
        Health_Reduce += Object_damage;
        var @re = Reset_Area();
        Area.BodyEntered += Add_Object;
        Area.BodyExited += ObjectExit;
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
    async void Object_damage(Node Damage_Object)
    {
        if (!Enable){return;}
        if (This_Initialization == false)
        {
            if (level != null)
            {
                level.Object_Kill += delay_Free;
            }
        }
        if (HP > 0){
            Damage_Souds.change_Souds();
            Damage_Souds.Play();
            if (Damage_Tween != null)
            {
                Damage_Tween.Kill();
            }
            Damage_Tween = CreateTween();
            Damage_Tween.TweenProperty(this,new NodePath(Level.Object.Monster.Zombies.PropertyName.Temp_Color),new Color(0.75f,-0.5f,-0.5f,1),0.1);
            Tween Temp_Tween = Damage_Tween;
            await ToSignal(Temp_Tween,Tween.SignalName.Finished);
            if (Temp_Tween == Damage_Tween)
            {
                if (Damage_Tween != null)
                {
                Damage_Tween.Kill();
                }
                Damage_Tween = CreateTween();
                Damage_Tween.TweenProperty(this,new NodePath(Level.Object.Monster.Zombies.PropertyName.Temp_Color),new Color(0,0,0,1),0.3);
            }
        }
        else
        {
            if (Damage_Tween != null)
            {
                Damage_Tween.Kill();
            }
            Damage_Tween = CreateTween();
            Damage_Tween.TweenProperty(this,new NodePath(Level.Object.Monster.Zombies.PropertyName.Temp_Color ),new Color(0.75f,-0.5f,-0.5f,1),0.1);
            Kill_Souds.Play();
            RemoveFromGroup("Monster");
            Enable_Health = false;
            auto_Move = false;
            kill =true;
            detection_Group.Clear();
            CreateTween().TweenProperty(this,new NodePath(Node2D.PropertyName.Rotation),90 * 3.14 / 180,1).SetTrans(Game.Get.TweenType.GetTweenType(Game.Get.TweenType.Twee.正弦));    
            if (level != null)
            {
                level.EmitSignal(Level_Master_Script.SignalName.Object_Kill,this);
            }
        }
        

    }
    public override async void _Process(double delta) {
        base._Process(delta);
        if (!Enable){return;}
        ((ShaderMaterial)ShaderNode.Material).SetShaderParameter("color_EX",Temp_Color);
        if (auto_Move){
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
        }
        if (Leg_Play == null || Hand_Play == null){return;}
        if (!kill){
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
        else
        {
            if (Move_Ing == true)
            {
                Move_Ing = false;
                MoveType = Move_Type.Script_Driver;
                if (Leg_Play != null){
                    Leg_Play.Play("Lag_End");
                }
                if (Hand_Play != null){
                    Hand_Play.Stop();
                }
                await ToSignal(Leg_Play,AnimationPlayer.SignalName.AnimationFinished);
                Leg_Play.QueueFree();
                Leg_Play = null;
                Hand_Play.QueueFree();
                Hand_Play = null;
            }
        }
    }
    public async void delay_Free(Node2D node)
    {
        if (node != this){return;}
        Timer timer = new();
        timer.WaitTime = 1;
        AddChild(timer);
        timer.Start();
        await ToSignal(timer,Godot.Timer.SignalName.Timeout);
        QueueFree();
    }
    public void ObjectExit(Node2D Node)
    {
        Remove_Object(Node);
        Remove_Null_Object();
    }
}
