# coding=utf-8
import abc


def echo(obj, color="#FFFFFF"):
  # type: (object, str) -> None
  """向控制台打印一条信息"""
  pass


def error(obj):
  # type: (object) -> None
  """向控制台打印一条错误信息"""
  pass


def warn(obj):
  # type: (object) -> None
  """向控制台打印一条警告信息"""
  pass


def subtitle(obj, color="#FFFFFF", seconds=2.0):
  # type: (object, str, float) -> None
  """显示一条字幕"""
  pass


class BoundEvent(object):
  """表示事件"""
  def __add__(self, other):
    # type: (BoundEvent, function) -> BoundEvent
    """为事件安装监听"""
    pass

  def __sub__(self, other):
    # type: (BoundEvent, function) -> BoundEvent
    """为事件卸载监听"""
    pass


def convert_path(path):
  # type: (str) -> str
  """将带有~的路径转换为绝对路径"""
  pass


def load_yaml(path):
  # type: (str) -> object
  """将YAML文件转换为对象"""
  pass


def load_yaml_str(path):
  # type: (str) -> object
  """将YAML字符串转换为对象"""
  pass


def to_json(obj):
  # type: (object) -> str
  """将obj转换为JSON字符串"""
  pass


class FizzleCoroutine(object):
  """条件抽象类"""
  @abc.abstractmethod
  def wait(self):
    """等待"""
    pass


class Event(FizzleCoroutine):
  """事件条件, 当事件发生时执行"""
  def __init__(self, fevt):
    # type: (Event, BoundEvent) -> None
    pass

  def wait(self):
    pass


class Delay(FizzleCoroutine):
  """延时条件, 当延时结束时执行"""
  def __init__(self, seconds):
    # type: (Delay, float) -> None
    pass

  def wait(self):
    pass


class While(FizzleCoroutine):
  """当条件, 当条件满足时执行"""
  def __init__(self, cond):
    # type: (While, function) -> None
    pass

  def wait(self):
    pass


class Until(FizzleCoroutine):
  """直到条件, 直到条件不满足时执行"""
  def __init__(self, cond):
    # type: (Until, function) -> None
    pass

  def wait(self):
    pass


class All(FizzleCoroutine):
  """所有条件都满足时执行"""
  def __init__(self, *motions):
    # type: (All, FizzleCoroutine) -> None
    pass

  def wait(self):
    pass


class Any(FizzleCoroutine):
  """任意一个条件满足时执行"""
  def __init__(self, *motions):
    # type: (Any, FizzleCoroutine) -> None
    pass

  def wait(self):
    pass


def forever():
  # type: () -> None
  """无限等待"""
  pass


def run(*funs):
  # type: (function) -> None
  """执行异步函数"""
  pass


def run_on(motion, *funs):
  # type: (FizzleCoroutine, function) -> None
  """当条件满足时, 执行异步函数"""
  pass


def run_on_every(motion, *funs):
  # type: (FizzleCoroutine, function) -> None
  """每当条件满足时, 执行异步函数"""
  pass


class FizzleObject(object):
  """物体抽象类"""

  @property
  def name(self):
    # type: (FizzleObject) -> str
    """物体名称"""
    return "box1"

  def destroy(self):
    # type: (FizzleObject) -> None
    """摧毁物体"""

    pass

  def disappear(self):
    # type: (FizzleObject) -> None
    """让物体消失"""
    pass

  def appear(self):
    # type: (FizzleObject) -> None
    """让物体出现"""
    pass

  def hide(self):
    # type: (FizzleObject) -> None
    """让物体隐藏"""
    pass

  def show(self):
    # type: (FizzleObject) -> None
    """让物体展示"""
    pass

  @property
  def get_objects_by_type(self):
    # type: (FizzleObject) -> dict
    """通过类型获取物体"""
    return {}


class FizzleSwitch(FizzleObject):
  """开关物体抽象类"""
  @property
  def activated(self):
    # type: (FizzleSwitch) -> bool
    """是否激活"""
    return False

  @property
  def active(self):
    # type: (FizzleSwitch) -> BoundEvent
    """激活事件"""
    return BoundEvent()

  @property
  def deactive(self):
    # type: (FizzleSwitch) -> BoundEvent
    """取消激活事件"""
    return BoundEvent()


class FizzleButton(FizzleSwitch):
  """开关"""
  def __init__(self, name):
    # type: (FizzleButton, str) -> None
    pass

  @property
  def active_color(self):
    # type: (FizzleButton) -> str
    """激活颜色"""
    return "#00FF00"

  @property
  def deactive_color(self):
    # type: (FizzleButton) -> str
    """激活颜色"""
    return "#FF0000"


class FizzleLogicCurtain(FizzleSwitch):
  """逻辑门"""
  def __init__(self, name):
    # type: (FizzleLogicCurtain, str) -> None
    pass

  @property
  def active_color(self):
    # type: (FizzleLogicCurtain) -> str
    """激活颜色"""
    return "#00FF00"

  @property
  def deactive_color(self):
    # type: (FizzleLogicCurtain) -> str
    """取消激活颜色"""
    return "#FF0000"


