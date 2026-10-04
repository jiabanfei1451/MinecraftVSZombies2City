extends Node2D
@export_flags("Fire", "Water", "Earth", "Wind") var spell_elements = 0 

@export_flags("Self:4", "Allies:8", "Foes:16") var spell_targets = 0 

@export_flags("A:16", "B", "C") var x 

@export_flags("Fire", "Water", "Earth", "Wind") var phase_elements: Array[int]
