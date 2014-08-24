using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
  public float xMargin;
  public float yMargin;
  public float xSmooth;
  public float ySmooth;

  public Transform player;


  bool CheckXMargin() {
    return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
  }
  
  bool CheckYMargin() {
    return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
  }

  void FixedUpdate() {
    TrackPlayer();
  }

  void TrackPlayer() {
    float targetX = transform.position.x;
    float targetY = transform.position.y;

    if (CheckXMargin()) {
      targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
    }
    if (CheckYMargin()) {
      targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
    }

    transform.position = new Vector3(targetX, targetY, transform.position.z);
  }
}
