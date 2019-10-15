# 示例：当玩家拿起方块时，显示你拿起了+方块名字
c = world_info.first_fizzle_character
c.carried_object += lambda b: echo("你拿起了" + b.name)

# 示例：当关卡进行20秒时，让场景中所有的方块隐藏，三秒后再显示
def hide_action():
  boxes = FizzleObject.get_objects_by_type[FizzleBox]()
  yield Until(lambda: world_info.global_time >= 20)
  for box in boxes:
    box.hide()
  yield Delay(3.0)
  for box in boxes:
    box.show()
run(hide_action)
