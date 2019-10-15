# coding=utf-8

__version__ = '1.0.0'

from util import echo, error, warn, subtitle, world_info, convert_path, load_yaml, load_yaml_str, to_json
from event import FizzleCoroutine, Event, Delay, While, Until, All, Any, run, run_on, run_on_every
from items import (
  FizzleObject,
  FizzleSwitch, FizzleButton, FizzleLogicCurtain, PressurePlate, FizzleTrigger,
  FizzleResponse, FizzleBarrier, FizzleElevator,
  FizzleBox,
  FizzleScript,
  FizzleCharacter, FirstFizzleCharacter, ForkFizzleCharacter,
  WorldInfo
)
from service import SubtitleService, StorageService

__subtitle_service = SubtitleService("~/Text/subtitle.yml")
get_subtitle = __subtitle_service.get_subtitle

__storage_service = StorageService("~/Resources/db.fizzle", "~/Plugins/fizzle/sql.yml")
is_achievement_stored = __storage_service.is_achievement_stored
store_achievement = __storage_service.store_achievement
is_key_set = __storage_service.is_key_set
get_key_value = __storage_service.get_key_value
store_key = __storage_service.store_key
is_level_set = __storage_service.is_level_set
get_best_time = __storage_service.get_best_time
store_level = __storage_service.store_level
get_max_level_index = __storage_service.get_max_level_index

del SubtitleService, StorageService, __subtitle_service, __storage_service
del util, event, items, service

