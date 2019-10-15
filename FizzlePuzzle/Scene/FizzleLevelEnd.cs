using FizzlePuzzle.Item;
using System.Collections;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class FizzleLevelEnd : FizzleTrigger
    {
        protected override void Awake()
        {
            base.Awake();
            _TriggerType = TriggerType.ONCE;
        }

        protected override void Start()
        {
            active += () => StartCoroutine(LoadNext());
        }

        internal static IEnumerator LoadNext()
        {
            yield return new WaitForSeconds(0.2f);
            FizzleScene.LoadNextLevel();
        }
    }
}