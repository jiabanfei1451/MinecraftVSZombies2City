using Godot;
using Level.Object;
using My_Csharp_Node;
using System;
using System.Threading.Tasks;

namespace MVZ2.Object.Equipment;
public partial class Landmine : MVZ2.Object.Equipment.Equipment
{
    [Export] public Godot.Collections.Array<Sprite2D> sprite2Ds = new();
    [Export] public int Sprite2D_ID = new();
    [Export] public AnimationPlayer Anima = null;
    [Export] public Timer Timer = null;
    [Export] public bool Boom = false;
    [Export] public Audio_Plus Unearthed_Souds = null;
    public override async void _Ready() {
        base._Ready();
        if (!Enable){return;}
        Anima.Play("TNT_Landmine/Start");
        Area.BodyEntered += Add_Object;
        Area.BodyExited += Remove_Object;
        await ToSignal(Timer,Timer.SignalName.Timeout);
        Unearthed_Souds.Play();
        Anima.Play("TNT_Landmine/Landmine_TNT");
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Enable){return;}
        int d = 0;
        foreach (Sprite2D sprite2D in sprite2Ds)
        {
            if (d != Sprite2D_ID)
            {
                sprite2D.Visible = false;
            }
            else
            {
                sprite2D.Visible = true;
            }
            d += 1;
        }
        if (Boom == false){return;}
        if (Current_detection_object.Count > 0)
        {
            Game.Static.Camera.Camera_Vibration(new Vector2(40,40),60,0.02f);
            foreach(LevelObject @object in Current_detection_object)
            {
                if (@object != null)
                {
                    @object.Reduce_Health(Damage,this);
                }
            }
            level.EmitSignal(Level.Level_Master_Script.SignalName.Object_Kill,this);
            QueueFree();
            return;
        }
    }
}
