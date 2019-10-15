# 示例：当三个FizzleTrigger都被触发时，打印三个FizzleTrigger都被触发了
trigger1 = FizzleTrigger("trigger1")
trigger2 = FizzleTrigger("trigger2")
trigger3 = FizzleTrigger("trigger3")
run_on(All(Event(trigger1.active), Event(trigger2.active), Event(trigger3.active)), lambda: echo("三个FizzleTrigger都被触发了"))
