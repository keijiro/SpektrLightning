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
using System.Collections;

namespace Spektr
{
    [ExecuteInEditMode]
    public class LightningRenderer : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        Transform _emitterTransform;

        public Transform emitterTransform {
            get { return _emitterTransform; }
            set { _emitterTransform = value; }
        }

        [SerializeField]
        Vector3 _emitterPosition = Vector3.right * -5;

        public Vector3 emitterPosition {
            get { return _emitterTransform ? _emitterTransform.position : _emitterPosition; }
            set { _emitterPosition = value; }
        }

        [SerializeField]
        Transform _receiverTransform;

        public Transform receiverTransform {
            get { return _receiverTransform; }
            set { _receiverTransform = value; }
        }

        [SerializeField]
        Vector3 _receiverPosition = Vector3.right * 5;

        public Vector3 receiverPosition {
            get { return _receiverTransform ? _receiverTransform.position : _receiverPosition; }
            set { _receiverPosition = value; }
        }

        [SerializeField, Range(0, 1)]
        float _throttle = 0.1f;

        public float throttle {
            get { return _throttle; }
            set { _throttle = value; }
        }

        [SerializeField]
        float _pulseInterval = 0.2f;

        public float pulseInterval {
            get { return _pulseInterval; }
            set { _pulseInterval = value; }
        }

        [SerializeField, Range(0, 1)]
        float _boltLength = 0.85f;

        public float boltLength {
            get { return _boltLength; }
            set { _boltLength = value; }
        }

        [SerializeField, Range(0, 1)]
        float _lengthRandomness = 0.8f;

        public float lengthRandomness {
            get { return _lengthRandomness; }
            set { _lengthRandomness = value; }
        }

        [SerializeField]
        float _noiseAmplitude = 1.2f;

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        [SerializeField]
        float _noiseFrequency = 0.1f;

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        [SerializeField]
        float _noiseMotion = 0.1f;

        public float noiseMotion {
            get { return _noiseMotion; }
            set { _noiseMotion = value; }
        }

        [SerializeField, ColorUsage(false, true, 0, 16, 0.125f, 3)]
        Color _color = Color.white;

        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        [SerializeField]
        LightningMesh _mesh;

        [SerializeField]
        int _randomSeed;

        #endregion

        #region Private Resources

        [SerializeField, HideInInspector]
        Shader _shader;

        #endregion

        #region Private Variables and Methods

        Material _material;
        MaterialPropertyBlock _materialProps;

        #endregion

        #region MonoBehaviour Functions

        void Update()
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            if (_materialProps == null)
                _materialProps = new MaterialPropertyBlock();

            // emitter/receiver position in the world space
            var p0 = transform.InverseTransformPoint(emitterPosition);
            var p1 = transform.InverseTransformPoint(receiverPosition);

            _material.SetVector("_Point0", p0);
            _material.SetVector("_Point1", p1);
            _material.SetFloat("_Distance", (p1 - p0).magnitude);

            // make orthogonal axes
            var v0 = (p1 - p0).normalized;
            var v0s = Mathf.Abs(v0.y) > 0.707f ? Vector3.right : Vector3.up;
            var v1 = Vector3.Cross(v0, v0s).normalized;
            var v2 = Vector3.Cross(v0, v1);

            _material.SetVector("_Axis0", v0);
            _material.SetVector("_Axis1", v1);
            _material.SetVector("_Axis2", v2);

            // other params
            _material.SetFloat("_Throttle", _throttle);
            _material.SetVector("_Interval", new Vector2(0.01f, _pulseInterval - 0.01f));
            _material.SetVector("_Length", new Vector2(1 - _lengthRandomness, 1) * _boltLength);

            _material.SetVector("_NoiseAmplitude", new Vector2(1, 0.1f) * _noiseAmplitude);
            _material.SetVector("_NoiseFrequency", new Vector2(1, 10) * _noiseFrequency);
            _material.SetVector("_NoiseMotion", new Vector2(1, 10) * _noiseMotion);

            _material.SetColor("_Color", _color);
            _material.SetFloat("_Seed", _randomSeed * _mesh.lineCount);

            // draw lines
            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _material, 0, null, 0, _materialProps);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(emitterPosition, _noiseAmplitude);
            Gizmos.DrawSphere(receiverPosition, _noiseAmplitude);
        }

        #endregion
    }
}
