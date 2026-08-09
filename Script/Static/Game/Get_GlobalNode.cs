
using Godot;
using System;

namespace Game{
/// <summary>
/// 获取全局节点
/// </summary>
static class Get_GlobalNode
{
	/// <summary>
	/// 当前关卡可读取的节点数据
	/// </summary>
	static Godot.Collections.Array<Godot.Collections.Array> NodeData = new Godot.Collections.Array<Godot.Collections.Array>()
	{
		// Node
		new Godot.Collections.Array(){},
		// Name
		new Godot.Collections.Array(){},
	};
	/// <summary>
	/// 用于添加删除节点索引的类
	/// </summary>
	public static class Node_Data
	{
		/// <summary>
		/// 搜索索引模式
		/// </summary>
		public enum Mode_Type
		{
			/// <summary>
			/// 按ID搜索(注意! 该模式下的索引是从0开始的,且不会随着节点的删除而改变,所以请谨慎使用)
			/// </summary>
			Index = 0,
			/// <summary>
			/// 按名称搜索(推荐使用,该模式下的索引会随着节点的删除而改变)
			/// </summary>
			Name = 1,	
		}
		/// <summary>
		/// 获取节点
		/// 该方法会返回一个节点,如果节点不存在则返回null
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="Index">索引</param>
		/// <param name="Get_Mode">搜索模式</param>
		/// <returns></returns>
		public static T Get_Node<T>(String Index = "null",Mode_Type Get_Mode = Mode_Type.Name) where T : class
		{
			Node Get;
			int index = -1;
			switch (Get_Mode){
				case Mode_Type.Name:
					index = NodeData[1].IndexOf(Index);
					DEBUG.Info.Print(index);
					break;
				case Mode_Type.Index:
					index = int.Parse(Index);
					break;
			}
			DEBUG.Info.Print(NodeData[0].Count);
			if (index < NodeData[0].Count)
			{
				Get = (Node)NodeData[0][index];
			}else
			{
				Get = null;
			}
			return Get as T;
		}
		/// <summary>
		/// 添加节点索引
		/// </summary>
		/// <param name="Node">索引</param>
		/// <param name="Name">名称</param>
		public static void Add_Node(Godot.Node Node,String Name)
		{
			NodeData[0].Add(Node);
			NodeData[1].Add(Name);
			DEBUG.Info.Print(NodeData);
		}
		/// <summary>
		/// 清空已有节点索引
		/// 警告: 该方法会清空所有节点索引,请谨慎使用
		/// 这会导致所有节点索引失效,请在使用前确保你已经备份了所有节点索引
		/// </summary>
		public static void Clear_Node()
		{
			NodeData[0].Clear();
			NodeData[1].Clear();
		}
		/// <summary>
		/// 是Clear_Node()的安全版本,该方法仅会清空Mode_Type为Name的节点索引,不会清空Mode_Type为Index的节点索引
		/// </summary>
		/// <param name="Index">索引</param>
		/// <param name="Mode">模式</param>
		public static void Remove_Node(String Index,Mode_Type Mode)
		{
			int index = -1;
			switch (Mode)
			{
				case Mode_Type.Name:
					index = NodeData[1].IndexOf(Index);
					break;
				case Mode_Type.Index:
					index = int.Parse(Index);
					break;
			}
			if (index != -1 && (Node)NodeData[0][index] != null)
			{
				NodeData[0].RemoveAt(index);
				NodeData[1].RemoveAt(index);
			}
			else
			{
				DEBUG.Info.PrintRich("[color=yellow][b]此节点不存在,正在执行自动筛选[/b][/color]");
				Screening_Not_Null_Node();
			}
		}
		/// <summary>
		/// 筛选掉NodeData中为null的节点,并重新生成索引
		/// </summary>
		public static void Screening_Not_Null_Node()
		{
		Godot.Collections.Array Name_Array = new Godot.Collections.Array();
		Godot.Collections.Array Node_Array = new Godot.Collections.Array();
		for(int index = 0 ; index < NodeData[0].Count ; index++)
		{
			if ((Node)NodeData[0][index] != null)
			{
				Node_Array.Add(NodeData[0][index]);
				Name_Array.Add(NodeData[1][index]);
			}
		}
		NodeData[0] = Node_Array;
		NodeData[1] = Name_Array;
		}
	}
	/// <summary>
	/// 获取卡槽数据
	/// </summary>
	/// <param name="tree"></param>
	/// <returns></returns>
	public static Card_Data Get_Card_Data(SceneTree tree)
		{
			Card_Data Node = tree.Root.GetNode<Card_Data>("CardData");
			return Node;
		}
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
