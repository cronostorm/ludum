using UnityEngine;
using System.Collections;

public class ConeScript : MonoBehaviour {
  public float normalSpeed; 
  public float sightDistance;
  private bool facingRight;
  public Rigidbody2D body;

  private int horizontal = 0;
  

	// Use this for initialization
	void Start () {
    facingRight = transform.localScale.x > 0;
	}
	
  void FixedUpdate() {
    bool hitplayer = false;
    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(1, 0), sightDistance);
    foreach (RaycastHit2D hit in hits) {
      if (hit.transform.tag == "Player") {
        hitplayer = true;
        break;
      }
    }
    if (!hitplayer) return;
    
    horizontal = -1;
    if (facingRight) {
      horizontal = 1;
    }

    body.AddForce(new Vector2(horizontal * 75, 0));
    Vector2 vel = body.velocity;
    vel.x = Mathf.Clamp(vel.x, -normalSpeed, normalSpeed);
    body.velocity = vel;
  }

  void Flip() {
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }
}
