using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleLogicCurtain : FizzleCurtain, ISwitch
    {
        [SerializeField] private bool m_DefaultActivate;
        [SerializeField] private List<AudioClip> m_ThroughSounds;
        private AudioSource audioSource;
        private RaycastHit lastHit;
        private RaycastHit enterHit;

        public bool Activated { get; private set; }

        internal FizzleColor ActiveColor { get; private set; } = (FizzleColor) "#00FF00";

        internal FizzleColor DeactiveColor { get; private set; } = (FizzleColor) "#FF0000";

        public event FizzleEvent active = () => { };

        public event FizzleEvent deactive = () => { };

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Start()
        {
            base.Start();
            glass.GetComponent<BoxCollider>().isTrigger = true;
            if (m_DefaultActivate)
            {
                SetActive();
            }
            else
            {
                SetInactive();
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientMultiplicationOrder")]
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (FizzleScene.TimeCtrl.Rewinding)
            {
                return;
            }

            int mask = FizzleLayerMask.GetMask("Player");
            RaycastHit hitInfo;
            bool flag = Physics.BoxCast(glass.transform.position - 0.5F * glass.transform.forward, transform.rotation * glass.transform.localScale * 0.5F, glass.transform.forward, out hitInfo, Quaternion.identity, 1.0F, mask);
            if (flag && this.StatusDetect("Curtain", true, true))
            {
                enterHit = hitInfo;
            }
            else if (!flag && this.StatusDetect("Curtain", false, false))
            {
                Vector3 vector31 = enterHit.point - glass.transform.position;
                Vector3 vector32 = lastHit.point - glass.transform.position;
                Vector3 right = glass.transform.right;
                if (Vector3.Cross(vector31, right).y * Vector3.Angle(vector31, right) * (Vector3.Cross(vector32, right).y * Vector3.Angle(vector32, right)) <= 0.0F)
                {
                    ToggleActive();
                }
            }

            lastHit = hitInfo;
        }

        internal void SetActive()
        {
            CommonTools.PlayRandomSound(audioSource, m_ThroughSounds);
            Activated = true;
            active();
            SetColor(ActiveColor);
        }

        internal void SetInactive()
        {
            CommonTools.PlayRandomSound(audioSource, m_ThroughSounds);
            Activated = false;
            deactive();
            SetColor(DeactiveColor);
        }

        internal void ToggleActive()
        {
            if (Activated)
            {
                SetInactive();
            }
            else
            {
                SetActive();
            }
        }

        public override void Generate(FizzleJson data)
        {
            base.Generate(data);
            m_DefaultActivate = data.GetOrDefault("default-activate", m_DefaultActivate);
            ActiveColor = (FizzleColor) data.GetOrDefault("active-color", ActiveColor.ToString());
            DeactiveColor = (FizzleColor) data.GetOrDefault("deactive-color", DeactiveColor.ToString());
            FizzleDebug.Log($"FizzleLogicCurtain name = {(object) data["name"] ?? name}, default-activate = {m_DefaultActivate}, active-color = {ActiveColor}, deactive-color = {DeactiveColor}");
        }
    }
}
