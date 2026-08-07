using Godot;
using Game;
using System;
using System.Threading.Tasks;
namespace My_Csharp_Node
{
[GlobalClass]
public partial class Audio_Plus : AudioStreamPlayer
{
	[Export] public String Muisc_ID = "MVZ2:Null";
	[Export] AudioStream Current_Stream = null;
	[Export(PropertyHint.Range,"-24,24,0.01")] public float Add_Volume = 0;
	[Export(PropertyHint.Range,"0,1,0.01")] public float Multiplication = 1;
	public float Current_Multiplication = 1;
	public enum Audio
	{
		Muisc = 0,
		Souds = 1
	}
	[Export] public Audio Audio_Type = Audio.Muisc;
	[Export] public Audio_List AudioList_Object;
	[Export] public float fade_Time = 5;
	public override void _Ready() {
		base._Ready();
		AudioList_Object = GetTree().Root.GetNode<Audio_List>("AudioList");
		AudioStream stream = Get_Muisc();
		Fade_Join();
		if (Current_Stream != stream)
		{
			Current_Stream = stream;
			Stream = stream;
		}
	}
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		float Type_volume = 0;
		if (Audio_Type == Audio.Muisc){
			Type_volume = AudioList_Object.Muisc_Volume;

			AudioStream stream = Get_Muisc();
			if (Current_Stream != stream)
			{
				Current_Stream = stream;
				Stream = stream;
			}
			if (Autoplay == true)
				{
					if (!Playing && stream != null)
					{
						Play();
					}
				}
		}
		else
		{
			Type_volume = AudioList_Object.Souds_Volume;
		}
		if (Type_volume / 100 * Multiplication * Current_Multiplication != 0){
		VolumeDb = -40 + (40 + Add_Volume) * (Type_volume / 100 * Multiplication * Current_Multiplication);
		}else
		{
			VolumeDb = -99;
		}
	}
	public void Fade_Join()
	{
		Current_Multiplication = 0;
		Tween t = CreateTween();
		t.TweenProperty(this,"Current_Multiplication",1,fade_Time);
	}
	public async void Fade_Exit()
	{
		Tween t = CreateTween();
		t.TweenProperty(this,"Current_Multiplication",0,fade_Time);
		await ToSignal(t,Tween.SignalName.Finished);
		Autoplay = false;
		Playing = false;
	}
	public AudioStream Get_Muisc()
	{
		int Array_index = -1;
		if (Muisc_ID != null){
		if (Muisc_ID.Find(":") != -1)
		{
			if (Muisc_ID.Find("CH:") != -1)
			{
				Array_index = AudioList_Object.Muisc_List[0].IndexOf(Muisc_ID[3..]);
			}
			else
			{
				Array_index = AudioList_Object.Muisc_List[1].IndexOf(Muisc_ID);
			}
		}
		else
		{
			if (Muisc_ID.Find("0") != -1 || Muisc_ID.Find("1") != -1 || Muisc_ID.Find("2") != -1 || Muisc_ID.Find("3") != -1 ||
			Muisc_ID.Find("4") != -1 || Muisc_ID.Find("5") != -1 || Muisc_ID.Find("6") != -1 || Muisc_ID.Find("7") != -1 || Muisc_ID.Find("8") != -1
			|| Muisc_ID.Find("9") != -1){
			Array_index = int.Parse(Muisc_ID);}
		}
		}
		if (Array_index != -1){
			AudioStream stream = (AudioStream)AudioList_Object.Muisc_List[2][Array_index];
			return stream;
		}
		else
		{
			return null;
		}
	}
}
}