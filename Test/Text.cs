using Godot;
using System;
using System.Text.Json;

public partial class Text : Node2D
{
    public class Tsest
    {
        public int sds = 1;
        public string dasd = "1";
    }
    public override void _Ready() {
        base._Ready();
        Tsest sd = new Tsest
        {
          sds = 1,
          dasd = "1"  
        };
        string jsstr = JsonSerializer.Serialize(sd);
        GD.Print(jsstr);
    }
}
