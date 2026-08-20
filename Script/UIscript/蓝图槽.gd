extends Control
@export var 显示 : Label
func _process(delta: float) -> void:
	设定卡槽($"卡槽",0)
	设定卡槽($"卡槽2",1)
	设定卡槽($"卡槽3",2)
	设定卡槽($"卡槽4",3)
	设定卡槽($"卡槽5",4)
	设定卡槽($"卡槽6",5)
	设定卡槽($"卡槽7",6)
	设定卡槽($"卡槽8",7)
	设定卡槽($"卡槽9",8)
	设定卡槽($"卡槽10",9)

func 设定卡槽(卡槽:Object,id:int):
	var qv:Array[int] = get_tree().current_scene.已选卡
	if 卡槽 != null:
		卡槽.器械ID = qv[id]
		卡槽.提示文字 = 显示
