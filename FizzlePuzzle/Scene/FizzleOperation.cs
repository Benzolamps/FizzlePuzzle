using FizzlePuzzle.Core;
using System;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class FizzleOperation : FizzleBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            LockCursor();
        }

        private void OnApplicationQuit()
        {
            StopAllCoroutines();
            Process.GetCurrentProcess().Kill();
        }

        private static void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        [Conditional("UNITY_EDITOR")]
        private static void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        [SuppressMessage("ReSharper", "StaticProblemInText")]
        protected override void Update()
        {
            base.Update();
            if (Input.GetButtonDown("Use"))
                LockCursor();
            Input.GetButtonDown("Quit");
            if (Input.GetButtonDown("Quit"))
            {
                StopCoroutine("PressButton");
                StartCoroutine(PressButton("Quit", Application.Quit));
            }
            if (Input.GetButtonDown("Reset"))
            {
                StopCoroutine("PressButton");
                StartCoroutine(PressButton("Reset", FizzleScene.ResetLevel));
            }
            if (!Input.GetButtonDown("Console"))
                return;
            if (!FizzleScene.FizzleView.fizzleConsole.IsConsoleShowing)
                FizzleScene.FizzleView.fizzleConsole.ShowConsole();
            else
                FizzleScene.FizzleView.fizzleConsole.HideConsole();
        }

        private static IEnumerator PressButton(string name, Action action)
        {
            FizzleScene.FizzleView.fizzleSubtitle.ShowSubtitle(FizzleScene.Subtitle["press-" + name.ToLower()].ToString(), "#00FFFF", 5f);
            float time = Time.timeSinceLevelLoad;
            while (Time.timeSinceLevelLoad - time <= 3.0F)
            {
                yield return new WaitForEndOfFrame();
                if (!Input.GetButton(name))
                    yield break;
            }
            action();
        }
    }
}
