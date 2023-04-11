using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCategories : MonoBehaviour
{

    const string src_dir = "Assets/Texts";
    string[] files;
    // Start is called before the first frame update
    void Start()
    {
        char[] delims = {'\\', '/'};
        files = Directory.GetFiles(src_dir);
        foreach (string file_name in files) {
            Debug.Log(file_name);
        }
    }



    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown("1")) {
            GameControl.file_path = files[0];
            SceneManager.LoadSceneAsync(0);
        } 
        if (Input.GetKeyDown("2")) {
            GameControl.file_path = files[2];
            SceneManager.LoadSceneAsync(0);
        }
        
    }
}
