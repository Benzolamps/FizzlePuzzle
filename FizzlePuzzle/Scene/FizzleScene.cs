using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FizzlePuzzle.Characters;
using FizzlePuzzle.Config;
using FizzlePuzzle.Core;
using FizzlePuzzle.TimeEffect;
using FizzlePuzzle.UI;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class FizzleScene : FizzleBehaviour
    {
        [SerializeField] [SuppressMessage("ReSharper", "IdentifierTypo")]
        private FizzleDefination m_FizzleDefination;

        private static CoroutineReceiver coroutineReceiver;
        private MapGenerate generate;
        private List<KeyValuePair<string, string>> sceneInfos;
        private FizzleSceneSetting sceneSetting;
        private int index;
        private Transform item;
        private Transform wall;
        private Transform @static;
        private ForkExchange forkExchange;
        private FizzleLevelEnd levelEnd;
        private static string commonScript;
        private static string customScript;
        internal static bool Ready { get; private set; }

        internal static TimeController TimeCtrl { get; private set; }

        internal static FizzleCharacterController FirstPersonCtrl { get; private set; }

        internal static FizzleCharacterController ForkCtrl { get; private set; }

        internal static FirstPersonCharacterAction FirstPersonCharAction { get; private set; }

        internal static ForkCharacterAction ForkCharAction { get; private set; }

        internal static string ForkActivator => GetInstance().forkExchange.Activator;

        internal static FizzleView FizzleView { get; private set; }

        internal static Camera Camera { get; private set; }

        internal static FizzleJson Subtitle { get; private set; }

        internal static FizzleJson Config { get; private set; }

        internal static FizzlePython Python { get; private set; }

        internal static string LevelName { get; private set; }

        internal static string LevelId { get; private set; }

        internal static float LevelDestTime { get; private set; }

        internal static event FizzleEvent levelFinished = () => { };

        internal static int LevelIndex => GetInstance().index;

        internal static int LevelCount { get; private set; }

        private void LoadLevel(int index)
        {
            while (transform.childCount > 1)
            {
                DestroyImmediate(transform.GetChild(transform.GetChild(0).name == "static" ? 1 : 0).gameObject);
            }

            if (TimeCtrl)
            {
                DestroyImmediate(TimeCtrl);
            }

            TimeCtrl = gameObject.AddComponent<TimeController>();
            Config = new FizzleJson(sceneInfos[index].Key);
            LevelName = Config.GetOrDefault("level-name", "untitled");
            LevelDestTime = Config.GetOrDefault("level-dest-time", 20.0F);
            FizzleView.ShowLevelName();
            ForkCtrl = Spawn(m_FizzleDefination.m_ForkPrefab, transform).GetComponent<ForkCharacterController>();
            ForkCharAction = ForkCtrl.GetComponent<ForkCharacterAction>();
            coroutineReceiver = Spawn<CoroutineReceiver>(transform, "coroutine");
            FirstPersonCtrl = Spawn(m_FizzleDefination.m_PlayerPrefab, transform);
            FirstPersonCharAction = FirstPersonCtrl.GetComponent<FirstPersonCharacterAction>();
            Camera = FirstPersonCtrl.transform.GetComponentInChildren<Camera>();
            TimeCtrl.timeControllerEnabled = false;
            FirstPersonCtrl.controllerEnabled = false;
            generate = new MapGenerate(m_FizzleDefination.m_GeneratableItemPrefabs, sceneInfos[index].Key, sceneInfos[index].Value);
            sceneSetting = new FizzleSceneSetting(m_FizzleDefination.m_LightPrefab);
            item = Spawn(transform, "container").transform;
            item.localPosition = Vector3.zero;
            item.localRotation = Quaternion.identity;
            wall = Spawn(transform, "wall").transform;
            wall.localPosition = Vector3.zero;
            wall.localRotation = Quaternion.identity;
        }

        protected override void Awake()
        {
            base.Awake();
            try
            {
                FizzleSettings.RedirectException();
                AssetBundle assetBundle = AssetBundle.LoadFromFile(CommonTools.ConvertPath("~/Resources/official-levels.fizzle"));
                commonScript = assetBundle.LoadAsset<TextAsset>("common.py").text;
                customScript = assetBundle.LoadAsset<TextAsset>("custom.py").text;
                Subtitle = PythonGenerator.LoadYaml("~/Text/subtitle.yml");
                @static = Spawn(transform, "static").transform;
                FizzleView = Spawn(m_FizzleDefination.m_FizzleView, @static);

                Python = PythonGenerator.Python;
                List<string> stringList;
                if (FizzleSettings.CommandArgs.ContainsKey("userlevel") && (stringList = FizzleSettings.CommandArgs["userlevel"]).Count > 0)
                {
                    string levelId;
                    Python.Execute(customScript);
                    sceneInfos = FizzleSettings.LoadSceneInfo(stringList[0], out levelId);
                    LevelId = levelId;
                }
                else
                {
                    SceneInfo[] osi = new SceneInfo[7];
                    for (int i = 0; i < osi.Length; i++)
                    {
                        osi[i] = new SceneInfo
                        {
                            m_ConfigText = assetBundle.LoadAsset<TextAsset>("config" + (i + 1)),
                            m_ObjectsText = assetBundle.LoadAsset<TextAsset>("layout" + (i + 1))
                        };
                    }

                    sceneInfos = new List<KeyValuePair<string, string>>(osi.Select(item => new KeyValuePair<string, string>(item.m_ConfigText.text, item.m_ObjectsText.text)));
                    LevelId = "official";
                }

                LevelCount = sceneInfos.Count;
                index = Python.GetVariable("get_max_level_index")(LevelId);
                LoadLevel(index = (index + 1) % LevelCount);
                TimeCtrl.timeControllerEnabled = true;
                FirstPersonCtrl.controllerEnabled = true;
                Ready = true;
            }
            catch (Exception)
            {
                Ready = false;
                throw;
            }
        }

        protected override void Start()
        {
            try
            {
                sceneSetting.Apply(transform, m_FizzleDefination.m_LightSettings[Config.GetOrDefault("light", 0) % m_FizzleDefination.m_LightSettings.Count], new List<string>(Config["audio"].Select(item => item.ToString())));
                generate.Generate(item, wall);
                FirstPersonCtrl.transform.position = JsonArrayToVector3(Config["player-spawn-position"]);
                string faceAt = Config.GetOrDefault("player-face-at", "forward");
                FirstPersonCtrl.transform.eulerAngles += 90.0F * (faceAt == "left" ? 1.0F : (faceAt == "back" ? 2.0F : (faceAt == "right" ? 3.0F : 0.0F))) * Vector3.up;
                forkExchange = Spawn<ForkExchange>(transform);
                forkExchange.Activator = Config.GetOrDefault<string>("fork-activator", null);
                levelEnd = Spawn(m_FizzleDefination.m_LevelEnd, transform);
                levelEnd.transform.position = JsonArrayToVector3(Config["level-end-position"]);
                Python.Execute(commonScript);
                Ready = true;
            }
            catch (Exception)
            {
                Ready = false;
                throw;
            }
        }

        internal static void LoadNextLevel()
        {
            try
            {
                levelFinished();
                GetInstance().StartCoroutine(InternalLoadLevel(GetInstance().index + 1));
                Ready = true;
            }
            catch (Exception)
            {
                Ready = false;
                throw;
            }
        }

        internal static void ResetLevel()
        {
            try
            {
                GetInstance().StartCoroutine(InternalLoadLevel(GetInstance().index));
                Ready = true;
            }
            catch (Exception)
            {
                Ready = false;
                throw;
            }
        }

        private static IEnumerator InternalLoadLevel(int index)
        {
            TimeCtrl.timeControllerEnabled = false;
            FirstPersonCtrl.controllerEnabled = false;
            yield return FizzleView.FadeOut();
            FizzleScene scene = GetInstance();
            scene.index = index % scene.sceneInfos.Count;
            scene.LoadLevel(scene.index);
            scene.Start();
            yield return FizzleView.FadeIn();
            TimeCtrl.timeControllerEnabled = true;
            FirstPersonCtrl.controllerEnabled = true;
        }

        private static FizzleScene GetInstance()
        {
            return FindObjectOfType<FizzleScene>();
        }

        internal static void StartOneCoroutine(IEnumerator coroutine)
        {
            coroutineReceiver?.StartCoroutine(coroutine);
        }

        internal static void StopOneCoroutine(IEnumerator coroutine)
        {
            coroutineReceiver?.StopCoroutine(coroutine);
        }

        internal static void StopAllCoroutine()
        {
            coroutineReceiver?.StopAllCoroutines();
        }

        internal static T FindObject<T>(string name) where T : class
        {
            return GetInstance().item.Find(name)?.GetComponent<ItemWrapper>()?.WrappedItem as T;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        internal static object FindObject(Type type, string name)
        {
            Transform transform = GetInstance().item.Find(name);
            if (transform == null)
            {
                return null;
            }

            ItemWrapper component = transform.GetComponent<ItemWrapper>();
            return component?.WrappedItem;
        }

        internal static IEnumerable<ItemWrapper> GetAllItems()
        {
            return GetInstance().item.gameObject.GetComponentsInChildren<ItemWrapper>();
        }

        private static Vector3 JsonArrayToVector3(FizzleJson array)
        {
            return new Vector3
            {
                x = array[0].ToObject<float>(),
                y = array[1].ToObject<float>(),
                z = array[2].ToObject<float>()
            };
        }
    }
}
