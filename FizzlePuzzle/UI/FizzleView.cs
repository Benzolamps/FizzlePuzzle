using System.Collections;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class FizzleView : FizzleBehaviour
    {
        internal FizzleConsole fizzleConsole;
        internal FizzleSubtitle fizzleSubtitle;
        private FizzleFigure fizzleFigure;
        private Text titleText;
        private FizzleFade fade;

        protected override void Awake()
        {
            base.Awake();
            fade = FizzleFade.CreateFade((FizzleColor) "#666666", transform, true);
            fizzleConsole = new FizzleConsole(transform.Find("console"));
            fizzleSubtitle = new FizzleSubtitle(transform.Find("subtitle"));
            fizzleFigure = new FizzleFigure(transform.Find("figure display"));
            titleText = transform.Find("title").GetComponent<Text>();
        }

        internal IEnumerator FadeIn()
        {
            return fade.FadeIn();
        }

        internal IEnumerator FadeOut()
        {
            return fade.FadeOut();
        }

        protected override void Start()
        {
            base.Start();
            fizzleConsole.InitConsole();
            fizzleFigure.StartFigure();
        }

        public void ShowLevelName()
        {
            StartCoroutine(InternalLevelName());
        }

        private IEnumerator InternalLevelName()
        {
            titleText.text = FizzleScene.LevelName;
            yield return new WaitForSeconds(5.0F);
            titleText.text = string.Empty;
        }
    }
}