﻿using FizzlePuzzle.Characters;
using FizzlePuzzle.UI;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    [CreateAssetMenu(fileName = "New Fizzle Defination", menuName = "Fizzle Defination")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal class FizzleDefination : ScriptableObject
    {
        [SerializeField] internal FizzleView m_FizzleView;
        [SerializeField] internal FirstPersonController m_PlayerPrefab;
        [SerializeField] internal Light m_LightPrefab;
        [SerializeField] internal ForkCharacterController m_ForkPrefab;
        [SerializeField] internal FizzleLevelEnd m_LevelEnd;
        [SerializeField] internal ItemMapping m_GeneratableItemPrefabs;
        [SerializeField] internal List<LightSetting> m_LightSettings;
        [SerializeField] internal string m_SubtitlePath;
        [SerializeField] internal SceneInfo[] m_OfficalSceneInfos;
        [SerializeField] internal string[] m_DefaultScriptPaths;
    }
}
