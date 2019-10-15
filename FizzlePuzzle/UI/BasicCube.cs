using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using UnityEngine;

namespace FizzlePuzzle.UI
{
    internal class BasicCube : FizzleBehaviour
    {
        [SerializeField] protected Color m_Color = (FizzleColor) "#FFFFFF";
        
        [SerializeField] protected float m_Width = 1.0F;
        
        [SerializeField] protected float m_Height = 1.0F;
        
        [SerializeField] protected float m_Length = 1.0F;
        
        [SerializeField] protected float m_RepeatScaleX = 1.0F;
        
        [SerializeField] protected float m_RepeatScaleY = 1.0F;
        
        [SerializeField] [Range(0.0F, 3.0F)] protected int m_RepeatTarget;
        
        private Material material;

        protected override void Awake()
        {
            base.Awake();
            material = GetComponent<MeshRenderer>().material;
        }

        protected override void Start()
        {
            base.Start();
            UpdateData();
        }

        protected override void OnInspector()
        {
            base.OnInspector();
            material = GetComponent<MeshRenderer>().sharedMaterial;
            UpdateData();
        }

        internal void SetSize(float width, float height, float length)
        {
            m_Width = width;
            m_Height = height;
            m_Length = length;
        }

        protected void UpdateData()
        {
            transform.localScale = new Vector3(m_Width, m_Height, m_Length);
            transform.localPosition = new Vector3(0.0F, m_Height * 0.5F, 0.0F);
            material.color = m_Color;
            material.mainTexture.wrapMode = TextureWrapMode.Repeat;
            switch (m_RepeatTarget)
            {
                case 1:
                    material.mainTextureScale = new Vector2(transform.localScale.x / m_RepeatScaleX, transform.localScale.y / m_RepeatScaleY);
                    break;
                case 2:
                    material.mainTextureScale = new Vector2(transform.localScale.z / m_RepeatScaleX, transform.localScale.y / m_RepeatScaleY);
                    break;
                case 3:
                    material.mainTextureScale = new Vector2(transform.localScale.x / m_RepeatScaleX, transform.localScale.z / m_RepeatScaleY);
                    break;
                default:
                    return;
            }
        }
    }
}