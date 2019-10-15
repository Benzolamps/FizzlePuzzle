# 示例：在分身还有三秒就要消失时，打印分身还有三秒就要消失了
def _3_seconds():
  while True:
    if world_info.forking and world_info.fork_time_remain <= 3:
      subtitle("分身还有三秒就要消失了")
    yield
run(_3_seconds)