class PressurePlate(FizzleSwitch):
  """压力板"""
  def __init__(self, name):
    # type: (PressurePlate, str) -> None
    pass

  @property
  def active_color(self):
    # type: (PressurePlate) -> str
    """激活颜色"""
    return "#00FF00"

  @property
  def deactive_color(self):
    # type: (PressurePlate) -> str
    """取消激活颜色"""
    return "#FF0000"


class FizzleTrigger(FizzleSwitch):
  """触发器"""
  def __init__(self, name):
    # type: (FizzleTrigger, str) -> None
    pass

  @property
  def trigger_type(self):
    # type: (FizzleTrigger) -> str
    """触发器类型, ONCE, ALWAYS"""
    return "ONCE"

  def recharge(self):
    """重新充能, 使其可被再次触发"""
    pass


class FizzleResponse(FizzleObject):
  """反馈物体抽象类"""
  @property
  def activator(self):
    # type: (FizzleResponse) -> str
    """激活条件表达式"""
    return "button1"


class FizzleBarrier(FizzleResponse):
  """屏障门"""
  def __init__(self, name):
    pass

  @property
  def opening(self):
    # type: (FizzleBarrier) -> bool
    """是否已打开"""
    return False

  @property
  def color(self):
    # type: (FizzleBarrier) -> str
    """颜色"""
    return "#0000FF"

  @property
  def opened(self):
    # type: (FizzleBarrier) -> BoundEvent
    """打开事件"""
    return BoundEvent()

  @property
  def closed(self):
    # type: (FizzleBarrier) -> BoundEvent
    """关闭事件"""
    return BoundEvent()

  def open(self):
    # type: () -> None
    """打开"""
    pass

  def close(self):
    # type: () -> None
    """关闭"""
    pass


class FizzleElevator(FizzleResponse):
  """电梯"""
  def __init__(self, name):
    pass

  @property
  def height(self):
    # type: (FizzleElevator) -> float
    """电梯高度"""
    return 2.0

  @property
  def status(self):
    # type: (FizzleElevator) -> str
    """电梯状态, RAISED, RAISING, DROPPED, DROPPING"""
    return "DROPPED"

  @property
  def raised(self):
    # type: (FizzleElevator) -> BoundEvent
    """升起事件"""
    return BoundEvent()

  @property
  def dropped(self):
    # type: (FizzleElevator) -> BoundEvent
    """降落事件"""
    return BoundEvent()

  @property
  def raise_finished(self):
    # type: (FizzleElevator) -> BoundEvent
    """升起结束事件"""
    return BoundEvent()

  @property
  def drop_finished(self):
    # type: (FizzleElevator) -> BoundEvent
    """降落结束事件"""
    return BoundEvent()

  def raise_(self):
    # type: () -> None
    """上升"""
    pass

  def drop(self):
    # type: () -> None
    """下降"""
    pass


class FizzleBox(FizzleObject):
  def __init__(self, name):
    pass

  @property
  def rewindable(self):
    # type: (FizzleBox) -> bool
    """是否受时间倒流影响"""
    return True

  @property
  def carried(self):
    # type: (FizzleBox) -> bool
    """是否被携带着"""
    return False

  @property
  def active(self):
    # type: (FizzleBox) -> BoundEvent
    """携带事件"""
    return BoundEvent()

  @property
  def deactive(self):
    # type: (FizzleBox) -> BoundEvent
    """放下事件"""
    return BoundEvent()


class FizzleScript(FizzleObject):
  """脚本类"""
  def __init__(self, name):
    pass

  @property
  def path(self):
    # type: (FizzleScript) -> str
    """路径"""
    return "E:/test.py"

  @property
  def code(self):
    # type: (FizzleScript) -> str
    """代码"""
    return "echo('Hello')"

  def execute(self):
    # type: () -> None
    """执行"""
    pass


class FizzleCharacter(FizzleObject):
  """角色抽象类"""
  @property
  def carrying_object(self):
    # type: (FizzleCharacter) -> FizzleBox
    """携带着的物体"""
    return FizzleBox("box1")

  @property
  def distance(self):
    # type: (FizzleCharacter) -> float
    """与面前物体的距离"""
    return 10.0

  @property
  def carried_object(self):
    # type: (FizzleCharacter) -> BoundEvent
    """拿起物体事件"""
    return BoundEvent()

  @property
  def released_object(self):
    # type: (FizzleCharacter) -> BoundEvent
    """放下物体事件"""
    return BoundEvent()

  @property
  def pressed_button(self):
    # type: (FizzleCharacter) -> BoundEvent
    """按下按钮事件"""
    return BoundEvent()


class FirstFizzleCharacter(FizzleCharacter):
  """第一人称角色类"""
  pass


