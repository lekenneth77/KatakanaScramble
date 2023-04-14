using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    //name is misleading but it's for any scene transition buttons
    public AudioSource click_sfx;
    public GameObject loadingPanel;
    public int sceneIndex;
    void Start() {
        click_sfx = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
        gameObject.GetComponent<Button>().onClick.AddListener(clicked);
        loadingPanel.GetComponent<FadeOutPanel>().sceneIndex = sceneIndex;
    }
    
    private void clicked() {
        click_sfx.Play();
        Time.timeScale = 1;
        loadingPanel.SetActive(true);
    }
}
