using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FizzlePuzzle.Core;
using UnityEngine;

namespace FizzlePuzzle.Effect
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class Outline : FizzleBehaviour
    {
        public int color;
        public bool eraseRenderer;
        [HideInInspector] public int originalLayer;
        [HideInInspector] public Material[] originalMaterials;
        public Renderer Renderer { get; private set; }

        protected override void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        protected override void OnEnable()
        {
            foreach (OutlineEffect outlineEffect in Camera.allCameras.AsEnumerable().Select(c => c.GetComponent<OutlineEffect>()).Where(e => e != null))
            {
                outlineEffect.AddOutline(this);
            }
        }

        protected override void OnDisable()
        {
            foreach (OutlineEffect outlineEffect in Camera.allCameras.AsEnumerable().Select(c => c.GetComponent<OutlineEffect>()).Where(e => e != null))
            {
                outlineEffect.RemoveOutline(this);
            }
        }
    }
}
