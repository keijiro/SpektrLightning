using UnityEngine;
using System.Collections.Generic;

namespace Spektr
{
    public class LightningMesh : ScriptableObject
    {
        #region Public Properties

        [SerializeField]
        int _vertexCount = 64;

        public int vertexCount {
            get { return _vertexCount; }
        }

        [SerializeField, HideInInspector]
        Mesh _mesh;

        public Mesh sharedMesh {
            get { return _mesh; }
        }

        #endregion

        #region Public Methods

        public void RebuildMesh()
        {
            if (_mesh == null)
            {
                Debug.LogError("Mesh asset is missing.");
                return;
            }

            _mesh.Clear();

            var varray = new Vector3[_vertexCount];
            var iarray = new int[_vertexCount];

            for (var vi = 0; vi < _vertexCount; vi++)
            {
                varray[vi] = Vector3.right * ((float)vi / (_vertexCount - 1));
                iarray[vi] = vi;
            }

            _mesh.vertices = varray;
            _mesh.SetIndices(iarray, MeshTopology.LineStrip, 0);

            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100);

            _mesh.Optimize();
            _mesh.UploadMeshData(true);
        }

        #endregion

        #region ScriptableObject Functions

        void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Line";
            }
        }

        #endregion
    }
}
