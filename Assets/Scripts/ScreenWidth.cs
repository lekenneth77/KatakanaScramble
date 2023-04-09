using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenWidth : MonoBehaviour {

    // Set this to the in-world distance between the left & right edges of your scene.
    public float sceneWidth = 105;

    Camera vcam;

    int old_width = 0;

    void Start() {
        vcam = GetComponent<Camera>();
        float unitsPerPixel = sceneWidth / Screen.width;
        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
        vcam.orthographicSize = desiredHalfHeight;
        old_width = Screen.width;
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update() {
        if (old_width != Screen.width) {
            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
            vcam.orthographicSize = desiredHalfHeight;
            old_width = Screen.width;
        }
    }
}