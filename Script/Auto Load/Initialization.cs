using PVZEngine.Data;
using Godot;
namespace PVZEngine.AutoLoad;
/// <summary>
/// 初始化静态类
/// </summary>
public partial class Initialization : Node
{
    public override void _Ready() {
        base._Ready();
        PVZEngine.Static.WindowTool.Process_Window = GetWindow();
        PlayerData.Add_Data("Version","0.1.0");
        DEBUG.Info.Print(PlayerData.Player_Data);
        Touch.Touch_Index.Set_Index_Enable(3,false);
        Game.Get_GlobalNode.Tree = GetTree();
        QueueFree();
    }
}
