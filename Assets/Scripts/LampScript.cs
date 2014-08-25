using UnityEngine;
using System.Collections;

public class LampScript : MonoBehaviour {
    //static lamp with specified degree for cone of light
    public float lightAngle;
    public float offsetAngle;
    public float distance;
    public int numRays;
    public Material lightMaterial;

    public bool spottedPlayer { get; protected set; }
    protected int[] triangles;
    protected Vector2[] points;
    protected Vector3[] verts;
    // Use this for initialization

    void Start() {
        points = new Vector2[numRays + 1];
        verts = new Vector3[numRays + 1];
        triangles = new int[(numRays - 1) * 3];
        GetComponent<MeshRenderer>().material = lightMaterial;
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        SetCollider();
        GenerateCone();
        mesh.uv = points;
        mesh.triangles = triangles;
    }

    void SetCollider() {
        float angle = lightAngle * Mathf.PI / 180;
        float delta = angle / (numRays - 1);
        angle = offsetAngle * 2 * Mathf.PI / 180 + angle;

        spottedPlayer = false;

        Vector2 origin = transform.position;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        float xangle = Mathf.Sin(-angle / 2);
        float yangle = -Mathf.Cos(-angle / 2);
        verts[0] = new Vector3(0, 0, 0);
        for (int i = 1; i < numRays + 1; i++) {
            xangle = Mathf.Sin(-angle / 2 + delta * (i - 1));
            yangle = -Mathf.Cos(-angle / 2 + delta * (i - 1));
            if (Physics2D.RaycastNonAlloc(origin, new Vector2(xangle, yangle).normalized, hits, distance) > 0) {
                verts[i] = new Vector3(xangle, yangle, 0).normalized * hits[0].fraction * distance;
                spottedPlayer = spottedPlayer || hits[0].collider.tag == "Player";
            } else {
                verts[i] = new Vector3(xangle, yangle, 0) * distance;
            }

            if (i > 1) {
                triangles[(i - 2) * 3] = 0;
                triangles[(i - 2) * 3 + 1] = i - 1;
                triangles[(i - 2) * 3 + 2] = i;
            }
        }
    }

    void GenerateCone() {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = verts;
    }

    void OnDrawGizmos() {
        Vector3 start = transform.position;
        Gizmos.DrawSphere(start, 1);

        float angle = (offsetAngle - lightAngle / 2) * Mathf.PI / 180;
        Vector3 end = start + new Vector3(Mathf.Sin(-angle), -Mathf.Cos(-angle), 0) * distance;
        Gizmos.DrawLine(start, end);

        angle = (offsetAngle + lightAngle / 2) * Mathf.PI / 180;
        end = start + new Vector3(Mathf.Sin(-angle), -Mathf.Cos(-angle), 0) * distance;
        Gizmos.DrawLine(start, end);
    }

    void Update() {
        SetCollider();
        GenerateCone();
    }
}
