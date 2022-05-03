#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

//https://answers.unity.com/questions/1594750/is-there-a-premade-triangle-asset.html
//https://blog.nobel-joergensen.com/2010/12/25/procedural-generated-mesh-in-unity/
public class TrianglePrimitive
{
#if UNITY_EDITOR
    private static Mesh CreateMesh()
    {
        //        Vector3[] vertices = {
        //             new Vector3(-0.5f, -0.5f, 0.5f),
        //             new Vector3(0.5f, -0.5f, 0.5f),
        //             new Vector3(0f, 0.5f, 0.5f),
        //             new Vector3(0.5f, -0.5f, -0.5f),
        //             new Vector3(-0.5f, -0.5f, -0.5f),
        //             new Vector3(0f, -0.5f, -0.5f),
        //         };

        //        //Vector2[] uv = {
        //        //     new Vector2(0, 0),
        //        //     new Vector2(1, 0),
        //        //     new Vector2(0.5f, 1),
        //        //     new Vector2(0, 0),
        //        //     new Vector2(1, 0),
        //        //     new Vector2(0.5f, 1),
        //        // };

        //        int[] triangles = { 0, 1, 2, 3, 4, 5 };

        //        var mesh = new Mesh();
        //        mesh.vertices = vertices;
        ////        mesh.uv = uv;
        //        mesh.triangles = triangles;
        //        mesh.RecalculateBounds();
        //        mesh.RecalculateNormals();
        //        mesh.RecalculateTangents();
        //        return mesh;

        Mesh mesh = new Mesh();

        Vector3 p0 = new Vector3(0, 0, 0) + new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p1 = new Vector3(1, 0, 0) + new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f)) + new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3) + new Vector3(-0.5f, -0.5f, -0.5f);

        mesh.Clear();
        mesh.vertices = new Vector3[]{
            p0,p1,p2,
            p0,p2,p3,
            p2,p1,p3,
            p0,p3,p1
        };
        mesh.triangles = new int[]{
            0,1,2,
            3,4,5,
            6,7,8,
            9,10,11
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }

    private static GameObject CreateObject()
    {
        var obj = new GameObject("Triangle");
        var mesh = CreateMesh();
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    [MenuItem("GameObject/3D Object/Triangle", false, 0)]
    public static void Create()
    {
        CreateObject();
    }
#endif
}