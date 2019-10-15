# coding=utf-8

from fizzle_puzzle import CommonUtility
from fizzle_puzzle.CommonUtility import delay, while_, until, FizzleCoroutine


class Event(FizzleCoroutine):
  def __init__(self, fevt):
    self.__fevt = fevt
    self.__flag = False

  def __call_back(self, *args):
    self.__flag = True

  def wait(self):
    self.__fevt += self.__call_back
    while not self.__flag:
      yield
    self.__flag = False
    self.__fevt -= self.__call_back


class Delay(FizzleCoroutine):
  def __init__(self, seconds):
    self.__seconds = seconds

  def wait(self):
    yield delay(self.__seconds)


class While(FizzleCoroutine):
  def __init__(self, cond):
    self.__cond = cond

  def wait(self):
    return while_(self.__cond)


class Until(FizzleCoroutine):
  def __init__(self, cond):
    self.__cond = cond

  def wait(self):
    return until(self.__cond)


class All(FizzleCoroutine):
  def __init__(self, *motions):
    self.__motions = motions

  def wait(self):
    __index = 0
    while True:
      for item in self.__motions:
        try:
          item().next()
        except StopIteration:
          __index += 1
      if __index >= len(self.__motions):
        return
      yield


class Any(FizzleCoroutine):
  def __init__(self, *motions):
    self.__motions = motions

  def wait(self):
    while True:
      for item in self.__motions:
        try:
          item().next()
        except StopIteration:
          return
      yield


def forever():
  while True:
    yield


def __run(fun):
  def action():
    yield fun()
  CommonUtility.run_async(action())


def __run_many(*funs):
  for fun in funs:
    __run(fun)


def run(*funs):
  # type: (callable) -> None
  """异步执行一个函数"""
  for fun in funs:
    __run(fun)


def run_on(motion, *funs):
  # type: (FizzleCoroutine, callable) -> None
  """在触发器触发时, 执行一个函数"""
  def action():
    yield motion.wait()
    __run_many(*funs)
  __run(action)


def run_on_every(motion, *funs):
  # type: (FizzleCoroutine, callable) -> None
  """在触发器每次触发时, 执行一个函数"""
  def action():
    for _ in forever():
      yield motion.wait
      __run_many(*funs)
  __run(action)
