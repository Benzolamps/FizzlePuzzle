using System.Collections;
using System.Collections.Generic;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.TimeEffect;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(FirstPersonController))]
    [RequireComponent(typeof(CharacterEffect))]
    internal class FirstPersonRewindController : ForkController
    {
        private FirstPersonCharacterAction action;
        private CharacterEffect anim;
        private CharacterController charCtrl;
        private FirstPersonController fpCtrl;
        private GameObject forkChar;
        [SerializeField] private List<AudioClip> m_BeginForkSounds;
        [SerializeField] private List<AudioClip> m_EndForkSounds;
        [SerializeField] private List<AudioClip> m_RewindingSounds;
        private AudioSource audioSource;

        protected override void Awake()
        {
            base.Awake();
            audioSource = Spawn<AudioSource>(transform, "audio rewind");
            anim = GetComponent<CharacterEffect>();
            charCtrl = GetComponent<CharacterController>();
            fpCtrl = GetComponent<FirstPersonController>();
            action = GetComponent<FirstPersonCharacterAction>();
            forkChar = FizzleScene.ForkCharAction.gameObject;
        }

        protected override void Start()
        {
            base.Start();
            forkChar.SetActive(false);
        }

        public override void BeginRewinding()
        {
            base.BeginRewinding();
            fpCtrl.enabled = false;
            charCtrl.detectCollisions = false;
            forkChar.SetActive(false);
            StartCoroutine(PlayRewinding());
        }

        private IEnumerator PlayRewinding()
        {
            while (true)
            {
                if (!audioSource.isPlaying)
                    CommonTools.PlayRandomSound(audioSource, m_RewindingSounds);
                yield return new WaitForFixedUpdate();
            }
        }

        public override void EndRewinding()
        {
            base.EndRewinding();
            anim.Continue();
            fpCtrl.enabled = true;
            charCtrl.detectCollisions = true;
            StopAllCoroutines();
            audioSource.Stop();
        }

        public override void BeginForking()
        {
            base.BeginForking();
            CommonTools.PlayRandomSound(audioSource, m_BeginForkSounds);
            forkChar.SetActive(true);
            forkChar.GetComponentInChildren<ForkEffect>().Continue();
        }

        public override void EndForking()
        {
            base.EndForking();
            CommonTools.PlayRandomSound(audioSource, m_EndForkSounds);
            forkChar.GetComponent<ForkCharacterAction>().ReleaseAll();
            forkChar.SetActive(false);
        }

        protected override void ForkStatus(IRewindStatus obj)
        {
            FirstPersonRewindStatus personRewindStatus = (FirstPersonRewindStatus) obj;
            ForkEffect componentInChildren = forkChar.GetComponentInChildren<ForkEffect>();
            ForkCharacterAction component = forkChar.GetComponent<ForkCharacterAction>();
            componentInChildren.Grounded = personRewindStatus.grounded;
            componentInChildren.Velocity = personRewindStatus.animationVelocity;
            forkChar.transform.position = personRewindStatus.position;
            forkChar.transform.rotation = personRewindStatus.rotation;
            component.currentInteractiveItem = personRewindStatus.currentItem;
            component.IsCarrying = personRewindStatus.isCarrying;
            component.CameraRay = personRewindStatus.cameraRay;
        }

        public override void TimeOutRewinding()
        {
            base.TimeOutRewinding();
            anim.Pause();
        }

        protected override void PopFromStack(IRewindStatus obj)
        {
            anim.Continue();
            FirstPersonRewindStatus personRewindStatus = obj.Cast<FirstPersonRewindStatus>();
            transform.localPosition = personRewindStatus.position;
            transform.localRotation = personRewindStatus.rotation;
            anim.Velocity = personRewindStatus.animationVelocity;
            anim.Grounded = personRewindStatus.grounded;
        }

        protected override IRewindStatus PushToStack()
        {
            return new FirstPersonRewindStatus
            {
                position = transform.localPosition,
                rotation = transform.localRotation,
                animationVelocity = anim.Velocity,
                grounded = anim.Grounded,
                cameraRay = action.CameraRay,
                currentItem = action.currentInteractiveItem,
                isCarrying = action.carryingObject
            };
        }

        protected override void OnForkStopAction()
        {
            base.OnForkStopAction();
            ForkEffect componentInChildren = forkChar.GetComponentInChildren<ForkEffect>();
            componentInChildren.Velocity = Vector3.zero;
            componentInChildren.Grounded = true;
        }

        public override void PauseRewinding()
        {
            base.PauseRewinding();
            anim.Pause();
        }
    }
}
