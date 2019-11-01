using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;
using FizzlePuzzle.Item;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class ForkExchange : FizzleBehaviour
    {
        [SerializeField] [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal string Activator;
        
        private SwitchResponse response;

        protected override void Update()
        {
            base.Update();
            response?.Test(
                () =>
                {
                    if (!FizzleScene.TimeCtrl.EnableFork)
                    {
                        FizzleScene.TimeCtrl.EnableForking();
                    }
                },
                () =>
                {
                    if (FizzleScene.TimeCtrl.EnableFork)
                    {
                        FizzleScene.TimeCtrl.DisableForking();
                    }
                }
            );
        }

        protected override void Start()
        {
            base.Start();
            if (Activator != null)
            {
                response = new SwitchResponse(Activator);
            }
        }
    }
}
