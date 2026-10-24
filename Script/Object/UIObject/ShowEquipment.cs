using Godot;
using PVZEngine.Static;

namespace UIObject;
public partial class ShowEquipment : TextureRect
{
    public override void _Process(double delta) {
        base._Process(delta);
        Label label = GetNode<Label>("Label");
        label.Text = Level_Script.Equipment_Capable.ToString();
    }
}
