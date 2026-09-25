using Game.Static;
using Godot;
using System;
using System.Dynamic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
namespace Game.AutoLoad;
/// <summary>
/// 初始化静态类
/// </summary>
public partial class Initialization : Node
{
    public override async void _Ready() {
        base._Ready();
        Game.WindowTool.Process_Window = GetWindow();
        PlayerData.Add_Data("Version","0.1.0");
        DEBUG.Info.Print(PlayerData.Player_Data);
        DEBUG.Info.Print("我是傻逼");
        DEBUG.Info.Print("OK");
        QueueFree();
    }
}
