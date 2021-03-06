﻿using UnityEngine;
using System.Collections;

public class RotatingLamp : MonoBehaviour {

  public LampScript lamp;
  public float startAngle;
  public float endAngle;
  public float patrolTime;

	// Use this for initialization
	void Start () {
		StartCoroutine(Patrol());
	}

	IEnumerator Patrol(){
		float t = 0;
		while (true) {
			//for sinusoidal platform movement (patrolTime doesn't work)
			//t = (Mathf.Sin ((Time.time*6.28f/patrolTime)) + 1) / 2;

			//for linear platform movement
			t = ((Time.timeSinceLevelLoad % patrolTime) / patrolTime) * 2;
			if (t > 1) t = 2-t;

			lamp.offsetAngle = Mathf.Lerp (startAngle, endAngle, t);
            foreach (Transform child in transform) {
                child.eulerAngles = new Vector3(0, 0, -lamp.offsetAngle + 90);
            }
			yield return null;		
		}
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
