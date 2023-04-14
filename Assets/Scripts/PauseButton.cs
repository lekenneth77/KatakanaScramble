using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseButton : MonoBehaviour
{
    public bool paused;
    public GameObject pauseCon;
    public AudioSource pause_sfx;
    // Start is called before the first frame update
    void Start()
    {
        pause_sfx = GameObject.Find("DJScratch").GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.AddListener(start_or_stop);
    }

    void start_or_stop() {
        pause_sfx.Play();
        if (paused) {
            //TODO fix bug where you can click on interactables when paused
            pauseCon.SetActive(false);
            Time.timeScale = 1;
        } else {
            pauseCon.SetActive(true);
            Time.timeScale = 0;
        }
    }

    
}
