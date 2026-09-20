using Godot;
using System;
using System.Collections.Generic;
namespace AutoLoad;
public partial class Key_Script : Node
{
    /// <summary>
    /// 按下时获取键
    /// </summary>
    /// <param name="key"></param>
    [Signal] public delegate void Key_DownEventHandler(InputEventKey key);
    /// <summary>
    /// 获取按下时的键值
    /// </summary>
    /// <param name="key"></param>
    [Signal] public delegate void Key_DownKeyCodeEventHandler(Key key);
    /// <summary>
    /// 抬起时获取键
    /// </summary>
    /// <param name="key"></param>
    [Signal] public delegate void Key_UPEventHandler(InputEventKey key);
    /// <summary>
    /// 获取抬起时的键值
    /// </summary>
    /// <param name="key"></param>
    [Signal] public delegate void Key_UPKeyCodeEventHandler(Key key);
    public List<Key> Down_Key = new(){};
    /// <summary>
    /// Alt是否被按下
    /// </summary>
    public bool Alt_Pressed = false;
    /// <summary>
    /// Ctrl是否被按下
    /// </summary>
    public bool Ctrl_Pressed = false;
    /// <summary>
    /// Shift是否被按下
    /// </summary>
    public bool Shift_Pressed = false;
    public override void _Ready() {
        base._Ready();
        Game.Get_GlobalNode.GetKey = this;
    }
    /// <summary>
    /// 获取按键事件
    /// </summary>
    /// <param name="event"></param>
    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (@event is InputEventKey)
        {
            Alt_Pressed = ((InputEventKey)@event).AltPressed;
            Ctrl_Pressed = ((InputEventKey)@event).CtrlPressed;
            Shift_Pressed = ((InputEventKey)@event).ShiftPressed;

            if (((InputEventKey)@event).IsPressed())
            {
                if (Down_Key.IndexOf(((InputEventKey)@event).Keycode) == -1){
                    Down_Key.Add(((InputEventKey)@event).Keycode);
                    EmitSignalKey_Down((InputEventKey)@event);
                    EmitSignalKey_DownKeyCode(((InputEventKey)@event).Keycode);
                }

            }
            else
            {
                if (Down_Key.IndexOf(((InputEventKey)@event).Keycode) != -1){
                    Down_Key.Remove(((InputEventKey)@event).Keycode);
                    EmitSignalKey_UP((InputEventKey)@event);
                    EmitSignalKey_UPKeyCode(((InputEventKey)@event).Keycode);
                }
            }
        }
    }
}
