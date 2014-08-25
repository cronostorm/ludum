﻿using UnityEngine;
using System.Collections;

public class LampScript : MonoBehaviour {

    //static lamp with specified degree for cone of light

    public float lightAngle;
    public float distance;
    public int numRays;
    public Material lightMaterial;
    private float lastTime = 0;
    protected int[] triangles;
    protected Vector2[] points;
    protected Vector3[] verts;
    // Use this for initialization

    void Start() {
        points = new Vector2[numRays + 1];
        verts = new Vector3[numRays + 1];
        triangles = new int[(numRays - 1) * 3];
        gameObject.AddComponent("MeshFilter");
        gameObject.AddComponent("MeshRenderer");
        GetComponent<MeshRenderer>().material = lightMaterial;
        GenerateConeFirst();
        InvokeRepeating("SetCollider", 0, 0.5f);
    }

    void SetCollider() {
        float angle = lightAngle * Mathf.PI / 180;
        float delta = angle / (numRays - 1);

        Vector2 origin = transform.position;
        RaycastHit2D[] hits = new RaycastHit2D[1];
        float xangle = Mathf.Sin(-angle / 2);
        float yangle = -Mathf.Cos(-angle / 2);
        points[0] = new Vector2(0, 0);
        verts[0] = (Vector3)points[0];
        for (int i = 1; i < numRays + 1; i++) {
            xangle = Mathf.Sin(-angle / 2 + delta * (i - 1));
            yangle = -Mathf.Cos(-angle / 2 + delta * (i - 1));
            if (Physics2D.RaycastNonAlloc(origin, new Vector2(xangle, yangle).normalized, hits, distance) > 0) {
                points[i] = new Vector2(xangle, yangle).normalized * hits[0].fraction * distance;
            } else {
                points[i] = new Vector2(xangle, yangle) * distance;
            }
            verts[i] = (Vector3)points[i];
            if (i > 1) {
                triangles[(i - 2) * 3] = 0;
                triangles[(i - 2) * 3 + 1] = i - 1;
                triangles[(i - 2) * 3 + 2] = i;
            }
        }

        gameObject.GetComponent<PolygonCollider2D>().SetPath(0, points);
        GenerateCone();
    }

    void GenerateConeFirst() {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    void GenerateCone() {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = verts;
        mesh.uv = points;
        mesh.triangles = triangles;
    }

    void Update() {
    }
}
