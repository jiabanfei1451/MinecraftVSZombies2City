using System;
using Godot;
namespace SaveData;
public struct SaveData()
{
    public Godot.Collections.Dictionary<String,Variant> data {get;set;}
}