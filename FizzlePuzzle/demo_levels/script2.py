# 示例：FizzleTrigger触发时，打印FizzleTrigger被触发了
trigger = FizzleTrigger("trigger")
run_on(Event(trigger.active), lambda: echo("FizzleTrigger被触发了"))
