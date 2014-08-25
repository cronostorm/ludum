using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public LampScript[] lamps;
    public GameObject lights;

    private bool gameStart;
    public static bool spotted;

    private Texture2D texture;
    private Rect rect;
    private float flashTime;

    // Use this for initialization
    void Start() {
        gameStart = false;
        spotted = false;

        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(1, 0, 0, 0.5f));
        texture.Apply();

        lamps = FindObjectsOfType<LampScript>();
    }

    void Update() {
        //foreach (LampScript lamp in lights.GetComponentsInChildren<LampScript>()) {
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
            GUI.Label(new Rect(10, 50, 200, 20), "Control to Crouch");
            GUI.Label(new Rect(10, 80, 200, 20), "<b>Avoid being spotted!</b>");
            if (GUI.Button(new Rect(10, 100, 70, 20), "Dismiss")) {
                gameStart = true;
            }
        }
        if (spotted) {
            flashTime += Time.deltaTime;
            if (flashTime > 2) {
                flashTime = 0;
            }
            if (flashTime < 1) {
                GUI.skin.box.normal.background = texture;
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
            }

            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 100, 40), "You were Spotted");
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), "Retry") || Input.GetKeyDown("space")) {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
