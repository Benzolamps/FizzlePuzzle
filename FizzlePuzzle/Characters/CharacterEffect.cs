using System.Collections;
using System.Collections.Generic;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    [RequireComponent(typeof(FirstPersonController))]
    internal class CharacterEffect : FizzleBehaviour
    {
        private bool lastGrounded = true;
        [SerializeField] private List<AudioClip> m_RunSounds;
        [SerializeField] private List<AudioClip> m_JumpSounds;
        [SerializeField] private List<AudioClip> m_FallSounds;
        private float lastVelocity;
        private AudioSource audioSource;

        private Animator Animator => GetComponentInChildren<Animator>();

        private CharacterController Controller => GetComponent<CharacterController>();

        internal bool Grounded
        {
            set
            {
                Animator.SetBool(nameof(Grounded), value);
                if (FizzleScene.TimeCtrl.Rewinding)
                {
                    return;
                }

                if (!lastGrounded & value)
                {
                    CommonTools.PlayRandomSound(audioSource, m_FallSounds);
                }

                if (lastGrounded && !value)
                {
                    CommonTools.PlayRandomSound(audioSource, m_JumpSounds);
                }

                lastGrounded = value;
            }
            get { return Controller.isGrounded; }
        }

        internal Vector3 Velocity
        {
            set
            {
                float x = value.x;
                float y = value.y;
                float z = value.z;
                Animator.SetFloat("Horizontal Velocity", y);
                Animator.SetFloat("Vertical Velocity", x * x + z * z);
                if (FizzleScene.TimeCtrl.Rewinding)
                {
                    StopAllCoroutines();
                }
                else
                {
                    float num1 = x * x + z * z;
                    int num2 = !Grounded ? 0 : ((double) lastVelocity > 10.0F ? 1 : 0);
                    bool flag = Grounded && num1 > 10.0F;
                    if (num2 == 0 & flag)
                    {
                        StopAllCoroutines();
                        StartCoroutine(PlayWalkOrRunSound(m_RunSounds));
                    }

                    if (num2 != 0 && !flag || !Grounded)
                    {
                        StopAllCoroutines();
                    }

                    lastVelocity = num1;
                }
            }
            get
            {
                return Controller.velocity;
            }
        }

        private IEnumerator PlayWalkOrRunSound(List<AudioClip> clips)
        {
            while (true)
            {
                if (!audioSource.isPlaying)
                {
                    CommonTools.PlayRandomSound(audioSource, clips);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Velocity = Velocity;
            Grounded = Grounded;
        }

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            Grounded = true;
        }

        internal void Pause()
        {
            Animator.speed = 0.0F;
        }

        internal void Continue()
        {
            Animator.speed = 1.0F;
        }
    }
}
