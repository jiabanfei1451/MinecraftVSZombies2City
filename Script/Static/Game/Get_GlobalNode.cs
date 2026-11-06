
using Godot;
using AutoLoad;
using PVZEngine.AutoLoad;
using PVZEngine.Module;
namespace Game{
/// <summary>
/// 获取全局节点
/// </summary>
static class Get_GlobalNode
{
	public static PVZEngine.AutoLoad.TimeScale TimeScale = null;
	public static Game.InputPosition InputPosition = null;
	public static SceneTree Tree = null;
	/// <summary>
	/// 输入框
	/// </summary>
	public static Command_Edit CommandEdit = null;
	public static Key_Script GetKey = null;
}
}
