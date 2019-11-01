using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Characters;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.UI;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class FizzleElevator : FizzleBehaviour, FizzleItem, IAlignable
    {
        internal FizzleElevatorStatus status;
        
        [SerializeField] internal string Activator;
        [SerializeField] internal float Height;
        [SerializeField] private bool m_DefaultRaised;
        [SerializeField] private List<AudioClip> m_LoopSounds;
        [SerializeField] private List<AudioClip> m_StopSounds;
        
        private Transform @base;
        private Transform root;
        private SwitchResponse response;
        private float realHeight;
        private AudioSource audioSource;
        private FizzleElevatorStatus lastStatus;

        internal event FizzleEvent raised = () => { };
        internal event FizzleEvent dropped = () => { };
        internal event FizzleEvent raiseFinished = () => { };
        internal event FizzleEvent dropFinished = () => { };

        private static bool isStatic(FizzleElevatorStatus status)
        {
            return status == FizzleElevatorStatus.RAISED || status == FizzleElevatorStatus.DROPPED;
        }

        public void Generate(FizzleJson data)
        {
            Height = data.GetOrDefault("elevator-height", 1.0F);
            realHeight = Height * 2.0F;
            root.GetComponent<BasicCube>().SetSize(1.4F, realHeight, 1.4F);
            m_DefaultRaised = data.GetOrDefault("default-raised", false);
            Activator = data.GetOrDefault<string>("activator", null);
            FizzleDebug.Log($"FizzleElevator name = {(object) data["name"] ?? name}, activator = {Activator ?? "None"}, elevator-height = {Height}, default-raised = {m_DefaultRaised}");
        }

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            @base = transform.Find("base");
            root = transform.Find("root");
        }

        protected override void Update()
        {
            base.Update();
            response?.Test(() =>
            {
                if (!m_DefaultRaised)
                {
                    Raise();
                }
                else
                {
                    Drop();
                }
            }, () =>
            {
                if (!m_DefaultRaised)
                {
                    Drop();
                }
                else
                {
                    Raise();
                }
            });

            if (!isStatic(status))
            {
                PlayLoopSound();
            }

            if (!isStatic(lastStatus) && isStatic(status))
            {
                PlayStopSound();
            }

            lastStatus = status;
        }

        internal void Raise()
        {
            StopAllCoroutines();
            StartCoroutine(InternalRaise());
            raised();
        }

        internal void Drop()
        {
            StopAllCoroutines();
            dropped();
            StartCoroutine(InternalDrop());
        }

        private void PlayLoopSound()
        {
            if (!audioSource.isPlaying || !m_LoopSounds.Contains(audioSource.clip))
            {
                CommonTools.PlayRandomSound(audioSource, m_LoopSounds);
            }
        }

        private void PlayStopSound()
        {
            CommonTools.PlayRandomSound(audioSource, m_StopSounds);
        }

        protected override void Start()
        {
            base.Start();
            @base.localPosition = new Vector3(0.0F, realHeight + 0.1F, 0.0F);
            if (Activator != null)
            {
                response = new SwitchResponse(Activator);
            }
            if (m_DefaultRaised ^ (response?.Test() == true))
            {
                status = FizzleElevatorStatus.RAISED;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                status = FizzleElevatorStatus.DROPPED;
                transform.localPosition = -transform.up * realHeight;
            }
        }

        private IEnumerator InternalDrop()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (FizzleScene.TimeCtrl.Rewinding)
                {
                    continue;
                }
                if (transform.localPosition.y > -realHeight)
                {
                    status = FizzleElevatorStatus.DROPPING;
                    transform.Translate(-transform.up * Time.fixedDeltaTime);
                    RaycastHit hitInfo;
                    if (Physics.BoxCast(@base.transform.position, @base.transform.localScale * 0.4F, @base.transform.up, out hitInfo, Quaternion.identity, 0.4F, FizzleLayerMask.GetMask("Player")) && hitInfo.collider.gameObject.GetComponent<FirstPersonController>())
                    {
                        hitInfo.collider.transform.Translate(-transform.up * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    transform.localPosition = -transform.up * realHeight;
                    if (status != FizzleElevatorStatus.DROPPING)
                    {
                        continue;
                    }
                    status = FizzleElevatorStatus.DROPPED;
                    dropFinished();
                }
            }
        }

        private IEnumerator InternalRaise()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (FizzleScene.TimeCtrl.Rewinding)
                {
                    continue;
                }
                if (transform.localPosition.y < 0.0F)
                {
                    status = FizzleElevatorStatus.RAISING;
                    transform.Translate(transform.up * Time.fixedDeltaTime);
                    RaycastHit hitInfo;
                    if (Physics.BoxCast(@base.transform.position, @base.transform.localScale * 0.4F, @base.transform.up, out hitInfo, Quaternion.identity, 0.4F, FizzleLayerMask.GetMask("Player")) && hitInfo.collider.gameObject.GetComponent<FirstPersonController>())
                        hitInfo.collider.transform.Translate(transform.up * Time.fixedDeltaTime);
                }
                else
                {
                    transform.localPosition = Vector3.zero;
                    if (status != FizzleElevatorStatus.RAISING)
                    {
                        continue;
                    }
                    status = FizzleElevatorStatus.RAISED;
                    raiseFinished();
                }
            }
        }

        public float AlignHeight => 0.2F + (!m_DefaultRaised ? 0.0F : realHeight);

        public Transform AlignTransform => transform.parent;
    }

    internal enum FizzleElevatorStatus
    {
        RAISED,
        DROPPED,
        RAISING,
        DROPPING
    }
}
