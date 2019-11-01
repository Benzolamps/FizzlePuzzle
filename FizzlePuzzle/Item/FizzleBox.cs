using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Characters;
using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.Utility;
using UnityEngine;

namespace FizzlePuzzle.Item
{
    internal class FizzleBox : InteractiveItem
    {
        internal Transform Carrier { get; private set; }
        internal event FizzleEvent carried = () => { };
        internal event FizzleEvent released = () => { };

        [SerializeField] private List<AudioClip> m_CarrySounds;
        [SerializeField] private List<AudioClip> m_ReleaseSounds;

        private string alignTo;
        private Rigidbody rigidbody;
        private BoxCollider boxCollider;
        private FizzleLayerMask layerMask;
        private float outRadius;
        private AudioSource audioSource;
        private IAlignable alignable;

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            rigidbody = GetComponent<Rigidbody>();
            boxCollider = GetComponent<BoxCollider>();
            layerMask = FizzleLayerMask.GetNotMask("Player", "Box", "Curtain", "Trigger");
            outRadius = transform.localScale.x * Mathf.Sqrt(2.0F) * 0.5F;
        }

        protected override void Update()
        {
            base.Update();
            if (!Carrier)
            {
                return;
            }

            Vector3 vector31 = Carrier.position + Carrier.forward * 2.0F;
            Vector3 position = Carrier.position;
            Vector3 direction = vector31 - Carrier.position;
            RaycastHit raycastHit;
            Vector3 vector32 = vector31 - Carrier.position;
            float num1 = vector32.magnitude + outRadius;
            int layerMask = this.layerMask;
            if (!Physics.Raycast(position, direction, out raycastHit, num1, layerMask))
            {
                transform.position = vector31 + Carrier.up * 0.1F;
            }
            else
            {
                Vector3 vector33 = raycastHit.point - Carrier.position;
                Vector3 normal = raycastHit.normal;
                float num2 = Vector3.Cross(vector33, normal).y * Vector3.Angle(vector33, normal);
                Vector3 normalized;
                if (num2 > 0.0F)
                {
                    vector32 = vector33 + Carrier.right;
                    normalized = vector32.normalized;
                }
                else
                {
                    vector32 = vector33 - Carrier.right;
                    normalized = vector32.normalized;
                }

                transform.position = Carrier.position + normalized + Carrier.up * 0.1F;
            }
        }

        internal void Carry(Transform player)
        {
            StopAllCoroutines();
            CommonTools.PlayRandomSound(audioSource, m_CarrySounds);
            rigidbody.isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = true;
            Carrier = player;
            Carrier.GetComponent<BaseCharacterAction>().Carry(this);
            carried();
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        internal void Release(Transform player)
        {
            StopAllCoroutines();
            StartCoroutine(PlayRelease());
            GetComponent<BoxCollider>().isTrigger = false;
            RaycastHit hitInfo;
            if (Physics.Raycast(Carrier.position, transform.position - Carrier.position, out hitInfo, (transform.position - Carrier.position).magnitude, layerMask))
            {
                transform.position = Carrier.position - Carrier.forward;
            }

            rigidbody.isKinematic = false;
            Carrier?.GetComponent<BaseCharacterAction>().Carry(null);
            Carrier = null;
            released();
        }

        private IEnumerator PlayRelease()
        {
            yield return new WaitForSeconds(0.5f);
            CommonTools.PlayRandomSound(audioSource, m_ReleaseSounds);
        }

        internal override void Interact(Transform player)
        {
            if (!player.GetComponent<BaseCharacterAction>().carryingObject)
            {
                if (Carrier)
                {
                    Release(Carrier);
                }

                Carry(player);
            }
            else
            {
                Release(player);
            }
        }

        public override void Generate(FizzleJson data)
        {
            transform.eulerAngles += Vector3.up * Random.Range(0.0F, 360.0F);
            alignTo = data.GetOrDefault("align-to", (string) null);
            FizzleDebug.Log($"FizzleBox name = {(object) data["name"] ?? name}, align-to = {alignTo ?? "None"}, rewindable = {"FizzleBoxRewind" != data["class"].ToString()}");
        }

        protected override void Start()
        {
            if (alignTo == null)
            {
                return;
            }
            alignable = FizzleScene.FindObject<IAlignable>(alignTo);
            transform.position = alignable.AlignTransform.position + alignable.AlignHeight * alignable.AlignTransform.up;
        }
    }
}
