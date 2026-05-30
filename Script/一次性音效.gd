extends AudioStreamPlayer
@export_enum("音乐","音效") var 选项 : String = "音乐"

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if 选项 == "音乐":
		if 音量.音乐音量 != 0:
			volume_db = -25 +音量.音乐音量 / 4
		else:volume_db = -100
	if 选项 == "音效":
		if 音量.音效音量!= 0:
			volume_db = -25 +音量.音效音量 / 4
		else:volume_db = -100

func _on_finished() -> void:
	queue_free()
