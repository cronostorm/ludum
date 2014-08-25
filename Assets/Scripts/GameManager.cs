using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
  public LampScript[] lamps;
  private bool gameStart;
  public static bool spotted;

	// Use this for initialization
	void Start () {
    gameStart = false;
    spotted = false;
	}

  void Update() {
    foreach (LampScript lamp in lamps) {
      if (lamp.spottedPlayer) {
        spotted = true;
        return;
      }
    }
  
  }
	
	void OnGUI() {
    if (!gameStart) {
      GUI.Label(new Rect(10, 10, 200, 20), "Arrow Keys to Move");
      GUI.Label(new Rect(10, 30, 200, 20), "Spacebar to Jump");
      GUI.Label(new Rect(10, 50, 200, 20),"Control to Crouch");
      GUI.Label(new Rect(10, 80, 200, 20),"<b>Avoid being spotted!</b>");
      if (GUI.Button(new Rect(10, 100, 70, 20), "Dismiss")) {
        gameStart=true;
      }
    }
    if (spotted) {
        GUI.Label(new Rect(Screen.width/2 - 50, Screen.height/2 - 60, 100, 40), "You were Spotted");
        if (GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 20, 100, 40), "Retry") || Input.GetKeyDown("space")) {
          Application.LoadLevel(0);
        }
    }
	}
}
