using UnityEngine;
using UnityEditor;

namespace Spektr
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LightningRenderer))]
    class LightningRendererEditor : Editor
    {
        SerializedProperty _mesh;

        SerializedProperty _throttle;

        SerializedProperty _emitterTransform;
        SerializedProperty _emitterPosition;
        SerializedProperty _receiverTransform;
        SerializedProperty _receiverPosition;

        SerializedProperty _pulseInterval;
        SerializedProperty _boltLength;
        SerializedProperty _lengthRandomness;

        SerializedProperty _noiseAmplitude;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;

        SerializedProperty _color;

        static GUIContent _textEmpty = new GUIContent();
        static GUIContent _textEmitter = new GUIContent("Emitter");
        static GUIContent _textReceiver = new GUIContent("Receiver");

        void OnEnable()
        {
            _mesh = serializedObject.FindProperty("_mesh");

            _throttle = serializedObject.FindProperty("_throttle");

            _emitterTransform = serializedObject.FindProperty("_emitterTransform");
            _emitterPosition = serializedObject.FindProperty("_emitterPosition");
            _receiverTransform = serializedObject.FindProperty("_receiverTransform");
            _receiverPosition = serializedObject.FindProperty("_receiverPosition");

            _pulseInterval = serializedObject.FindProperty("_pulseInterval");
            _boltLength = serializedObject.FindProperty("_boltLength");
            _lengthRandomness = serializedObject.FindProperty("_lengthRandomness");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion = serializedObject.FindProperty("_noiseMotion");

            _color = serializedObject.FindProperty("_color");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            EditorGUILayout.PropertyField(_mesh);
            EditorGUILayout.PropertyField(_throttle);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_emitterTransform, _textEmitter);
            if (_emitterTransform.hasMultipleDifferentValues ||
                _emitterTransform.objectReferenceValue == null)
                EditorGUILayout.PropertyField(_emitterPosition, _textEmpty);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_receiverTransform, _textReceiver);
            if (_receiverTransform.hasMultipleDifferentValues ||
                _receiverTransform.objectReferenceValue == null)
                EditorGUILayout.PropertyField(_receiverPosition, _textEmpty);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_pulseInterval);
            EditorGUILayout.PropertyField(_boltLength);
            EditorGUILayout.PropertyField(_lengthRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_noiseAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency);
            EditorGUILayout.PropertyField(_noiseMotion);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_color);

            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            var instance = (LightningRenderer)target;

            EditorGUI.BeginChangeCheck();

            var p0 = instance.emitterPosition;
            var p1 = instance.receiverPosition;

            if (instance.emitterTransform == null)
                p0 = Handles.PositionHandle(p0, Quaternion.identity);

            if (instance.receiverTransform == null)
                p1 = Handles.PositionHandle(p1, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Position");

                instance.emitterPosition = p0;
                instance.receiverPosition = p1;

                EditorUtility.SetDirty(target);
            }
        }
    }
}
