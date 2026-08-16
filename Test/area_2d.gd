extends Area2D


func _on_body_entered(body: Node2D) -> void:
	print(1)


func _on_body_exited(body: Node2D) -> void:
	print(2)
