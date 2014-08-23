using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
  public float speed = 1.0f;
  
  private bool facingRight = true;

	void Start () {
	}
	
  void FixedUpdate() {
    float move = Input.GetAxis("Horizontal");
    rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);

    if ((move > 0 && !facingRight) || (move < 0 && facingRight)) {
      Flip();
    }
  }

  void Flip() {
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }
}
