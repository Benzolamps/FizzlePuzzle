import xlrd


class CustomLevelGenerator:
  def __init__(self, config_path, layout_path):
    self.__layout_path = layout_path
    self.__prepare_config(config_path)
    self.__prepare_layout(layout_path)
    self.__generate_layout()
    self.__generate_config()

  def __add_pillar(self, position, size):
    index = -1
    for i in range(0, len(self.__object_list)):
      if self.__object_list[i]["position"] == position:
        index = i
        break

    if index == -1:
      self.__object_list.append({
        "class": self.__wall_style,
        "name": "pillar",
        "position": position,
        "size": size,
        "repeat-target": 1
      })
    else:
      size0_cmp = cmp(self.__object_list[index]["size"][0], size[0])
      size1_cmp = cmp(self.__object_list[index]["size"][1], size[1])
      size2_cmp = cmp(self.__object_list[index]["size"][2], size[2])
      if size0_cmp == 0 and size1_cmp == 0:
        if size2_cmp < 0:
          del self.__object_list[index]
        else:
          return
      else:
        return
      self.__object_list.append({
        "class": self.__wall_style,
        "name": "pillar",
        "position": position,
        "size": size,
        "repeat-target": 1
      })

  @staticmethod
  def __get_or_default(dicts, keys, default_value=None):
    return dicts[keys] if keys in dicts else default_value

  @staticmethod
  def __static_del(dicts, *keys):
    for k in keys:
      if k in dicts:
        del dicts[k]

  @staticmethod
  def __get_or_raise(dicts, keys, exception=None):
    try:
      return dicts[keys]
    except KeyError as error:
      raise exception if exception is not None else error

  def __prepare_layout(self, path):
    try:
      self.__table = xlrd.open_workbook(path).sheets()[0]
      assert isinstance(self.__table, xlrd.sheet.Sheet)
    except Exception as e:
      raise Exception("Excel表格中只能包含一个工作簿!" + str(type(e)) + e.message)

  def __prepare_config(self, path):
    self.__config = load_yaml(path)
    self.__player_spawn_target = self.__get_or_raise(self.__config, "player-spawn-target", AssertionError("必须指定角色出生点!"))
    self.__level_end_target = self.__get_or_raise(self.__config, "level-end-target", AssertionError("必须指定关卡终点!"))
    self.__wall_style = self.__get_or_default(self.__config, "wall-style", "Wall01")
    self.__floor_style = self.__get_or_default(self.__config, "floor-style", "Floor01")
    self.__level_border_height = self.__get_or_default(self.__config, "level-border-height", 2.5)
    self.__base_height = self.__get_or_default(self.__config, "base-height", 2.5)
    self.__static_del(self.__config, "player-spawn-target", "level-end-target", "wall-style", "floor-style", "level-border-height", "base-height")

  def __generate_layout(self):
    self.__object_list = [
      {
        "class": self.__floor_style,
        "name": "base",
        "position": (self.__table.ncols * 1.25 + 0.25, -0.5, self.__table.nrows * 1.25 + 0.25),
        "size": (self.__table.ncols * 2.5 + 0.5, 0.5, self.__table.nrows * 2.5 + 0.5),
        "repeat-target": 3,
      },
      {
        "class": self.__wall_style,
        "name": "west",
        "position": (0.25, 0.0, self.__table.nrows * 1.25 + 0.25),
        "size": (0.5, 2.0 * self.__level_border_height, self.__table.nrows * 2.5 + 0.5),
        "repeat-target": 2,
      },
      {
        "class": self.__wall_style,
        "name": "north",
        "position": (self.__table.ncols * 1.25 + 0.25, 0.0, 0.25),
        "size": (self.__table.ncols * 2.5 + 0.5, 2.0 * self.__level_border_height, 0.5),
        "repeat-target": 1,
      },
      {
        "class": self.__wall_style,
        "name": "east",
        "position": (self.__table.ncols * 2.5 + 0.25, 0.0, self.__table.nrows * 1.25 + 0.25),
        "size": (0.5, 2.0 * self.__level_border_height, self.__table.nrows * 2.5 + 0.5),
        "repeat-target": 2,
      },
      {
        "class": self.__wall_style,
        "name": "south",
        "position": (self.__table.ncols * 1.25 + 0.25, 0.0, self.__table.nrows * 2.5 + 0.25),
        "size": (self.__table.ncols * 2.5 + 0.5, 2.0 * self.__level_border_height, 0.5),
        "repeat-target": 1,
      }
    ]
    self.__add_pillar(
      (0.25, 0.0, 0.25),
      (0.7, 2.0 * self.__level_border_height + 0.5, 0.7)
    )
    self.__add_pillar(
      (self.__table.ncols * 2.5 + 0.25, 0.0, 0.25),
      (0.7, 2.0 * self.__level_border_height + 0.5, 0.7)
    )
    self.__add_pillar(
      (0.25, 0.0, self.__table.nrows * 2.5 + 0.25),
      (0.7, 2.0 * self.__level_border_height + 0.5, 0.7)
    )
    self.__add_pillar(
      (self.__table.ncols * 2.5 + 0.25, 0.0, self.__table.nrows * 2.5 + 0.25),
      (0.7, 2.0 * self.__level_border_height + 0.5, 0.7)
    )
    self.__generate_rows()
    self.__generate_columns()
    self.__generate_object()

  def __generate_rows(self):
    for i in range(1, self.__table.nrows):
      cur_back_height = -self.__base_height
      cur_span = 1
      length = len(self.__table.row_values(i))
      for j in range(0, length + 1):
        if j < length:
          item = self.__table.row_values(i)[j]
          data = load_yaml_str(item)
          data = data if isinstance(data, dict) else {}
          col_back_height = self.__get_or_default(data, "back", -self.__base_height)
        else:
          col_back_height = -self.__base_height
        if col_back_height != cur_back_height:
          if cur_back_height + self.__base_height > 0:
            position_x = 2.5 * j - 1.25 * cur_span + 0.25
            position_y = 0
            position_z = i * 2.5 + 0.25
            self.__object_list.append({
              "class": self.__wall_style,
              "name": "wall",
              "position": (position_x, position_y, position_z),
              "size": (2.5 * cur_span + 0.5, (self.__base_height + cur_back_height) * 2.0, 0.5),
              "repeat-target": 1
            })
            self.__add_pillar(
              (2.5 * j + 0.25, position_y, position_z),
              (0.7, ((self.__base_height + cur_back_height) if j < length else self.__level_border_height) * 2.0 + 0.5, 0.7)
            )
            self.__add_pillar(
              (2.5 * (j - cur_span) + 0.25, position_y, position_z),
              (0.7, ((self.__base_height + cur_back_height) if j > cur_span else self.__level_border_height) * 2.0 + 0.5, 0.7)
            )
          cur_back_height = col_back_height
          cur_span = 1
        else:
          cur_span += 1

  def __generate_columns(self):
    for i in range(1, self.__table.ncols):
      cur_left_height = -self.__base_height
      cur_span = 1
      length = len(self.__table.col_values(i))
      for j in range(0, length + 1):
        if j < length:
          item = self.__table.col_values(i)[j]
          data = load_yaml_str(item)
          data = data if isinstance(data, dict) else {}
          row_left_height = self.__get_or_default(data, "left", -self.__base_height)
        else:
          row_left_height = -self.__base_height
        if row_left_height != cur_left_height:
          """
              i = 1, z = 0.5 + 2 + 0.25
              i = 2, z = 0.5 + 2 + 0.5 + 2 + 0.25
              i = n, z = 2.5 * n + 0.25

              j = 1, span = 1, x = (0.5 + 2 + 0.5) * 0.5
              j = 2, span = 2, x = (0.5 + 2 + 0.5 + 2 + 0.5) * 0.5
              j = 2, span = 1, x = 0.5 + 2 + (0.5 + 2 + 0.5) * 0.5
              j = 3, span = 3, x = (0.5 + 2 + 0.5 + 2 + 0.5 + 2 + 0.5) * 0.5
              j = 3, span = 2, x = 0.5 + 2 + (0.5 + 2 + 0.5 + 2 + 0.5) * 0.5
              j = 3, span = 1, x = 0.5 + 2 + 0.5 + 2 + (0.5 + 2 + 0.5) * 0.5
              j = m, span = n, x = (m - n) * 2.5 + (n * 2.5 + 0.5) * 0.5
                                 = 2.5 * m - 2.5 * n + 1.25 * n + 0.25
                                 = 2.5 * m - 1.25 * n + 0.25
              0.5 * (9 + 1) + 9 * 2
          """
          if cur_left_height + self.__base_height > 0:
            position_z = 2.5 * j - 1.25 * cur_span + 0.25
            position_y = 0.0
            position_x = i * 2.5 + 0.25
            self.__object_list.append({
              "class": self.__wall_style,
              "name": "wall",
              "position": (position_x, position_y, position_z),
              "size": (0.5, (self.__base_height + cur_left_height) * 2.0, 2.5 * cur_span + 0.5),
              "repeat-target": 2
            })
            self.__add_pillar(
              (position_x, position_y, 2.5 * j + 0.25),
              (0.7, ((self.__base_height + cur_left_height) if j < length else self.__level_border_height) * 2.0 + 0.5, 0.7)
            )
            self.__add_pillar(
              (position_x, position_y, 2.5 * (j - cur_span) + 0.25),
              (0.7, ((self.__base_height + cur_left_height) if j > cur_span else self.__level_border_height) * 2.0 + 0.5, 0.7)
            )
          cur_left_height = row_left_height
          cur_span = 1
        else:
          cur_span += 1

  def __generate_object(self):
    for i in range(0, self.__table.nrows):
      length = len(self.__table.row_values(i))
      for j in range(0, length):
        item = self.__table.row_values(i)[j]
        data = load_yaml_str(item)
        data = data if isinstance(data, dict) else {}
        height = self.__get_or_default(data, "height", 0.0)
        if self.__get_or_default(data, "name", "untitled") == self.__player_spawn_target:
          self.__player_spawn_position = (2.5 * j + 1.5, 2.0 * height + 0.98, 2.5 * i + 1.5)
        if self.__get_or_default(data, "name", "untitled") == self.__level_end_target:
          self.__level_end_position = (2.5 * j + 1.5, 2.0 * height, 2.5 * i + 1.5)
        if "class" in data:
          data["position"] = (2.5 * j + 1.5, 2.0 * height, 2.5 * i + 1.5)
          data["size"] = (1.0, 1.0, 1.0)
          if "left" in data:
            del data["left"]
          if "back" in data:
            del data["back"]
          if "height" in data:
            del data["height"]
          self.__object_list.append(data)
        if height > 0.0:
          brick = {
            "class": self.__wall_style,
            "name": "floor",
            "position": (2.5 * j + 1.5, 0, 2.5 * i + 1.5),
            "size": (2.5, 2.0 * height, 2.5),
            "repeat-target": 2
          }
          self.__object_list.append(brick)
    assert self.__player_spawn_position is not None, "角色出生点" + self.__player_spawn_target + "未找到!"
    assert self.__level_end_position is not None, "关卡终点" + self.__level_end_target + "未找到!"

  def __generate_config(self):
    self.__config["player-spawn-position"] = self.__player_spawn_position
    self.__config["level-end-position"] = self.__level_end_position

  def get_object_list(self):
    return self.__object_list

  def get_config(self):
    return self.__config
