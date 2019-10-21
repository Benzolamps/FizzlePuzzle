using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class FizzleFigure
    {
        private readonly Transform figureContainer;

        private readonly Text rewindText;

        private readonly Text forkText;

        private readonly Text globalText;

        private readonly Text fpsText;

        private readonly Text distText;

        internal FizzleFigure(Transform figureContainer)
        {
            this.figureContainer = figureContainer;
            rewindText = figureContainer.Find("rewind text").GetComponent<Text>();
            forkText = figureContainer.Find("fork text").GetComponent<Text>();
            globalText = figureContainer.Find("global text").GetComponent<Text>();
            fpsText = figureContainer.Find("fps text").GetComponent<Text>();
            distText = figureContainer.Find("dist text").GetComponent<Text>();
        }

        internal void StartFigure()
        {
            figureContainer.GetComponent<Image>().StartCoroutine(InternalProcess());
        }

        private IEnumerator InternalProcess()
        {
            while (true)
            {
                if (FizzleScene.Ready)
                {
                    GenerateRewind();
                    GenerateFork();
                    GenerateGlobal();
                    GenerateDistance();
                    GenerateFPS();
                }
                yield return new WaitForSeconds(0.1F);
            }
        }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        private void GenerateFork()
        {
            if (FizzleScene.TimeCtrl.EnableFork)
            {
                if (FizzleScene.TimeCtrl.Forking)
                {
                    forkText.text = FizzleScene.Subtitle["fork-text-sample"].ToString().Replace("%time%", SecondsToString(FizzleScene.TimeCtrl.ForkSecondsRemain));
                }
                else
                {
                    forkText.text = FizzleScene.Subtitle["fork-active"].ToString();
                }
            }
            else
            {
                forkText.text = FizzleScene.Subtitle["fork-deactive"].ToString();
            }
        }

        private void GenerateRewind()
        {
            rewindText.text = FizzleScene.Subtitle["rewind-text-sample"].ToString().Replace("%speed%", FizzleScene.TimeCtrl.Rewinding ? "[" + FizzleScene.TimeCtrl.CurrentRewindSpeed + "×]" : "").Replace("%time%", SecondsToString(FizzleScene.TimeCtrl.RewindSeconds));
        }

        private void GenerateGlobal()
        {
            globalText.text = FizzleScene.Subtitle["global-text-sample"].ToString().Replace("%time%", SecondsToString(FizzleScene.TimeCtrl.GlobalSeconds));
        }

        private void GenerateDistance()
        {
            distText.text = FizzleScene.Subtitle["distance-text-sample"].ToString().Replace("%dist%", FizzleScene.FirstPersonCharAction.Distance.ToString("0.00"));
        }
        
        private void GenerateFPS()
        {
            fpsText.text = FizzleScene.Subtitle["fps-text-sample"].ToString().Replace("%fps%", (1.0F / Time.deltaTime).ToString("0.00"));
        }
        
        private static string SecondsToString(float seconds)
        {
            int hour = 0;
            int minute = 0;
            int second = (int) seconds;
            int millisecond = (int) ((seconds - second) * 100.0F);
            if (second >= 60)
            {
                minute = second / 60;
                second %= 60;
            }

            if (minute >= 60)
            {
                hour = minute / 60;
                minute %= 60;
            }

            return hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00") + "." + millisecond.ToString("00");
        }
    }
}