using System.Collections;
using System.Diagnostics;
using System.Threading;
using FizzlePuzzle.Config;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FizzlePuzzle.UI
{
    internal class TitleProcess : FizzleBehaviour
    {
        [SerializeField] private Sprite[] m_Sprites;
        [SerializeField] private string m_SubtitlePath;
        [SerializeField] private Image m_LoadingScreen;
        [SerializeField] private Image m_SplashScreen;
        [SerializeField] private TextAsset m_DataAsset;
        
        private FizzleFade blackObj;
        private Image loadingObj;
        private Image splashObj;
        private FizzleJson subtitle;

        private void OnApplicationQuit()
        {
            StopAllCoroutines();
            Process.GetCurrentProcess().Kill();
        }

        private IEnumerator ShowProcess()
        {
            TitleProcess titleProcess = this;
            yield return new WaitForSeconds(0.5f);
            Thread thread = titleProcess.CopyProcess();
            thread.Start();
            yield return new WaitWhile(() => thread.IsAlive);
            if (FizzleSettings.CommandArgs.ContainsKey("skiplogo"))
            {
                yield return SceneManager.LoadSceneAsync(1);
            }
            else
            {
                titleProcess.subtitle = PythonGenerator.LoadYaml("~/Text/subtitle.yml");
                yield return titleProcess.blackObj.FadeIn();
                yield return new WaitForSeconds(4.0F);
                yield return titleProcess.blackObj.FadeOut();
                DestroyImmediate(titleProcess.splashObj);
                titleProcess.loadingObj.transform.Find("loading tip").GetComponent<Text>().text = titleProcess.subtitle["loading-tip"].ToString();
                Image loadingImage = titleProcess.loadingObj.transform.Find("loading image").GetComponent<Image>();
                Text loadingText = titleProcess.loadingObj.transform.Find("loading text").GetComponent<Text>();
                yield return new WaitForSeconds(0.5F);
                yield return titleProcess.blackObj.FadeIn();
                yield return new WaitForSeconds(4.0F);
                loadingText.text = titleProcess.subtitle["loading-text"].ToString();
                titleProcess.StartCoroutine(titleProcess.ShowAnimation(loadingImage));
                AsyncOperation result = SceneManager.LoadSceneAsync(1);
                result.allowSceneActivation = false;
                yield return new WaitUntil(() => result.progress >= 0.85F);
                yield return new WaitForSeconds(5.0F);
                yield return titleProcess.blackObj.FadeOut();
                yield return new WaitForSeconds(0.6F);
                result.allowSceneActivation = true;
            }
        }

        private Thread CopyProcess()
        {
            return new Thread(new DatabaseCopyProcess(m_DataAsset.bytes, FizzleSettings.CommandArgs.ContainsKey("erasure")).Process);
        }

        private IEnumerator ShowAnimation(Image image)
        {
            int index = 0;
            while (true)
            {
                image.sprite = m_Sprites[index++ % m_Sprites.Length];
                yield return new WaitForSeconds(0.1F);
            }
        }

        protected override void Awake()
        {
            FizzleSettings.RedirectException();
            loadingObj = Spawn(m_LoadingScreen, transform, "_Loading");
            splashObj = Spawn(m_SplashScreen, transform, "splash");
            blackObj = FizzleFade.CreateFade("#666666", transform);
            start += () => StartCoroutine(ShowProcess());
        }
    }
}
