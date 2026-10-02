using Game;
using Godot;
using My_Csharp_Node;
namespace MVZ2.Object.Equipment;
public partial class Equipment : Level.Object.LevelObject
{
    [ExportGroup("Object")]
    [Export] public Touch.TouchPad TouchPad = null;
    [ExportGroup("Number")]
    [Export] public int Kill_Number = 50;
    /// <summary>
    /// 颜色
    /// </summary>
    [Export] public int Particie_Number = 2;
    /// <summary>
    /// 颜色
    /// </summary>
    [Export] public Godot.Collections.Array<Color> Colors = new();
    /// <summary>
    /// 大招状态
    /// </summary>
    [Export] public bool Ultimate_Move = false;
    /// <summary>
    /// 死亡自动销毁
    /// </summary>
    [Export] public bool Kill_Auto_Free = true;
    /// <summary>
    /// 放置音效组
    /// </summary>
    [Export] public Godot.Collections.Array<AudioStream> PlacedAudios = new(){};
    /// <summary>
    /// 死亡音效组
    /// </summary>
    [Export] public Godot.Collections.Array<AudioStream> KillAudios = new(){};
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        if (Level_Script.Use_Prop == Level_Script.Prop.Not){Modulate = new Color(1,1,1,1);return;}
        if (Is_Selected_This())
        {
            Modulate = new Color(2,2,2,1);
        }
        else
        {
            Modulate = new Color(1,1,1,1);
        }
    }
    public override void _ExitTree() {
        base._ExitTree();
        if (Level_Script.Selected_Object == this)
        {
            Level_Script.Selected_Object = null;
        }
    }
    public override void _Ready() {
        base._Ready();
        if (!Enable){return;}
        if (PlacedAudios.Count > 0)
        {
            Audio_Plus audio = new();
            audio.Audio_Type = Audio_Plus.Audio.Souds;
            audio.Auto_Get_File = false;
            audio.Auto_QueneFree = true;
            audio.Autoplay = true;
            audio.Stream = PlacedAudios.PickRandom();
            GetTree().CurrentScene.AddChild(audio);
            audio.Play();
            
        }
        Health_Reduce += This_Damage;
        Kill += This_Kill;
        if (TouchPad != null){
            TouchPad.Focus_Joinvoid += Selected_This;
            TouchPad.Focus_Exitvoid += Selected_Null;
            TouchPad.Button_Pressedvoid += Pressed;
        }
    }
    public void Selected_Null()
    {
        Level_Script.Selected_Object = null;
    }
    public void Selected_This()
    {
        Level_Script.Selected_Object = this;
    }
    public void This_Damage(Node Damage_Object)
    {
        for (int i = 0;i < Particie_Number;i++){
            float x = -Game.Get.Random.NextFloat_32(24,0);
            float y = -Game.Get.Random.NextFloat_32(21,0);
            float x2 = Game.Get.Random.NextFloat_32(24,0);
            float y2 = Game.Get.Random.NextFloat_32(22,0);
            Vector2 Summand_Position = new(x + x2,y + y2);
            PackedScene packed = Game.ResourceScene.LoadScene("res://Object/Effect/Particle/Default.tscn");
            MVZ2.Object.Particie Summand_Particie = packed.Instantiate<MVZ2.Object.Particie>();
            Summand_Particie.Enable = false;
            Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("particle").AddChild(Summand_Particie);
            Summand_Particie.Position = Position + Summand_Position;
            Summand_Particie.Random_Color = false;
            Summand_Particie.Modulate = Colors.PickRandom();
            Summand_Particie.EmitSignal(MVZ2.Object.Particie.SignalName.Initialization);
        }
    }
    public void Pressed()
    {
        if (!Enable){return;}
        if (Level_Script.Use_Prop == Level_Script.Prop.iron_pickaxe)
        {
            if (Is_Selected_This() == true)
            {
                Level_Script.Use_Prop = Level_Script.Prop.Not;
                This_Kill();
            }
        }
    }
    public void This_Kill()
    {
        if (!Kill_Auto_Free){return;}
        if (!Enable){return;}
        if (KillAudios.Count > 0)
        {
            Audio_Plus audio = new();
            audio.Audio_Type = Audio_Plus.Audio.Souds;
            audio.Auto_Get_File = false;
            audio.Auto_QueneFree = true;
            audio.Autoplay = true;
            audio.Stream = KillAudios.PickRandom();
            GetTree().CurrentScene.AddChild(audio);
            audio.Play();
        }
        if (level != null){
            level.EmitSignal("Object_Kill",this);
        }
        Modulate = new Color(0,0,0,0);
        for (int i = 0;i < Kill_Number;i++){
            float x = -Game.Get.Random.NextFloat_32(24,0);
            float y = -Game.Get.Random.NextFloat_32(21,0);
            float x2 = Game.Get.Random.NextFloat_32(24,0);
            float y2 = Game.Get.Random.NextFloat_32(22,0);
            Vector2 Summand_Position = new(x + x2,y + y2);
            PackedScene packed = Game.ResourceScene.LoadScene("res://Object/Effect/Particle/Default.tscn");
            MVZ2.Object.Particie Summand_Particie = packed.Instantiate<MVZ2.Object.Particie>();
            Summand_Particie.Enable = false;
            Game.Get_GlobalNode.Node_Data.Get_Node<Node2D>("particle").AddChild(Summand_Particie);
            Summand_Particie.Position = Position + Summand_Position;
            Summand_Particie.Random_Color = false;
            Summand_Particie.Modulate = Colors.PickRandom();
            Summand_Particie.EmitSignal(MVZ2.Object.Particie.SignalName.Initialization);
        }
        QueueFree();
    }
    public bool Is_Selected_This()
    {
        if (Level_Script.Selected_Object == this)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
