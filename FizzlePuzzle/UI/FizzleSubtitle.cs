using FizzlePuzzle.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class FizzleSubtitle
    {
        private readonly Text subtitle;
        private readonly Queue<SubtitlePair> stringQueue;

        internal FizzleSubtitle(Component subtitleTransform)
        {
            subtitle = subtitleTransform.GetComponent<Text>();
            stringQueue = new Queue<SubtitlePair>();
            subtitle.StartCoroutine(InitSubtitle());
        }

        internal void ShowSubtitle(string str, float seconds = 2.0F)
        {
            stringQueue.Enqueue(new SubtitlePair
            {
                content = str,
                color = (FizzleColor) "#FFFFFF",
                seconds = seconds
            });
        }

        internal void ShowSubtitle(string str, FizzleColor color, float seconds = 2.0F)
        {
            stringQueue.Enqueue(new SubtitlePair
            {
                content = str,
                color = color,
                seconds = seconds
            });
        }

        private IEnumerator InitSubtitle()
        {
            SubtitlePair lastSubtitlePair = new SubtitlePair();
            while (true)
            {
                if (stringQueue.Count == 0)
                {
                    lastSubtitlePair = new SubtitlePair();
                }
                yield return new WaitUntil(() => stringQueue.Count > 0);
                SubtitlePair subtitlePair = stringQueue.Dequeue();
                subtitle.text = $"<color={subtitlePair.color}>{subtitlePair.content}</color>";
                if (lastSubtitlePair.content == subtitlePair.content && lastSubtitlePair.color == subtitlePair.color)
                {
                    if (lastSubtitlePair.seconds < subtitlePair.seconds)
                    {
                        yield return new WaitForSeconds(subtitlePair.seconds - lastSubtitlePair.seconds);
                    }
                    else
                    {
                        subtitle.text = string.Empty;
                        continue;
                    }
                }
                else
                {
                    yield return new WaitForSeconds(subtitlePair.seconds);
                }

                lastSubtitlePair = subtitlePair;
                subtitle.text = string.Empty;
            }
        }
    }
}
