using UnityEngine;
using System.Collections;

public class PlatformMoving : MonoBehaviour {

	public float distance;
	public float xDist;
	public float yDist;
	public float patrolTime;
	protected bool hitEnd = false;

	// Use this for initialization
	void Start () {
		Vector3 origin = transform.position;
		Vector3 destination = transform.position + new Vector3 (xDist, yDist, 0);
		StartCoroutine(Patrol (origin, destination));
	}

	IEnumerator Patrol(Vector3 start, Vector3 end){
		float t = 0;
		while (true) {
			//for sinusoidal platform movement (patrolTime doesn't work)
			//t = (Mathf.Sin ((Time.time*6.28f/patrolTime)) + 1) / 2;

			//for linear platform movement
			t = ((Time.time % patrolTime) / patrolTime) * 2;
			if (t > 1) t = 2-t;
			Debug.Log (t);
			transform.position = Vector3.Lerp (start, end, t);
			yield return null;		
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
