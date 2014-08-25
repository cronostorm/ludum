using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
  public float normalSpeed; 
  public float chaseSpeed; 
  public float sightDistance;
  private bool facingRight;
  public Rigidbody2D body;
  public LampScript lamp;
  public Animator animator;

  public Transform start;
  public Transform end;

  private float lastFlip;
  private int horizontal = 0;
  

	// Use this for initialization
	void Start () {
        lastFlip = 0;
        facingRight = transform.localScale.x > 0;
	}
	
  void FixedUpdate() {
      Debug.Log(transform.position);
    if (Mathf.Abs(transform.position.x - start.position.x) < 0.1f) {
      Flip();
    }
    else if (Mathf.Abs(transform.position.x - end.position.x) < 0.1f) {
      Flip();
    }

    if (lamp.spottedPlayer) ChasePlayer();
    else Patrol();
  }

  void Update() {
      animator.SetBool("Grounded", true);
      animator.SetBool("Moving", rigidbody2D.velocity.x != 0);
  }

  void Patrol() {
    horizontal = -1;
    if (facingRight) {
      horizontal = 1;
    }

    body.AddForce(new Vector2(horizontal * 75, 0));
    Vector2 vel = body.velocity;
    vel.x = Mathf.Clamp(vel.x, -normalSpeed, normalSpeed);
    body.velocity = vel;
  }

  void ChasePlayer() {
    horizontal = -1;
    if (facingRight) {
      horizontal = 1;
    }

    body.AddForce(new Vector2(horizontal * 1000, 0));
    Vector2 vel = body.velocity;
    vel.x = Mathf.Clamp(vel.x, -chaseSpeed, chaseSpeed);
    body.velocity = vel;
  }

  void Flip() {
    if (Time.time - lastFlip < 0.2f) {
      return;
    }
    lastFlip = Time.time;
    facingRight = !facingRight;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale = scale;
    lamp.offsetAngle = -90 * transform.localScale.x;
    lamp.transform.localScale = new Vector3(1/transform.localScale.x, 1/transform.localScale.y, 1/transform.localScale.z);
    
  }
}
