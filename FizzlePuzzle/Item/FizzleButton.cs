﻿using System.Collections.Generic;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleButton : InteractiveItem, ISwitch
    {
        [SerializeField] private bool m_DefaultActivate;
        [SerializeField] private List<AudioClip> m_PressSounds;
        private Transform button;
        private AudioSource audioSource;

        internal FizzleColor ActiveColor { get; private set; } = (FizzleColor) "#00FF00";

        internal FizzleColor DeactiveColor { get; private set; } = (FizzleColor) "#FF0000";

        public bool Activated { get; private set; }

        public event FizzleEvent active = () => { };

        public event FizzleEvent deactive = () => { };

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            transform.parent.Find("stock").SetColor((FizzleColor) "#666666");
            button = transform.parent.Find("button");
        }

        protected override void Start()
        {
            base.Start();
            if (!m_DefaultActivate)
            {
                SetDeactive();
            }
            else
            {
                SetActive();
            }
        }

        public override void Generate(FizzleJson data)
        {
            m_DefaultActivate = data.GetOrDefault("default-activate", m_DefaultActivate);
            string faceAt = data.GetOrDefault("face-at", "forward").ToLower();
            switch (faceAt)
            {
                case "left":
                    transform.parent.eulerAngles += 90.0F * Vector3.up;
                    break;
                case "back":
                    transform.parent.eulerAngles += 180.0F * Vector3.up;
                    break;
                case "right":
                    transform.parent.eulerAngles += 270.0F * Vector3.up;
                    break;
                default:
                    faceAt = "forward";
                    break;
            }
            ActiveColor = (FizzleColor) data.GetOrDefault("active-color", ActiveColor.ToString());
            DeactiveColor = (FizzleColor) data.GetOrDefault("deactive-color", DeactiveColor.ToString());
            FizzleDebug.Log($"FizzleButton name = {(object) data["name"] ?? name}, default-activate = {m_DefaultActivate}, face-at = {faceAt}, active-color = {ActiveColor}, deactive-color = {DeactiveColor}");
        }

        internal void SetActive()
        {
            CommonTools.PlayRandomSound(audioSource, m_PressSounds);
            Activated = true;
            active();
            button.SetColor(ActiveColor);
        }

        internal void SetDeactive()
        {
            CommonTools.PlayRandomSound(audioSource, m_PressSounds);
            Activated = false;
            deactive();
            button.SetColor(DeactiveColor);
        }

        internal void ToggleActive()
        {
            if (Activated)
            {
                SetDeactive();
            }
            else
            {
                SetActive();
            }
        }

        internal override void Interact(Transform player)
        {
            ToggleActive();
        }
    }
}
