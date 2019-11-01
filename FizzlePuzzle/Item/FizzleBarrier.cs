﻿using System.Collections;
using System.Collections.Generic;
 using System.Diagnostics.CodeAnalysis;
 using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleBarrier : FizzleCurtain
    {
        [SerializeField] [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal string Activator;
        
        private SwitchResponse response;
        
        [SerializeField] private List<AudioClip> m_OpenSounds;
        [SerializeField] private List<AudioClip> m_CloseSounds;
        
        private AudioSource audioSource;

        internal FizzleColor Color { get; private set; } = (FizzleColor) "#0000FF";

        internal bool Opening { get; private set; }

        internal event FizzleEvent opened = () => { };

        internal event FizzleEvent closed = () => { };

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            response?.Test(() =>
            {
                if (!Opening)
                {
                    Open();
                }
            }, () =>
            {
                if (Opening)
                {
                    Close();
                }
            });
        }

        internal void Open()
        {
            StopAllCoroutines();
            opened();
            CommonTools.PlayRandomSound(audioSource, m_OpenSounds);
            Opening = true;
            StartCoroutine(InternalOpen());
        }

        public void Close()
        {
            StopAllCoroutines();
            closed();
            CommonTools.PlayRandomSound(audioSource, m_CloseSounds);
            Opening = false;
            StartCoroutine(InternalClose());
        }

        private IEnumerator InternalOpen()
        {
            FizzleBarrier fizzleBarrier = this;
            while (fizzleBarrier.glass.transform.localPosition.y > -0.5F * fizzleBarrier.glass.transform.localScale.y)
            {
                fizzleBarrier.glass.transform.localPosition -= 0.5F * fizzleBarrier.glass.transform.up;
                yield return new WaitForFixedUpdate();
            }

            fizzleBarrier.glass.transform.localPosition = -0.5f * fizzleBarrier.glass.transform.localScale.y * fizzleBarrier.glass.transform.up;
            fizzleBarrier.glass.gameObject.SetActive(false);
        }

        private IEnumerator InternalClose()
        {
            FizzleBarrier fizzleBarrier = this;
            fizzleBarrier.glass.gameObject.SetActive(true);
            while (fizzleBarrier.glass.transform.localPosition.y < 0.5F * fizzleBarrier.glass.transform.localScale.y)
            {
                fizzleBarrier.glass.transform.localPosition += 0.5F * fizzleBarrier.glass.transform.up;
                yield return new WaitForFixedUpdate();
            }

            fizzleBarrier.glass.transform.localPosition = 0.5F * fizzleBarrier.glass.transform.localScale.y * fizzleBarrier.glass.transform.up;
        }

        public override void Generate(FizzleJson data)
        {
            base.Generate(data);
            Activator = data.GetOrDefault<string>("activator", null);
            Color = (FizzleColor) data.GetOrDefault("color", Color.ToString());
            FizzleDebug.Log($"FizzleBarrier name = {(object) data["name"] ?? name}, color = {Color}, activator = {Activator ?? "None"}");
        }

        protected override void Start()
        {
            base.Start();
            if (Activator != null)
            {
                response = new SwitchResponse(Activator);
            }

            SetColor(Color);
        }
    }
}
