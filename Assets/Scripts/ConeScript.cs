using UnityEngine;
using System.Collections;

public class ConeScript : MonoBehaviour {
  public float speed; 
  private bool facingRight;
  private Rigidbody2D body;
  private Vector3 parentPos;
  

	// Use this for initialization
	void Start () {
	  body = transform.parent.gameObject.rigidbody2D;
    parentPos = transform.parent.position;
    facingRight = transform.parent.localScale.x > 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnTriggerStay2D(Collider2D collider) {
    GameObject player = collider.gameObject;
    if ((player.transform.position.x > parentPos.x && !facingRight) || 
       (player.transform.position.x < parentPos.x && facingRight)) {
      Flip();
    }

    if (facingRight) {
      Move(speed, body.velocity.y);
    }
    else {
      Move(-1 * speed, body.velocity.y);
    }
  }

  void Move(float xvel, float yvel) {
    body.velocity = new Vector2(xvel, yvel);
  }
  
  void Flip() {
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
  }
}
