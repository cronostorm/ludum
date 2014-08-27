using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public LampScript[] lamps;
    public GameObject lights;
    public PlayerController player;
    public Color endColor;
    public SpriteRenderer sky;
    public Font font;

    private bool gameStart;
    private bool spotted;
    private bool finished;

    public static bool done;

    private Texture2D texture;
    private Rect rect;
    private float flashTime = 0;
    private float finishTime = 0;

    private Color startColor;
    private float startTime = 0;

    private float runTime = 0;


    // Use this for initialization
    void Start() {
        gameStart = false;
        spotted = false;
        finished = false;
        done = false;

        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(1, 0, 0, 0.5f));
        texture.Apply();

        lamps = FindObjectsOfType<LampScript>();

        startColor = Camera.main.backgroundColor;

    }

    void OnTriggerStay2D(Collider2D other) {
        if (done) return;
        if (other.tag == "Player") {
            if (player.isGrounded()) {
                done = true;
                finished = true;
            }
        }
    }

    void Update() {
        //foreach (LampScript lamp in lights.GetComponentsInChildren<LampScript>()) {
        foreach (LampScript lamp in lamps) {
            if (lamp.spottedPlayer) {
                spotted = true;
                done = true;
                return;
            }
        }
        if (!done) {
            runTime = Time.timeSinceLevelLoad;
        }
    }

    void OnGUI() {
        GUIStyle guiStyle = new GUIStyle();
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        guiStyle.font = font;
        guiStyle.normal.textColor = Color.white;
        buttonStyle.font = font;

        guiStyle.fontSize = 16;

        if (!gameStart) {
            startTime += Time.deltaTime;
            if (startTime > 16) {
                gameStart = true;
            }
            if (startTime > 15) {
                guiStyle.normal.textColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), (startTime - 15));
            }

            GUI.Label(new Rect(10, Screen.height - 100, 200, 200),
            "One day you, a young ninja-in-training, are suddenly sucked\n" +
            "into a rip in the time-space continuum!\n" +
            "Now you are far in the future, where everyone is a killer robot!\n" +
            "You must find your way home, but tread carefully and AVOID BEING SPOTTED!\n"
            , guiStyle);

            guiStyle.fontSize = 12;

            float x = Screen.width - 200;
            float y = Screen.height - 100;
            GUI.Label(new Rect(x, y, 200, 20), "Arrow Keys to Move", guiStyle);
            GUI.Label(new Rect(x, y+15, 200, 20), "Spacebar to Jump", guiStyle);
            GUI.Label(new Rect(x, y+30, 200, 20), "Control to Crouch", guiStyle);
            if (GUI.Button(new Rect(x, y+45, 100, 20), "Got it", buttonStyle)) {
                gameStart = true;
            }
            guiStyle.fontSize = 16;
        }

        guiStyle.normal.textColor = Color.white;

        if (spotted) {
            flashTime += Time.deltaTime;
            if (flashTime > 2) {
                flashTime = 0;
            }
            if (flashTime < 1) {
                GUI.skin.box.normal.background = texture;
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none);
            }

            guiStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(0, Screen.height/2 - 60, Screen.width, 40), "You have been seen, and the universe is now broken.", guiStyle);
            if (GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 20, 100, 40), "Oops", buttonStyle) || Input.GetKeyDown("space")) {
                spotted = false;
              Application.LoadLevel(0);
            }
            guiStyle.alignment = TextAnchor.UpperLeft;
        }
        if (finished) {
            finishTime += Time.deltaTime;
            Camera.main.backgroundColor = Color.Lerp(startColor, endColor, finishTime / 3.0f);
            Color color = sky.color;
            color.a = Mathf.Lerp(0, 1.0f, finishTime / 2.0f);
            sky.color = color;

            GUI.Label(new Rect(10, 10, 200, 200), 
            "Congratulations!\n" + 
            "You've managed to escape the robots in " + runTime.ToString("0.00") +" seconds.\n" +
            "Well...for now at least.\n\n" +

            "To be continued...??\n"
            , guiStyle);

            if (GUI.Button(new Rect(10, 100, 100, 20), "Again!", buttonStyle)) {
                Application.LoadLevel(0);
            }
        }

        guiStyle.fontSize = 24;

        guiStyle.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(0, 10, Screen.width - 10, 50), runTime.ToString("0.00") + " s", guiStyle);
    }

}
