using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GetCategories : MonoBehaviour
{

    public GameObject parent;
    public GameObject loadingPanel;

    const string src_dir = "Assets/Texts";
    private Sprite[] inter_sprites;

    private GameObject[] buttons;

    string[] files;
    // Start is called before the first frame update
    void Start()
    {
        inter_sprites = Resources.LoadAll<Sprite>("interactables/");

        char[] delims = {'\\', '/'};
        files = Directory.GetFiles(src_dir);
        buttons = new GameObject[files.Length / 2];

        //heavy assumption that meta files are going to right after the regular files.
        for (int i = 0; i < files.Length; i += 2) {
            StreamReader inp_stm = new StreamReader(files[i]);
            string category_name = inp_stm.ReadLine( );
            buttons[i / 2] = create_button(files[i], category_name, new Vector3(0, 200 - (250 * (i / 2))));
        }

        
    }

    private GameObject create_button(string file_path ,string category_name, Vector3 pos) {
        //perhaps in the future add an image slideshow here showcasing the category?

        TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
        GameObject button = TMP_DefaultControls.CreateButton(resources);
        button.GetComponent<Button>().onClick.AddListener(delegate{choose_category(file_path);});
        RectTransform trans = button.GetComponent<RectTransform>();
        trans.transform.SetParent(parent.transform);
        trans.localScale = Vector3.one;
        trans.sizeDelta = new Vector3(1300, 150, 1);
        trans.localPosition = pos;

       GameObject text_box = button.transform.GetChild(0).gameObject;
        RectTransform text_trans = text_box.GetComponent<RectTransform>();
        text_trans.localPosition = Vector3.one;
        text_trans.localScale = Vector3.one;
        text_trans.sizeDelta = new Vector3(1300, 150, 1);
        TextMeshProUGUI text = text_box.GetComponent<TextMeshProUGUI>();
        text.text = category_name;
        text.fontSize = 100;
        text.color = Color.black;
        text.alignment = TextAlignmentOptions.Center;

        return button;   
    }

    void choose_category(string file_path) {
        GameControl.file_path = file_path;
        foreach (GameObject button in buttons) {
            Destroy(button);
        }
        StartCoroutine("load_scene_async");
    }

    IEnumerator load_scene_async() {
        loadingPanel.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(0);
        while ( !op.isDone )
        {
            yield return null;
        }
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
