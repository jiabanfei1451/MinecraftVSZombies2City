using Godot;
using System;
using System.Threading.Tasks;

namespace My_Csharp_Node;
/// <summary>
/// 值类型的进度条
/// </summary>
public partial class ValueBar : Control
{
    [Export] public float MAX_Value = 100;
    [Export] public float MIN_Value = 0;
    [Export] public float Current_Value = 0;
    [Export] public Godot.Control Value_Object = null;
    [Export] public Godot.Control Value_Object_Hollow = null;
    public Godot.Vector2 Temp_Scale = Vector2.Zero; 
    public override async void _Ready() {
        base._Ready();
        while(true){
            if(Value_Object == null){return;}
            if(Value_Object_Hollow == null){return;}
            if(Temp_Scale == Vector2.Zero)
            {
                Temp_Scale = Value_Object.Scale;
            }
            Value_Object.Scale = new Vector2(((Current_Value - MIN_Value) / (MAX_Value - MIN_Value)) * Temp_Scale.X,Temp_Scale.Y);
            await Task.Delay(1000 / 60);
        }
    }
}
