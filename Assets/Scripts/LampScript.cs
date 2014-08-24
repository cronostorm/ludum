using UnityEngine;
using System.Collections;

public class LampScript : MonoBehaviour {

	//static lamp with specified degree for cone of light

	public float lightAngle;
	public float distance;
	public Material lightMaterial;
	protected Vector3[] verts;
	protected int[] triangles;
	protected Vector2[] points;
	// Use this for initialization

	void Start () {
		SetCollider ();
		verts = new Vector3[points.Length];
		for (int i = 0; i < points.Length; i++){
			verts[i] = (Vector3) points[i];
		}
		triangles = new int[] {0, 1, 2};
		GenerateCone (verts, triangles);
	}
	
	void SetCollider () {
		float angle = lightAngle * Mathf.PI / 180;
		points = new Vector2[3];
		Vector2 down = new Vector2(0, -1) * distance;
		Vector2 side = new Vector2(1, 0) * (distance * Mathf.Tan (angle/2));
		points [0] = new Vector2(0,0);
		points [1] = points[0] + down + side;
		points [2] = points [0] + down - side;
		gameObject.GetComponent<PolygonCollider2D> ().SetPath (0, points);
	}

	void GenerateCone (Vector3[] verts, int[] triangles) {
		gameObject.AddComponent ("MeshFilter");
		gameObject.AddComponent ("MeshRenderer");
		Mesh mesh = GetComponent <MeshFilter> ().mesh;
		GetComponent<MeshRenderer> ().material = lightMaterial;
		mesh.Clear ();
		mesh.vertices = verts;
		mesh.triangles = triangles;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