class ForkFizzleCharacter(FizzleCharacter):
  """分类角色类"""
  pass


class WorldInfo(object):
  """世界信息"""
  @property
  def level_id(self):
    # type: (WorldInfo) -> str
    """关卡集id"""
    return "demo"

  @property
  def level_name(self):
    # type: (WorldInfo) -> str
    """当前关卡名称"""
    return "鹊桥相会"

  @property
  def level_count(self):
    # type: (WorldInfo) -> int
    """关卡集中的关卡总数"""
    return 5

  @property
  def level_index(self):
    # type: (WorldInfo) -> int
    """当前关卡在关卡集中的索引"""
    return 0

  @property
  def level_dest_time(self):
    # type: (WorldInfo) -> float
    """关卡目标时间"""
    return 12.0

  @property
  def global_time(self):
    # type: (WorldInfo) -> float
    """全局时间"""
    return 0.0

  @property
  def rewind_time(self):
    # type: (WorldInfo) -> float
    """可回溯时间"""
    return 0.0

  @property
  def max_rewind_speed(self):
    # type: (WorldInfo) -> int
    """最大回溯速度"""
    return 16

  @property
  def fork_time_remain(self):
    # type: (WorldInfo) -> float
    """分身剩余时间"""
    return 0.0

  @property
  def fork_activator(self):
    # type: (WorldInfo) -> str
    """分身激活的条件表达式"""
    return "True"

  @property
  def rewinding(self):
    # type: (WorldInfo) -> bool
    """是否在回溯"""
    return False

  @property
  def forking(self):
    # type: (WorldInfo) -> bool
    """是否在分身"""
    return False

  @property
  def max_rewind_time(self):
    # type: (WorldInfo) -> int
    """最大回溯时间"""
    return 500

  @property
  def current_rewind_speed(self):
    # type: (WorldInfo) -> int
    """当前回溯速度, -16, -8, -4, -2, -1, 0, 1, 2, 4, 8, 16"""
    return 1

  @property
  def frames_per_second(self):
    # type: (WorldInfo) -> float
    """FPS"""
    return 60.0

  @property
  def begin_rewinding(self):
    # type: (WorldInfo) -> BoundEvent
    """开始回溯事件"""
    return BoundEvent()

  @property
  def end_rewinding(self):
    # type: (WorldInfo) -> BoundEvent
    """结束回溯事件"""
    return BoundEvent()

  @property
  def time_out_rewinding(self):
    # type: (WorldInfo) -> BoundEvent
    """回溯超时事件"""
    return BoundEvent()

  @property
  def begin_forking(self):
    # type: (WorldInfo) -> BoundEvent
    """开始分身事件"""
    return BoundEvent()

  @property
  def end_forking(self):
    # type: (WorldInfo) -> BoundEvent
    """结束分身事件"""
    return BoundEvent()

  @property
  def level_finished(self):
    # type: (WorldInfo) -> BoundEvent
    """关卡完成事件"""
    return BoundEvent()

  @property
  def first_fizzle_character(self):
    # type: (WorldInfo) -> FirstFizzleCharacter
    """第一人称角色"""
    return FirstFizzleCharacter()

  @property
  def fork_fizzle_character(self):
    # type: (WorldInfo) -> ForkFizzleCharacter
    """分身角色"""
    return ForkFizzleCharacter()

  def get_fork_enabled(self):
    # type: () -> bool
    """分身是否可用"""
    pass

  def enable_fork(self):
    # type: () -> None
    """让分身可用"""
    pass

  def disable_fork(self):
    # type: () -> None
    """让分身不可用"""
    pass


def get_subtitle(key):
  # type: (str) -> str
  """获取subtitle.yml中的文本"""
  pass


def is_achievement_stored(level_id, achv_name):
  # type: (str, str) -> bool
  """成就是否解锁"""
  pass


def store_achievement(level_id, achv_name, achv_desc):
  # type: (str, str, str) -> None
  """解锁成就"""
  pass


def is_key_set(level_id, key_name):
  # type: (str, str) -> bool
  """件是否设置"""
  pass


def get_key_value(level_id, key_name):
  # type: (str, str) -> str
  """获取键值"""
  pass


def store_key(level_id, key_name, key_value):
  # type: (str, str, str) -> None
  """设置键值"""
  pass


def is_level_set(level_id, level_index):
  # type: (str, int) -> bool
  """关卡是否通关"""
  pass


def get_best_time(level_id, level_index):
  # type: (str, int) -> float
  """获取关卡最佳时间"""
  pass


def store_level(level_id, level_index, level_name, level_dest_time, level_best_time):
  # type: (str, int, str, float, float) -> None
  """关卡通关"""
  pass


def get_max_level_index(level_id):
  # type: (str) -> int
  """获取关卡最大的已通关索引"""
  pass


world_info = WorldInfo()  # type: WorldInfo
