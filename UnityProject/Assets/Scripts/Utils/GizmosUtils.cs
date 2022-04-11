using UnityEngine;

public class GizmosUtils
{
    private static Mesh _cubeMesh = new Mesh();

    private static void RotatePointsAroundPivot(Vector3 center, Vector3[] points, Quaternion rotation)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 dir = points[i] - center; // get point direction relative to center
            dir = rotation * dir; // rotate it
            points[i] = dir + center; // calculate rotated point
        }
    }
     
    public static void DrawCube(Vector3 center, Vector3 size, Quaternion rotation)
    {
    
        _cubeMesh = new Mesh();
        var midSize = size / 2;
        Vector3[] vertices = {
            new Vector3 (center.x + midSize.x, center.y + midSize.y, center.z + midSize.z),
            new Vector3 (center.x - midSize.x, center.y + midSize.y, center.z + midSize.z),
            new Vector3 (center.x + midSize.x, center.y - midSize.y, center.z + midSize.z),
            new Vector3 (center.x - midSize.x, center.y - midSize.y, center.z + midSize.z),
            
            new Vector3 (center.x + midSize.x, center.y + midSize.y, center.z - midSize.z),
            new Vector3 (center.x - midSize.x, center.y + midSize.y, center.z - midSize.z),
            new Vector3 (center.x + midSize.x, center.y - midSize.y, center.z - midSize.z),
            new Vector3 (center.x - midSize.x, center.y - midSize.y, center.z - midSize.z),
        };
        int[] triangles = {
            0, 1, 2, //face front
            1, 3, 2,
            0, 4, 1, //face top
            1, 4, 5,
            0, 2, 6, //face right
            0, 6, 4,
            1, 5, 3, //face left
            3, 5, 7,
            4, 6, 7, //face back
            4, 7, 5,
            2, 3, 6, //face bottom
            3, 7, 6
        };
        RotatePointsAroundPivot(center, vertices, rotation);
        
        _cubeMesh.Clear ();
        _cubeMesh.vertices = vertices;
        _cubeMesh.triangles = triangles;
        _cubeMesh.Optimize ();
        _cubeMesh.RecalculateNormals ();
        
        Gizmos.DrawMesh(_cubeMesh);
    }
}