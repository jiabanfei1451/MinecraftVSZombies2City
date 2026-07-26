
using Godot;
using System;

namespace Game{
/// <summary>
/// 获取全局节点
/// </summary>
static class Get_GlobalNode
{
	/// <summary>
	/// 获取音乐引擎
	/// </summary>
	/// <param name="tree"></param>
	/// <returns></returns>
	public static Muisc_Engine Get_Muisc_Engine(SceneTree tree)
	{
		Muisc_Engine muisc_Engine = tree.Root.GetNode<Muisc_Engine>("MuiscEngine");
		return muisc_Engine;
	}
	/// <summary>
	/// 获取音频列表
	/// </summary>
	/// <param name="tree"></param>
	/// <returns></returns>
	public static Audio_List Get_Audio_List(SceneTree tree){
		Audio_List audio_List = tree.Root.GetNode<Audio_List>("AudioList");
		return audio_List;	
	}
}
}
