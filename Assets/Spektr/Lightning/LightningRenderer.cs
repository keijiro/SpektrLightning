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
        Vector3 _pointFrom = Vector3.zero;

        [SerializeField]
        Vector3 _pointTo = Vector3.up * 5;

        [Space]
        [SerializeField]
        float _interval = 0.4f;

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

        #region Private Variables

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

            _material.SetVector("_Point0", _pointFrom);
            _material.SetVector("_Point1", _pointTo);
            _material.SetVector("_Interval", new Vector2(0.01f, _interval - 0.01f));
            _material.SetVector("_NoiseAmplitude", new Vector2(_noiseAmplitude1, _noiseAmplitude2));
            _material.SetVector("_NoiseFrequency", new Vector2(_noiseFrequency1, _noiseFrequency2));
            _material.SetVector("_NoiseMotion", new Vector2(_noiseMotion1, _noiseMotion2));
            _material.SetColor("_Color", _color);

            for (var i = 0; i < 20; i++)
            {
                _materialProps.SetFloat("_RandomSeed", 0.13f + i * 0.31f);
                Graphics.DrawMesh(
                    _mesh.sharedMesh, transform.localToWorldMatrix,
                    _material, 0, null, 0, _materialProps
                );
            }
        }

        #endregion
    }
}
