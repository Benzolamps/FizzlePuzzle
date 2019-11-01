def main():

  def log_level_info():
    echo("Level load, level-id = %s, level-index = %d, level-name = %s, level-dest-time = %.2f, level-best-time = %.2f, fork-activator = %s" % (
      world_info.level_id, 
      world_info.level_index, 
      world_info.level_name, 
      world_info.level_dest_time, 
      get_best_time(world_info.level_id, world_info.level_index),
      world_info.fork_activator
    ))

  def detect_time():
    yield Delay(0.5)
    best_time = get_best_time(world_info.level_id, world_info.level_index)
    if world_info.global_time < best_time:
      best_time = world_info.global_time
      store_level(
        world_info.level_id,
        world_info.level_index,
        world_info.level_name,
        world_info.level_dest_time,
        best_time
      )
      best_time = world_info.global_time
    subtitle(get_subtitle('time-tip')
      % (
        world_info.level_index + 1,
        world_info.level_name,
        world_info.global_time,
        world_info.level_dest_time,
        best_time
      ),
      "#982655",
      10.0
    )
    echo("Level finish, level-id = %s, level-index = %d, level-name = %s, level-finish-time = %.2f level-dest-time = %.2f, level-best-time = %.2f"
      % (
        world_info.level_id, 
        world_info.level_index, 
        world_info.level_name, 
        world_info.global_time,
        world_info.level_dest_time,
        best_time
      )
    )
  
  def box_count():
    boxes = FizzleObject.get_objects_by_type[FizzleBox]()
    index = 0
    for item in boxes: index += 1
    return index

  def detect_steal():
    if world_info.level_id != "official": 
      return
    stolen = world_info.first_fizzle_character.carrying_object is not None
    exists_box = box_count() > 0
    if stolen:
      achv_name = "Companion Cube"
      achv_desc = "将盒子带到关卡终点"
      store_achievement(world_info.level_id, achv_name, achv_desc)
    if stolen or not exists_box:
      if not is_key_set(world_info.level_id, "box-stolen-" + str(world_info.level_index)):
        store_key(world_info.level_id, "box-stolen-" + str(world_info.level_index), 1)
        if not is_key_set(world_info.level_id, "box-stolen-count"):
          count = 1
        else:
          count = int(get_key_value(world_info.level_id, "box-stolen-count")) + 1
        store_key(world_info.level_id, "box-stolen-count", count)
        if count >= 7:
          achv_name = "Sorry, I'm not Companion Cube"
          achv_desc = "在每个有盒子的关卡中将任意盒子带到关卡终点"
          store_achievement(world_info.level_id, achv_name, achv_desc)

  def reverse_detect():
    if world_info.level_id != "official": 
      return
    achv_name = "时光正流"
    achv_desc = "尝试时光正流"
    store_achievement(world_info.level_id, achv_name, achv_desc)

  def get_current_rewind_speed():
    try:
      return world_info.current_rewind_speed
    except Exception:
      return -1

  run_on(Event(world_info.level_finished), detect_time, detect_steal)
  run_on(Until(lambda: get_current_rewind_speed() == 16), reverse_detect)
  log_level_info()

main()
del main
