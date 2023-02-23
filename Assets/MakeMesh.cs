using System;
using UnityEngine;

public class MakeMesh : MonoBehaviour
{
    [SerializeField] private float _factor;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private MeshFilter _filter;
    private Mesh _mesh;
    private float _offset;

    [ContextMenu("GenMesh")]
    void Start()
    {
        _mesh = new Mesh();
        _mesh.name = "Custom" ;
        Generator(_mesh);
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        _filter.sharedMesh = _mesh;
    }

    private void Generator(Mesh mesh)
    {
        mesh.vertices = GenVertices(0);
        mesh.triangles = GenTriangles();
    }

    private int[] GenTriangles()
    {
        return new[]
        {
            //bottom
             0,1,2,
             0,2,3,
            
            //top
            4,5,6,
            4,6,7,
            
            //left
            0,4,7,
            7,3,0,
            
            //right
            6,5,1,
            1,2,6,
            
            //back
            5,4,0,
            0,1,5,
            
            //forward
            6,2,3,
            3,7,6,
        };
    }

    private Vector3[] GenVertices(float scale)
    {
        return new []
        {
            //bottom
            new Vector3(-1,0,1),
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1),
            
            //top
            new Vector3(-1+scale,2,1),
            new Vector3(1+scale,2,1),
            new Vector3(1+scale,2,-1),
            new Vector3(-1+scale,2,-1)
        };
    }

    private void Update()
    {
        _offset += _speed * Time.deltaTime;
        _mesh.vertices = GenVertices(Mathf.Sin(_offset)*_factor);
    }
}
