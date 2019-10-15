using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal struct LightSetting
    {
        [SerializeField] internal Material Skybox;
        [SerializeField] internal Color AmbientLightColor;
        [SerializeField] internal float AmbientLightIntensity;
        [SerializeField] internal Color DirectionalLightColor;
        [SerializeField] internal float DirectionalLightIntensity;
    }
}