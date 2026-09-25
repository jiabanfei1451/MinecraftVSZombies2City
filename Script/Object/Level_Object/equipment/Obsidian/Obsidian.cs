using Godot;

public partial class Obsidian : Level.Object.LevelObject
{
    /// <summary>
    /// 颜色
    /// </summary>
    [Export] public Godot.Collections.Array<Color> Colors = new();
    /// <summary>
    /// 大招状态
    /// </summary>
    [Export] public bool Ultimate_Move = false;
    public override void _Ready() {
        base._Ready();
        Health_Reduce += This_Damage;
        Kill += This_Kill;
    }
    public void This_Damage(Node Damage_Object)
    {
        for (int i = 0;i < 2;i++){
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
    public void This_Kill()
    {
        level.EmitSignal("Object_Kill",this);
        Modulate = new Color(0,0,0,0);
        for (int i = 0;i < 50;i++){
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
}
