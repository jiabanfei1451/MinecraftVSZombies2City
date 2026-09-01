using Godot;
using System;
using System.Threading.Tasks;

public partial class Monster_Kill_Particie : Sprite2D
{
    [Export] public Godot.Collections.Array<Rect2> region_arrays = new(){
    new(56,24,2,2)
    ,new(38,22,4,4),
    new(22,22,6,8),
    new(6,20,6,10),
    new Rect2(50,4,8,10),
    new Rect2(16,4,10,10),
    new Rect2(0,0,16,16),
    new Rect2(18,2,14,14)};
    [Export] public float Timeout = 0.3f;
    public override async void _Ready() {
        base._Ready();
        AtlasTexture atlas = new();
        Texture2D h78 = atlas;
        atlas.Atlas = GD.Load<Texture2D>("res://Image/Effect/particle/smoke.png");
        Random index_random = new();
        int get_Index = index_random.Next(2,region_arrays.Count - 1);
        atlas.Region = region_arrays[get_Index];
        Texture = h78;
        for(int i = get_Index;i > 0;i--)
        {
            if (i < 0){break;}
            atlas.Region = region_arrays[i];
            await ToSignal(GetTree().CreateTimer(Timeout),SceneTreeTimer.SignalName.Timeout);
        }
        QueueFree();
    }
}
