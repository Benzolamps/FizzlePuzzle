﻿using System.Collections;
using System.Collections.Generic;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class PressurePlate : FizzleBehaviour, FizzleItem, ISwitch, IAlignable
    {
        internal FizzleColor ActiveColor { get; private set; } = (FizzleColor)"#00FF00";
        internal FizzleColor DeactiveColor { get; private set; } = (FizzleColor)"#FF0000";
        public bool Activated => activated ?? false;
        public event FizzleEvent active = () => { };
        public event FizzleEvent deactive = () => { };
        
        [SerializeField] private List<AudioClip> m_ActiveSounds;
        [SerializeField] private List<AudioClip> m_DeactiveSounds;
        
        private bool? activated;
        private GameObject button;
        private Material material;
        private AudioSource audioSource;
        
        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            button = transform.Find("button").gameObject;
            material = button.GetComponent<MeshRenderer>().material;
        }

        protected override void Update()
        {
            base.Update();
            if (Physics.BoxCast(button.transform.position, button.transform.localScale * 0.4F, button.transform.up, Quaternion.identity, 0.4F, FizzleLayerMask.GetMask("Player", "Box")))
            {
                if (activated == true)
                {
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(Active());
                activated = true;
            }
            else
            {
                if (activated == false)
                {
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(Deactive());
                activated = false;
            }
        }

        private IEnumerator Active()
        {
            yield return new WaitForSeconds(0.2F);
            CommonTools.PlayRandomSound(audioSource, m_ActiveSounds);
            button.transform.localPosition = transform.up * 0.12F;
            material.color = ActiveColor;
            active();
        }

        private IEnumerator Deactive()
        {
            yield return new WaitForSeconds(0.2F);
            CommonTools.PlayRandomSound(audioSource, m_DeactiveSounds);
            button.transform.localPosition = transform.up * 0.2F;
            material.color = DeactiveColor;
            deactive();
        }

        public void Generate(FizzleJson data)
        {
            ActiveColor = (FizzleColor) data.GetOrDefault("active-color", ActiveColor.ToString());
            DeactiveColor = (FizzleColor) data.GetOrDefault("deactive-color", DeactiveColor.ToString());
            FizzleDebug.Log($"PressurePlate name = {(object) data["name"] ?? name}, active-color = {ActiveColor}, deactive-color = {DeactiveColor}");
        }

        public float AlignHeight => 0.25F;

        public Transform AlignTransform => transform;
    }
}
