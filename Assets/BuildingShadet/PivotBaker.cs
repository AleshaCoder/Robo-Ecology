#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class PivotBaker
{
    [MenuItem("CONTEXT/Transform/Bake Meshes With Pivots")]
    private static void BakeMeshesWithPivots(MenuCommand command)
    {
        var targetTransform = (Transform)command.context;

        var gameObjectName = targetTransform.gameObject.name;
        var path = EditorUtility.SaveFilePanelInProject(
            gameObjectName,
            gameObjectName,
            "asset", $"Save baked mesh for {gameObjectName}");

        if (string.IsNullOrEmpty(path)) return;

        var originalMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        var newMesh = Bake(targetTransform, originalMesh);

        if (ReferenceEquals(newMesh, originalMesh))
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            AssetDatabase.CreateAsset(newMesh, path);
        }
    }

    private static Mesh Bake(Transform root, Mesh originalMesh)
    {
        var meshFilters = root.GetComponentsInChildren<MeshFilter>(true);
        var vertexCount = 0;
        var indexCount = 0;
        foreach (var meshFilter in meshFilters)
        {
            var mesh = meshFilter.sharedMesh;
            vertexCount += mesh.vertexCount;
            indexCount += mesh.GetSubMesh(0).indexCount;
        }

        var vertices = new Vector3[vertexCount];
        var normals = new Vector3[vertexCount];
        var uv1 = new Vector2[vertexCount];
        var indices = new int[indexCount];
        var pivotUvs = new Vector4[vertexCount];

        var vertexStart = 0;
        var indexStart = 0;

        var tmpVertices = new List<Vector3>();
        var tmpNormals = new List<Vector3>();
        var tmpUv1 = new List<Vector2>();
        var tmpIndices = new List<int>();
        var rootMatrix = root.transform.worldToLocalMatrix;
        var delta = 1.0f / meshFilters.Length;
        var randomValue = -delta;
        for (var meshIndex = 0; meshIndex < meshFilters.Length; meshIndex++)
        {
            var meshFilter = meshFilters[meshIndex];
            var mesh = meshFilter.sharedMesh;

            mesh.GetVertices(tmpVertices);
            mesh.GetNormals(tmpNormals);
            mesh.GetUVs(0, tmpUv1);
            mesh.GetIndices(tmpIndices, 0);

            randomValue += delta;
            var transform = rootMatrix * meshFilter.transform.localToWorldMatrix;
            for (int i = 0; i < tmpVertices.Count; i++)
            {
                var index = vertexStart + i;
                var position = tmpVertices[i];
                vertices[index] = transform.MultiplyPoint3x4(position);
                normals[index] = transform.MultiplyVector(tmpNormals[i]);
                uv1[index] = tmpUv1[i];

                var originOffset = vertices[index] - transform.MultiplyPoint3x4(Vector3.zero);
                pivotUvs[vertexStart + i] =
                    new Vector4(originOffset.x, originOffset.y, originOffset.z, randomValue);
            }

            for (int i = 0; i < tmpIndices.Count; i++)
            {
                indices[indexStart + i] = tmpIndices[i] + vertexStart;
            }

            vertexStart += tmpVertices.Count;
            indexStart += tmpIndices.Count;
        }

        var newMesh = originalMesh == null ? new Mesh() : originalMesh;
        newMesh.Clear();

        newMesh.SetVertices(vertices);
        newMesh.SetNormals(normals);
        newMesh.SetUVs(0, uv1);
        newMesh.SetUVs(1, pivotUvs);
        newMesh.SetIndices(indices, MeshTopology.Triangles, 0);

        newMesh.RecalculateBounds();

        return newMesh;
    }
}

#endif