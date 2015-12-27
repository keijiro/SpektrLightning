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

        [SerializeField]
        float _pointSpread = 1.0f;

        public float pointSpread {
            get { return _pointSpread; }
            set { _pointSpread = value; }
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

        static Vector4 MakeVector4(Vector3 v, float s)
        {
            return new Vector4(v.x, v.y, v.z, s);
        }

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

            var p0 = transform.InverseTransformPoint(_emitterPosition);
            var p1 = transform.InverseTransformPoint(_receiverPosition);

            _material.SetVector("_Point0", MakeVector4(p0, _pointSpread));
            _material.SetVector("_Point1", MakeVector4(p1, _pointSpread));

            _material.SetVector("_Interval", new Vector2(0.01f, _interval - 0.01f));
            _material.SetVector("_Length", new Vector2(1 - _lengthRandomness, 1) * _length);

            _material.SetVector("_NoiseAmplitude", new Vector2(_noiseAmplitude1, _noiseAmplitude2));
            _material.SetVector("_NoiseFrequency", new Vector2(_noiseFrequency1, _noiseFrequency2));
            _material.SetVector("_NoiseMotion", new Vector2(_noiseMotion1, _noiseMotion2));
            _material.SetColor("_Color", _color);

            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _material, 0, null, 0, _materialProps);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 0, 0.2f);
            Gizmos.DrawSphere(_emitterPosition, _pointSpread);
            Gizmos.DrawSphere(_receiverPosition, _pointSpread);
        }

        #endregion
    }
}
