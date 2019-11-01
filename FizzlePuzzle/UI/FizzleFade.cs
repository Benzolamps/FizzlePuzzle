using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class FizzleFade : FizzleBehaviour
    {
        [SerializeField] private FizzleColor m_Color = Color.black;

        private Image image;

        protected override void Awake()
        {
            image = gameObject.AddComponent<Image>();
        }

        protected override void Start()
        {
            image.color = m_Color;
            image.rectTransform.sizeDelta = new Vector2(Screen.width * 2.0F, Screen.height * 2.0F);
            image.rectTransform.localPosition = Vector3.zero;
        }

        public static FizzleFade CreateFade(FizzleColor color, Transform parent, bool hideDefault = false)
        {
            FizzleFade fizzleFade = Spawn<FizzleFade>(parent, "black");
            fizzleFade.m_Color = color.Replace(a: hideDefault ? 0 : byte.MaxValue);
            return fizzleFade;
        }

        public IEnumerator FadeIn()
        {
            float speed = Time.deltaTime / 2.0F;
            for (float rest = 1.0F; rest > 0.0F; rest -= speed)
            {
                image.color = image.color.Replace(a: rest);
                yield return new WaitForEndOfFrame();
            }

            image.color.Replace(a: 0.0F);
        }

        public IEnumerator FadeOut()
        {
            float speed = Time.deltaTime / 2.0F;
            for (float rest = 0.0F; rest < 1.0F; rest += speed)
            {
                image.color = image.color.Replace(a: rest);
                yield return new WaitForEndOfFrame();
            }

            image.color = image.color.Replace(a: 1.0F);
        }
    }
}
