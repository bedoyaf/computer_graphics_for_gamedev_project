using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralPlane : MonoBehaviour
{
    [Header("Plane Settings")]
    public int resolution = 100; // poèet vertexù na osu
    public float size = 10f;     // velikost plane

    void Start()
    {
        Generate();
    }
    [ContextMenu("Generate")]
    public void Generate()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Plane";

        int vertCount = (resolution + 1) * (resolution + 1);

        Vector3[] vertices = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] triangles = new int[resolution * resolution * 6];

        float step = size / resolution;

        int v = 0;
        int t = 0;

        for (int z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                float xPos = x * step - size / 2f;
                float zPos = z * step - size / 2f;

                vertices[v] = new Vector3(xPos, 0, zPos);
                uvs[v] = new Vector2((float)x / resolution, (float)z / resolution);

                if (x < resolution && z < resolution)
                {
                    int i = v;

                    triangles[t++] = i;
                    triangles[t++] = i + resolution + 1;
                    triangles[t++] = i + 1;

                    triangles[t++] = i + 1;
                    triangles[t++] = i + resolution + 1;
                    triangles[t++] = i + resolution + 2;
                }

                v++;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}