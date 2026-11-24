using Godot;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection;
using PVZEngine;
using PVZEngine.Object.Module;

public partial class Text : Node2D
{
    public override void _Ready() {
        base._Ready();
        PVZEngine.Data.Summand_Data data = new();
        string JsonText = JsonSerializer.Serialize(data);
        GD.Print(JsonText);
    }
}

