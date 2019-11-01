﻿using System;
using System.Collections.Generic;
 using System.Linq;
 using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;

namespace FizzlePuzzle.Item
{
    internal class SwitchResponse
    {
        private readonly BooleanExpression exp;
        private readonly Dictionary<string, ISwitch> switches;
        private bool? lastResult;

        public SwitchResponse(string exp)
        {
            if (exp == null)
            {
                return;
            }

            this.exp = new BooleanExpression(exp);
            switches = new Dictionary<string, ISwitch>();
            foreach (var variable in this.exp.GetVariables().Where(variable => FizzleScene.FindObject<ISwitch>(variable) != null))
            {
                switches[variable] = FizzleScene.FindObject<ISwitch>(variable);
            }
        }

        public bool Test()
        {
            return exp?.CalcResult(name => switches[name].Activated) == true;
        }

        public void Test(Action active, Action deactive)
        {
            Action action = () => { };
            bool result = Test();
            if (lastResult == result)
            {
                return;
            }
            (result ? active ?? action : deactive ?? action)();
            lastResult = result;
        }
    }
}
