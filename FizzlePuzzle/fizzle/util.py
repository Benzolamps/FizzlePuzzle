# coding=utf-8

import json
import yaml
import os
from fizzle_puzzle import CommonUtility


def echo(obj, color="#FFFFFF"):
  CommonUtility.echo(str(obj), color)


def error(obj):
  CommonUtility.error(str(obj))


def warn(obj):
  CommonUtility.warn(str(obj))


def subtitle(obj, color="#FFFFFF", seconds=2.0):
  CommonUtility.subtitle(str(obj), color, seconds)


world_info = CommonUtility.world_info
convert_path = CommonUtility.convert_path


def load_yaml_str(content):
  return yaml.load(content)


def load_yaml(path):
  if not os.path.exists(path):
    raise Exception("找不到文件" + path)
  fobj = open(path, "r")
  content = "\n".join(fobj).decode("UTF-8")
  fobj.close()
  return load_yaml_str(content)


def to_json(obj):
  return json.dumps(obj)
