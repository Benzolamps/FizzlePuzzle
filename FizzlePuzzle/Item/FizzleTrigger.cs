using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Characters;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleTrigger : FizzleBehaviour, FizzleItem, ISwitch
    {
        internal enum TriggerType
        {
            ONCE,
            ALWAYS
        }

        [SerializeField] [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal TriggerType _TriggerType;

        private bool needRecharge;

        public bool Activated { get; private set; }

        public event FizzleEvent active = () => { };

        public event FizzleEvent deactive = () => { };

        private static FizzleCharacterController Player => FizzleScene.FirstPersonCtrl;

        protected override void Start()
        {
            base.Start();
            DestroyImmediate(GetComponent<MeshRenderer>());
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (!(other.gameObject == Player.gameObject))
            {
                return;
            }

            active();
            Activated = true;
        }

        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (!(other.gameObject == Player.gameObject))
            {
                return;
            }

            deactive();
            Activated = false;
            if (_TriggerType == TriggerType.ONCE && !needRecharge)
            {
                GetComponent<BoxCollider>().enabled = false;
            }

            needRecharge = false;
        }

        public void Generate(FizzleJson data)
        {
            _TriggerType = data.GetOrDefault("trigger-type", "once") == "always" ? TriggerType.ALWAYS : TriggerType.ONCE;
            FizzleDebug.Log($"FizzleTrigger name = {(object) data["name"] ?? name}, type = {_TriggerType.ToString()}");
        }

        internal void Recharge()
        {
            GetComponent<BoxCollider>().enabled = needRecharge = true;
        }
    }
}