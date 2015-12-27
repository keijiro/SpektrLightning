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

            // Create an NoiseBballMesh asset.
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
