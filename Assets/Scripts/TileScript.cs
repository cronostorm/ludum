using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject level = GameObject.Find("level");
        foreach (Transform childTransform in level.transform) {
            GameObject child = childTransform.gameObject;
            if (child.renderer.material.mainTexture != null) {
                float w = child.renderer.material.mainTexture.width / 32.0f;
                float h = child.renderer.material.mainTexture.height / 32.0f;
                child.renderer.material.mainTextureScale = new Vector2(childTransform.localScale.x / w, childTransform.localScale.y / h);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    /*void OnDrawGizmos() {
        float w = renderer.material.mainTexture.width / 16.0f;
        float h = renderer.material.mainTexture.height / 16.0f;
        renderer.material.mainTextureScale = new Vector2(transform.localScale.x / w, transform.localScale.y / h);
    }*/
}
