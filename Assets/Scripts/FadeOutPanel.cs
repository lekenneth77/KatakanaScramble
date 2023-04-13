using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOutPanel : MonoBehaviour
{
    public int sceneIndex;

    void OnEnable() {
        StartCoroutine("load_scene_async");
    }

    IEnumerator load_scene_async() {
        Image img = gameObject.GetComponent<Image>();
        Color temp = img.color;
        temp.a = 0;
        gameObject.SetActive(true);
        img.color = temp;
        for (int i = 1; i < 100; i++) {
            temp = img.color;
            temp.a += 0.01f;
            img.color = temp;
            yield return new WaitForSeconds(0.005f);
        }
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
    }

}
