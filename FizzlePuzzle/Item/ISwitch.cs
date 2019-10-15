﻿using FizzlePuzzle.Core;

namespace FizzlePuzzle.Item
{
    internal interface ISwitch
    {
        bool Activated { get; }

        event FizzleEvent active;

        event FizzleEvent deactive;
    }
}
