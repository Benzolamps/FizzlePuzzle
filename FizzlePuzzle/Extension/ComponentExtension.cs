using UnityEngine;

namespace FizzlePuzzle.Extension
{
    internal static class ComponentExtension
    {
        internal static void SetColor(this Component component, FizzleColor color)
        {
            MeshRenderer component1 = component.GetComponent<MeshRenderer>();
            if (!component1)
            {
                throw new FizzleException("物体中必须包含MeshRender组件");
            }
            component1.material.color = color;
        }
    }
}
