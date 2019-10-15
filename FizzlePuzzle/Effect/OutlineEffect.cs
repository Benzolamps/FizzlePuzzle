using FizzlePuzzle.Core;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
using Debug = System.Diagnostics.Debug;

namespace FizzlePuzzle.Effect
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    internal class OutlineEffect : FizzleBehaviour
    {
        private readonly IList<Outline> outlines = new List<Outline>();
        [Range(1.0F, 6.0F)] public float lineThickness = 1.25F;
        [Range(0.0F, 10.0F)] public float lineIntensity = 0.5F;
        [Range(0.0F, 1.0F)] public float fillAmount = 0.2F;
        public Color lineColor0 = Color.red;
        public Color lineColor1 = Color.green;
        public Color lineColor2 = Color.blue;
        public bool backfaceCulling = true;
        [Header("Advanced settings")] public bool scaleWithScreenSize = true;
        [Range(0.1F, 0.9F)] public float alphaCutoff = 0.5F;
        private readonly List<Material> materialBuffer = new List<Material>();
        private static OutlineEffect m_instance;
        public bool additiveRendering;

        [Header("These settings can affect performance!")]
        public bool cornerOutlines;

        public bool addLinesBetweenColors;
        public bool flipY;
        public Camera sourceCamera;
        [HideInInspector] public Camera outlineCamera;
        private Material outline1Material;
        private Material outline2Material;
        private Material outline3Material;
        private Material outlineEraseMaterial;
        public Shader outlineShader;
        public Shader outlineBufferShader;
        [HideInInspector] public Material outlineShaderMaterial;
        [HideInInspector] public RenderTexture renderTexture;
        [HideInInspector] public RenderTexture extraRenderTexture;
        private CommandBuffer commandBuffer;

        public static OutlineEffect Instance
        {
            get
            {
                if (Equals(m_instance, null))
                {
                    return m_instance = FindObjectOfType(typeof(OutlineEffect)) as OutlineEffect;
                }
                return m_instance;
            }
        }

        private OutlineEffect()
        {
        }

        private Material CreateMaterial(Color emissionColor)
        {
            Material material = new Material(outlineBufferShader);
            material.SetColor("_Color", emissionColor);
            material.SetInt("_SrcBlend", 5);
            material.SetInt("_DstBlend", 10);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            return material;
        }

        private Material GetMaterialFromId(int id)
        {
            switch (id)
            {
                case 0:
                    return outline1Material;
                case 1:
                    return outline2Material;
                default:
                    return outline3Material;
            }
        }

        protected override void Awake()
        {
            m_instance = this;
        }

        protected override void Start()
        {
            CreateMaterialsIfNeeded();
            UpdateMaterialsPublicProperties();
            if (sourceCamera == null)
            {
                sourceCamera = GetComponent<Camera>();
                if (sourceCamera == null)
                {
                    sourceCamera = Camera.main;
                }
            }
            Debug.Assert(sourceCamera != null, nameof(sourceCamera) + " != null");
            if (outlineCamera == null)
            {
                outlineCamera = new GameObject("Outline Camera")
                {
                    transform =
                    {
                        parent = sourceCamera.transform
                    }
                }.AddComponent<Camera>();
                outlineCamera.enabled = false;
            }

            renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            UpdateOutlineCameraFromSource();
            commandBuffer = new CommandBuffer();
            outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, commandBuffer);
        }

        private void OnPreRender()
        {
            if (commandBuffer == null)
            {
                return;
            }
            CreateMaterialsIfNeeded();
            if (renderTexture == null || renderTexture.width != sourceCamera.pixelWidth || renderTexture.height != sourceCamera.pixelHeight)
            {
                renderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                extraRenderTexture = new RenderTexture(sourceCamera.pixelWidth, sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                outlineCamera.targetTexture = renderTexture;
            }

            UpdateMaterialsPublicProperties();
            UpdateOutlineCameraFromSource();
            outlineCamera.targetTexture = renderTexture;
            commandBuffer.SetRenderTarget((RenderTargetIdentifier) renderTexture);
            commandBuffer.Clear();
            if (outlines != null)
            {
                foreach (Outline outline in outlines)
                {
                    LayerMask cullingMask = sourceCamera.cullingMask;
                    if (outline != null && cullingMask == (cullingMask | 1 << outline.originalLayer))
                    {
                        foreach (var material in outline.Renderer.sharedMaterials)
                        {
                            Material material1 = null;
                            if (material.mainTexture != null && material)
                            {
                                foreach (var material2 in materialBuffer.Where(material2 => material2.mainTexture == material.mainTexture))
                                {
                                    if (outline.eraseRenderer && material2.color == outlineEraseMaterial.color)
                                    {
                                        material1 = material2;
                                    }
                                    else if (material2.color == GetMaterialFromId(outline.color).color)
                                    {
                                        material1 = material2;
                                    }
                                }

                                if (material1 == null)
                                {
                                    material1 = !outline.eraseRenderer ? new Material(GetMaterialFromId(outline.color)) : new Material(outlineEraseMaterial);
                                    material1.mainTexture = material.mainTexture;
                                    materialBuffer.Add(material1);
                                }
                            }
                            else
                            {
                                material1 = !outline.eraseRenderer ? GetMaterialFromId(outline.color) : outlineEraseMaterial;
                            }

                            material1.SetInt("_Culling", backfaceCulling ? 2 : 0);
                            commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), material1, 0, 0);
                            MeshFilter component1 = outline.GetComponent<MeshFilter>();
                            if (component1)
                            {
                                for (int submeshIndex = 1; submeshIndex < component1.sharedMesh.subMeshCount; ++submeshIndex)
                                {
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), material1, submeshIndex, 0);
                                }
                            }

                            SkinnedMeshRenderer component2 = outline.GetComponent<SkinnedMeshRenderer>();
                            if (component2)
                            {
                                for (int submeshIndex = 1; submeshIndex < component2.sharedMesh.subMeshCount; ++submeshIndex)
                                {
                                    commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), material1, submeshIndex, 0);
                                }
                            }
                        }
                    }
                }
            }

            outlineCamera.Render();
        }

        protected override void OnEnable()
        {
            foreach (Outline outline in FindObjectsOfType<Outline>())
            {
                outline.enabled = false;
                outline.enabled = true;
            }
        }

        protected override void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
            }
            if (extraRenderTexture != null)
            {
                extraRenderTexture.Release();
            }
            DestroyMaterials();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            outlineShaderMaterial.SetTexture("_OutlineSource", renderTexture);
            if (addLinesBetweenColors)
            {
                Graphics.Blit(source, extraRenderTexture, outlineShaderMaterial, 0);
                outlineShaderMaterial.SetTexture("_OutlineSource", extraRenderTexture);
            }

            Graphics.Blit(source, destination, outlineShaderMaterial, 1);
        }

        private void CreateMaterialsIfNeeded()
        {
            if (outlineShaderMaterial == null)
            {
                outlineShaderMaterial = new Material(outlineShader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                UpdateMaterialsPublicProperties();
            }

            if (outlineEraseMaterial == null)
            {
                outlineEraseMaterial = CreateMaterial(new Color(0.0F, 0.0F, 0.0F, 0.0F));
            }
            if (outline1Material == null)
            {
                outline1Material = CreateMaterial(new Color(1.0F, 0.0F, 0.0F, 0.0F));
            }
            if (outline2Material == null)
            {
                outline2Material = CreateMaterial(new Color(0.0F, 1.0F, 0.0F, 0.0F));
            }
            if (outline3Material == null)
            {
                outline3Material = CreateMaterial(new Color(0.0F, 0.0F, 1.0F, 0.0F));
            }
        }

        private void DestroyMaterials()
        {
            foreach (Material material in materialBuffer)
            {
                DestroyImmediate(material);
            }
            materialBuffer.Clear();
            DestroyImmediate(outlineShaderMaterial);
            DestroyImmediate(outlineEraseMaterial);
            DestroyImmediate(outline1Material);
            DestroyImmediate(outline2Material);
            DestroyImmediate(outline3Material);
            outlineShader = null;
            outlineBufferShader = null;
            outlineShaderMaterial = null;
            outlineEraseMaterial = null;
            outline1Material = null;
            outline2Material = null;
            outline3Material = null;
        }

        private void UpdateMaterialsPublicProperties()
        {
            if (!outlineShaderMaterial)
            {
                return;
            }
            float num = 1.0F;
            if (scaleWithScreenSize)
            {
                num = Screen.height / 360.0F;
            }
            if (scaleWithScreenSize && num < 1.0F)
            {
                if (XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", 1.0F / 1000.0F * (1.0F / XRSettings.eyeTextureWidth) * 1000.0F);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", 1.0F / 1000.0F * (1.0F / XRSettings.eyeTextureHeight) * 1000.0F);
                }
                else
                {
                    outlineShaderMaterial.SetFloat("_LineThicknessX", 1.0F / 1000.0F * (1.0F / Screen.width) * 1000.0F);
                    outlineShaderMaterial.SetFloat("_LineThicknessY", 1.0F / 1000.0F * (1.0F / Screen.height) * 1000.0F);
                }
            }
            else if (XRSettings.isDeviceActive && sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
            {
                outlineShaderMaterial.SetFloat("_LineThicknessX", num * (lineThickness / 1000.0F) * (1.0F / XRSettings.eyeTextureWidth) * 1000.0F);
                outlineShaderMaterial.SetFloat("_LineThicknessY", num * (lineThickness / 1000.0F) * (1.0F / XRSettings.eyeTextureHeight) * 1000.0F);
            }
            else
            {
                outlineShaderMaterial.SetFloat("_LineThicknessX", num * (lineThickness / 1000.0F) * (1.0F / Screen.width) * 1000.0F);
                outlineShaderMaterial.SetFloat("_LineThicknessY", num * (lineThickness / 1000.0F) * (1.0F / Screen.height) * 1000.0F);
            }

            outlineShaderMaterial.SetFloat("_LineIntensity", lineIntensity);
            outlineShaderMaterial.SetFloat("_FillAmount", fillAmount);
            outlineShaderMaterial.SetColor("_LineColor1", lineColor0 * lineColor0);
            outlineShaderMaterial.SetColor("_LineColor2", lineColor1 * lineColor1);
            outlineShaderMaterial.SetColor("_LineColor3", lineColor2 * lineColor2);
            outlineShaderMaterial.SetInt("_FlipY", flipY ? 1 : 0);
            outlineShaderMaterial.SetInt("_Dark", !additiveRendering ? 1 : 0);
            outlineShaderMaterial.SetInt("_CornerOutlines", cornerOutlines ? 1 : 0);
            Shader.SetGlobalFloat("_OutlineAlphaCutoff", alphaCutoff);
        }

        private void UpdateOutlineCameraFromSource()
        {
            outlineCamera.CopyFrom(sourceCamera);
            outlineCamera.renderingPath = RenderingPath.Forward;
            outlineCamera.backgroundColor = new Color(0.0F, 0.0F, 0.0F, 0.0F);
            outlineCamera.clearFlags = CameraClearFlags.Color;
            outlineCamera.rect = new Rect(0.0F, 0.0F, 1.0F, 1.0F);
            outlineCamera.cullingMask = 0;
            outlineCamera.targetTexture = renderTexture;
            outlineCamera.enabled = false;
            outlineCamera.allowHDR = false;
        }

        internal void AddOutline(Outline outline)
        {
            if (!outlines.Contains(outline))
            {
                outlines.Add(outline);
            }
        }

        internal void RemoveOutline(Outline outline)
        {
            if (outlines.Contains(outline))
            {
                outlines.Remove(outline);
            }
        }
    }
}