using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using UnityEngine;

namespace FizzlePuzzle.Scene
{
    internal class CubeTexture : FizzleBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mesh.subMeshCount = 6;
            int[] triangles = mesh.triangles;
            mesh.SetTriangles(triangles.Subarray(0, 5), 0);
            mesh.SetTriangles(triangles.Subarray(6, 11), 1);
            mesh.SetTriangles(triangles.Subarray(12, 17), 2);
            mesh.SetTriangles(triangles.Subarray(18, 23), 3);
            mesh.SetTriangles(triangles.Subarray(24, 29), 4);
            mesh.SetTriangles(triangles.Subarray(30, 35), 5);
        }
    }
}