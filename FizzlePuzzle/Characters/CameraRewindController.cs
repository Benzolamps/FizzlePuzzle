using FizzlePuzzle.TimeEffect;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace FizzlePuzzle.Characters
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(PostProcessingBehaviour))]
    internal class CameraRewindController : RewindController
    {
        [SerializeField] private PostProcessingProfile m_ProfileNormal;
        [SerializeField] private PostProcessingProfile m_ProfileRewind;
        private PostProcessingBehaviour effect;

        protected override void Awake()
        {
            base.Awake();
            effect = GetComponent<PostProcessingBehaviour>();
        }

        public override void BeginRecording()
        {
            base.BeginRecording();
            effect.profile = m_ProfileNormal;
        }

        public override void BeginRewinding()
        {
            base.BeginRewinding();
            effect.profile = m_ProfileRewind;
        }

        protected override void PopFromStack(IRewindStatus obj)
        {
            CameraRewindStatus cameraRewindStatus = (CameraRewindStatus) obj;
            transform.localPosition = cameraRewindStatus.position;
            transform.localRotation = cameraRewindStatus.rotation;
        }

        protected override IRewindStatus PushToStack()
        {
            return new CameraRewindStatus
            {
                position = transform.localPosition,
                rotation = transform.localRotation
            };
        }
    }
}
