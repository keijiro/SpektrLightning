using UnityEngine;
using System.Collections.Generic;

namespace Spektr
{
    public class LightningMesh : ScriptableObject
    {
        #region Public Properties

        [SerializeField]
        int _lineCount = 20;

        public int lineCount {
            get { return _lineCount; }
        }

        [SerializeField]
        int _vertexCount = 30;

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

            var lcount = Mathf.Max(_lineCount, 1);
            var vcount = Mathf.Max(_vertexCount, 2);

            var varray = new List<Vector3>(lcount * vcount);
            var iarray = new List<int>(lcount * (vcount - 1) * 2);

            for (var li = 0; li < lcount; li++)
                for (var vi = 0; vi < vcount; vi++)
                    varray.Add(new Vector3((float)vi / (vcount - 1), li + 1, 0));

            for (var li = 0; li < lcount; li++)
            {
                for (var vi = 0; vi < vcount - 1; vi++)
                {
                    var i = li * vcount + vi;
                    iarray.Add(i);
                    iarray.Add(i + 1);
                }
            }

            _mesh.SetVertices(varray);
            _mesh.SetIndices(iarray.ToArray(), MeshTopology.Lines, 0);

            // very bad way to avoid being culled. don't do at home.
            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);

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
                _mesh.name = "Lines";
            }
        }

        #endregion
    }
}
