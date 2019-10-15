using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleCurtain : FizzleBehaviour, FizzleItem
    {
        protected Transform glass;

        protected override void Awake()
        {
            base.Awake();
            glass = transform.Find("glass");
        }

        public virtual void Generate(FizzleJson data)
        {
            string curtainAlign = data.GetOrDefault("curtain-align", "left");
            if (curtainAlign == "back")
            {
                transform.position -= 1.25F * Vector3.forward;
            }
            else
            {
                transform.position -= 1.25F * Vector3.right;
                transform.eulerAngles -= 90.0F * Vector3.up;
            }
        }

        protected void SetColor(FizzleColor color)
        {
            glass.SetColor(color.Replace(a: 120));
        }
    }
}
