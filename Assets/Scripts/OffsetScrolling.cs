using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour {
    public float scrollSpeed = .2f;
    public bool vertical;

    private MeshRenderer rend;
    

    void Start () {
        rend = GetComponent<MeshRenderer>();
    }

    void Update () {
        if (vertical) {
	        rend.material.mainTextureOffset = new Vector2(0f, (Time.time * scrollSpeed));
        } else {
            rend.material.mainTextureOffset = new Vector2(-(Time.time * scrollSpeed), 0f);
        }
    }
}
