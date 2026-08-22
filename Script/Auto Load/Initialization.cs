using Godot;
using System;
namespace Game.AutoLoad;
/// <summary>
/// 初始化静态类
/// </summary>
public partial class Initialization : Node
{
    public override void _Ready() {
        base._Ready();
        Game.WindowTool.Process_Window = GetWindow();
        QueueFree();
    }
}
