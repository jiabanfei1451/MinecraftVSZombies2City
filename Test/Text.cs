using Godot;
using PVZEngine.Enum;
using PVZEngine.Object.Module;
using PVZEngine.Data;
using PVZEngine.Type;
using System.Text.Json;
using System;
using System.Threading.Tasks;

public partial class Text : Node2D
{
    
    public override async void _Ready() {
        base._Ready();
        await Task.Delay(100);
        var d = PVZEngine.Static.FileSystem.Handle_JsonText(JsonSerializer.Serialize<PVZEngine.Data.PlayerData.Data>(PlayerData.Savedata));
        var w = JsonSerializer.Deserialize<PVZEngine.Data.PlayerData.Data>(d);
        PVZEngine.Object.Module.Equipment ds = new();
        GD.Print(ds.GetType().GetProperties()[0]);
    }
}

