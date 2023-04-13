using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject loadingPanel;
    void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(clicked);
    }
    
    private void clicked() {
        loadingPanel.SetActive(true);
    }
}
