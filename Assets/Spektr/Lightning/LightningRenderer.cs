using UnityEngine;
using System.Collections;

namespace Spektr
{
    [ExecuteInEditMode]
    public class LightningRenderer : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        LightningMesh _mesh;

        [Space]
        [SerializeField]
        Vector3 _emitterPosition = Vector3.right * -5;

        public Vector3 emitterPosition {
            get { return _emitterPosition; }
            set { _emitterPosition = value; }
        }

        [SerializeField]
        Vector3 _receiverPosition = Vector3.right * 5;

        public Vector3 receiverPosition {
            get { return _receiverPosition; }
            set { _receiverPosition = value; }
        }

        [Space]
        [SerializeField]
        float _interval = 0.4f;

        [SerializeField, Range(0, 1)]
        float _length = 0.5f;

        [SerializeField, Range(0, 1)]
        float _lengthRandomness = 0.5f;

        [Space]
        [SerializeField]
        float _noiseAmplitude1 = 0.05f;

        [SerializeField]
        float _noiseFrequency1 = 1.0f;

        [SerializeField]
        float _noiseMotion1 = 0.2f;

        [Space]
        [SerializeField]
        float _noiseAmplitude2 = 0.05f;

        [SerializeField]
        float _noiseFrequency2 = 1.0f;

        [SerializeField]
        float _noiseMotion2 = 0.2f;

        [Space]
        [SerializeField, ColorUsage(false, true, 0, 16, 0.125f, 3)]
        Color _color = Color.white;

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
            var p0 = transform.InverseTransformPoint(_emitterPosition);
            var p1 = transform.InverseTransformPoint(_receiverPosition);

            _material.SetVector("_Point0", p0);
            _material.SetVector("_Point1", p1);

            // make orthogonal axes
            var v0 = (p1 - p0).normalized;
            var v0s = Mathf.Abs(v0.y) > 0.707f ? Vector3.right : Vector3.up;
            var v1 = Vector3.Cross(v0, v0s).normalized;
            var v2 = Vector3.Cross(v0, v1);

            _material.SetVector("_Axis0", v0);
            _material.SetVector("_Axis1", v1);
            _material.SetVector("_Axis2", v2);

            // other params
            _material.SetVector("_Interval", new Vector2(0.01f, _interval - 0.01f));
            _material.SetVector("_Length", new Vector2(1 - _lengthRandomness, 1) * _length);

            _material.SetVector("_NoiseAmplitude", new Vector2(_noiseAmplitude1, _noiseAmplitude2));
            _material.SetVector("_NoiseFrequency", new Vector2(_noiseFrequency1, _noiseFrequency2));
            _material.SetVector("_NoiseMotion", new Vector2(_noiseMotion1, _noiseMotion2));

            _material.SetColor("_Color", _color);

            // draw lines
            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _material, 0, null, 0, _materialProps);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(_emitterPosition, _noiseAmplitude1);
            Gizmos.DrawSphere(_receiverPosition, _noiseAmplitude1);
        }

        #endregion
    }
}
