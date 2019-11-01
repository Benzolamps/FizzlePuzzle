using UnityEngine;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class FizzleConsole
    {
        private readonly Transform console;
        private readonly Text[] bars;

        internal FizzleConsole(Transform console)
        {
            this.console = console;
            bars = new []
            {
                console.Find("text1").GetComponent<Text>(),
                console.Find("text2").GetComponent<Text>(),
                console.Find("text3").GetComponent<Text>()
            };
        }

        internal void InitConsole()
        {
            HideConsole();
        }

        internal bool IsConsoleShowing { get; private set; }

        internal void ShowMessage(string msg)
        {
            bars[0].text = bars[1].text;
            bars[1].text = bars[2].text;
            bars[2].text = msg;
        }

        internal void ShowConsole()
        {
            foreach (object obj in console.transform)
            {
                (obj as Transform)?.gameObject.SetActive(true);
                IsConsoleShowing = true;
            }
        }

        internal void HideConsole()
        {
            foreach (object obj in console.transform)
            {
                (obj as Transform)?.gameObject.SetActive(false);
                IsConsoleShowing = false;
            }
        }
    }
}
