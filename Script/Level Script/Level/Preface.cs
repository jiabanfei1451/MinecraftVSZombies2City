using System;
using PVZEngine;
using Godot;
using PVZEngine.Module;
using PVZEngine.Static;
using PVZEngine.Enum;
namespace Level;
/// <summary>
/// 序章
/// </summary>
public partial class Preface : Level_Master_Script
{
    public override async void _Ready() {
        base._Ready();
        Game.WindowTool.Set_Title(10, Tween.TransitionType.Circ,"MVZ2_City");
        choose_Card();
        Object_List list = Game.Get_GlobalNode.object_List; 
        Summand.Data = new();
        await ToSignal(GetTree().CreateTimer(1),SceneTreeTimer.SignalName.Timeout);
        Summand.Data.Add_Wave(new()
        {
            Add_ID = new(){Object_List.Get_ID("0", IndexMode.index)},
            Number = 1
        });
        Summand.Data.Add_Wave(new()
        {
            Add_ID = new(){Object_List.Get_ID("0", IndexMode.index)},
            Number = 1
        });
        Summand.Data.Add_Wave(new()
        {
            Add_ID = new(){Object_List.Get_ID("0", IndexMode.index)},
            Number = 1
        });
        Summand.Data.Add_Wave(new()
        {
            Add_ID = new(){Object_List.Get_ID("0", IndexMode.index)},
            Number = 1
        });
        Summand.Data.Add_Wave(new()
        {
            Add_ID = new(){Object_List.Get_ID("0", IndexMode.index)},
            Number = 1
        });
        Summand.Data.random.Seed = 0;
        await ToSignal(GetTree().CreateTimer(1),SceneTreeTimer.SignalName.Timeout);
        GD.Print(Summand.Data.Summand_IDs.Count);
        Summand.Start_Summand();
        await ToSignal(GetTree().CreateTimer(1),SceneTreeTimer.SignalName.Timeout);
        Summand.Data.Remove_Null_Object();
    }
}
