using PVZEngine.Data;
using Godot;
using Game;
namespace PVZEngine.AutoLoad;
/// <summary>
/// 初始化静态类
/// </summary>
public partial class Initialization : Node
{
    public override void _Ready() {
        base._Ready();
        PVZEngine.Static.WindowTool.Process_Window = GetWindow();
        Touch.Touch_Index.Set_Index_Enable(3,false);
        Game.Get_GlobalNode.Tree = GetTree();
        Game.Get_GlobalNode.InputPosition = GetTree().Root.GetNode<InputPosition>("InputPosition");
        QueueFree();
    }
}
