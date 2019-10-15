from database import SqliteConnection
from util import load_yaml, convert_path


class SubtitleService(object):
  def __init__(self, path):
    self.__subtitles = load_yaml(convert_path(path))

  def get_subtitle(self, key):
    try:
      return self.__subtitles[key]
    except KeyError as e:
      raise Exception(e.message + key)


class StorageService(object):
  def __init__(self, db_path, sql_path):
    self.__conn = SqliteConnection(convert_path(db_path))
    self.__sqls = load_yaml(convert_path(sql_path))

  def is_achievement_stored(self, level_id, achv_name):
    result = self.__conn.execute_query(self.__sqls["is-achievement-stored"], level_id, achv_name)
    return len(result) > 0

  def store_achievement(self, level_id, achv_name, achv_desc):
    if self.is_achievement_stored(level_id, achv_name):
      self.__conn.execute_update(self.__sqls["store-achievement-update"], level_id, achv_name, achv_desc)
    else:
      subtitle(get_subtitle("achv-store") % (achv_name, achv_desc), "#00FFFF", 10.0)
      echo("achievement store, level-id = %s, achv-name = %s, achv_desc = %s" % (world_info.level_id, achv_name, achv_desc))
      self.__conn.execute_update(self.__sqls["store-achievement-insert"], level_id, achv_name, achv_desc)

  def is_key_set(self, level_id, key_name):
    result = self.__conn.execute_query(self.__sqls["is-key-set"], level_id, key_name)
    return len(result) > 0

  def get_key_value(self, level_id, key_name):
    result = self.__conn.execute_query(self.__sqls["get-key-value"], level_id, key_name)
    if len(result) == 0:
      raise AssertionError("Key %s, %s is not set!" % (level_id, key_name))
    return str(result[0]["key_value"])

  def store_key(self, level_id, key_name, key_value):
    key = "store-key-" + ["insert", "update"][self.is_key_set(level_id, key_name)]
    self.__conn.execute_update(self.__sqls[key], level_id, key_name, key_value)

  def is_level_set(self, level_id, level_index):
    result = self.__conn.execute_query(self.__sqls["is-level-set"], level_id, level_index)
    return len(result) > 0

  def get_best_time(self, level_id, level_index):
    result = self.__conn.execute_query(self.__sqls["get-best-time"], level_id, level_index)
    return float("inf" if len(result) == 0 else result[0]["level_best_time"])

  def store_level(self, level_id, level_index, level_name, level_dest_time, level_best_time):
    key = "store-level-" + ["insert", "update"][self.is_level_set(level_id, level_index)]
    self.__conn.execute_update(self.__sqls[key], level_id, level_index, level_name, level_dest_time, level_best_time)

  def get_max_level_index(self, level_id):
    result = self.__conn.execute_query(self.__sqls["get-max-level-index"], level_id)
    return int(result[0]["max_level_index"])
