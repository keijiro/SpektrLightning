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

namespace Spektr
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LightningRenderer))]
    class LightningRendererEditor : Editor
    {
        SerializedProperty _emitterTransform;
        SerializedProperty _emitterPosition;
        SerializedProperty _receiverTransform;
        SerializedProperty _receiverPosition;

        SerializedProperty _throttle;
        SerializedProperty _pulseInterval;
        SerializedProperty _boltLength;
        SerializedProperty _lengthRandomness;

        SerializedProperty _noiseAmplitude;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseMotion;

        SerializedProperty _color;
        SerializedProperty _mesh;
        SerializedProperty _randomSeed;

        static GUIContent _textEmpty = new GUIContent();
        static GUIContent _textEmitter = new GUIContent("Emitter");
        static GUIContent _textReceiver = new GUIContent("Receiver");

        void OnEnable()
        {
            _emitterTransform = serializedObject.FindProperty("_emitterTransform");
            _emitterPosition = serializedObject.FindProperty("_emitterPosition");
            _receiverTransform = serializedObject.FindProperty("_receiverTransform");
            _receiverPosition = serializedObject.FindProperty("_receiverPosition");

            _throttle = serializedObject.FindProperty("_throttle");
            _pulseInterval = serializedObject.FindProperty("_pulseInterval");
            _boltLength = serializedObject.FindProperty("_boltLength");
            _lengthRandomness = serializedObject.FindProperty("_lengthRandomness");

            _noiseAmplitude = serializedObject.FindProperty("_noiseAmplitude");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseMotion = serializedObject.FindProperty("_noiseMotion");

            _color = serializedObject.FindProperty("_color");
            _mesh = serializedObject.FindProperty("_mesh");
            _randomSeed = serializedObject.FindProperty("_randomSeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

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

            EditorGUILayout.PropertyField(_throttle);
            EditorGUILayout.PropertyField(_pulseInterval);
            EditorGUILayout.PropertyField(_boltLength);
            EditorGUILayout.PropertyField(_lengthRandomness);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_noiseAmplitude);
            EditorGUILayout.PropertyField(_noiseFrequency);
            EditorGUILayout.PropertyField(_noiseMotion);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_color);
            EditorGUILayout.PropertyField(_mesh);
            EditorGUILayout.PropertyField(_randomSeed);

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
