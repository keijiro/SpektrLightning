//
// Spektr/Lightning - lightning bolt effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Spektr
{
    [CustomEditor(typeof(LightningMesh))]
    public class LightningMeshEditor : Editor
    {
        SerializedProperty _lineCount;
        SerializedProperty _vertexCount;

        void OnEnable()
        {
            _lineCount = serializedObject.FindProperty("_lineCount");
            _vertexCount = serializedObject.FindProperty("_vertexCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_lineCount);
            EditorGUILayout.PropertyField(_vertexCount);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (rebuild)
                foreach (var t in targets)
                    ((LightningMesh)t).RebuildMesh();
        }

        [MenuItem("Assets/Create/LightningMesh")]
        public static void CreateLightningMeshAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/LightningMesh.asset");

            // Create an asset.
            var asset = ScriptableObject.CreateInstance<LightningMesh>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.sharedMesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
