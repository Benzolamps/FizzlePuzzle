# coding=utf-8

"""
author: Benzolamps
用于连接数据库的类
"""

import sqlite3  # 导入sqlite3模块
import os  # 导入os模块


# 定义SQLite数据库连接类, 它的父类是object
class SqliteConnection(object):
  # 定义构造方法, self代表当前对象, path代表SQLite数据库文件位置
  def __init__(self, path):
    # 文件不存在
    if not os.path.exists(path):
      raise AssertionError("File " + path + " doesn't exist!")  # 抛出一个异常
    else:
      self.__path = path

  # 连接数据库
  def __connect(self):
    self.__conn = sqlite3.connect(self.__path)  # 连接数据库

  # 与数据库断开连接
  def __disconnect(self):
    self.__conn.commit()  # 提交修改 rollback回滚修改
    self.__conn.close()  # 关闭连接

  # 执行查询语句, 根据语句不同, 返回不同的结果
  # 在参数前面添加*号代表该参数是个可变长度参数
  def execute_query(self, sql, *args):
    self.__connect()  # 连接数据库
    result = self.__conn.execute(sql, args)  # 执行查询语句
    try:
      # 判断是否是一个查询语句
      if result.description:
        # for ... in 结构遍历集合 []代表列表(list), ()代表元组(tuple)
        # list中的值可以修改, tuple不能修改
        fields = [desc[0] for desc in result.description]  # 获取每个字段的字段名
        rows = [rows for rows in result]  # 获取每一行的数据
        # 将字段跟数据组合起来输出
        # zip将两个列表组合成键值对, dict将键值对组合成字典
        # json.dumps将Python对象转换为JSON字符串
        return [dict(zip(fields, content)) for content in rows]
      else:
        raise AssertionError("Not a 'SELECT' sentence!")  # 抛出一个异常
    finally:
      self.__disconnect()

  # 执行更新语句
  def execute_update(self, sql, *args):
    self.__connect()  # 连接数据库
    result = self.__conn.execute(sql, args)  # 执行更新语句
    try:
      # 判断是否是一个更新语句
      if result.description:
        raise AssertionError("Not a 'DELETE' 'INSERT' or 'UPDATE' sentence!")
      else:
        return result.rowcount  # 返回操作后受影响的行数
    finally:
      self.__disconnect()

  # 执行一条语句
  def execute(self, sql, *args):
    self.__connect()
    self.__conn.execute(sql, args)
    self.__disconnect()
