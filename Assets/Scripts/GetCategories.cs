using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GetCategories : MonoBehaviour
{

    public GameObject parent;
    public GameObject loadingPanel;
    public GameObject loadingText;

    const string src_dir = "Assets/Texts";
    private Sprite[] inter_sprites;

    private GameObject[] buttons;
    private TMP_FontAsset font;

    string[] files;
    // Start is called before the first frame update
    void Start()
    {
        inter_sprites = Resources.LoadAll<Sprite>("interactables/");
        TMP_FontAsset[] fonts = Resources.LoadAll<TMP_FontAsset>("SU3DJPFont/TextMeshProFont/Selected/");
        font = fonts[0];

        char[] delims = {'\\', '/'};
        files = Directory.GetFiles(src_dir);
        buttons = new GameObject[files.Length / 2];

        //heavy assumption that meta files are going to right after the regular files.
        for (int i = 0; i < files.Length; i += 2) {
            StreamReader inp_stm = new StreamReader(files[i]);
            string category_name = inp_stm.ReadLine( );
            buttons[i / 2] = create_button(files[i], category_name, new Vector3(0, 150 - (250 * (i / 2))));
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
        trans.sizeDelta = new Vector3(1000, 200, 1);
        trans.localPosition = pos;

        Image sprite = button.GetComponent<Image>();
        sprite.sprite = inter_sprites[2];

       GameObject text_box = button.transform.GetChild(0).gameObject;
        RectTransform text_trans = text_box.GetComponent<RectTransform>();
        text_trans.localPosition = new Vector3(1, 1, 0);
        text_trans.localScale = Vector3.one;
        text_trans.sizeDelta = new Vector3(1000, 200, 1);
        TextMeshProUGUI text = text_box.GetComponent<TextMeshProUGUI>();
        text.text = category_name;
        // text.font = font;
        text.fontSize = 80;
        text.color = Color.black;
        text.alignment = TextAlignmentOptions.Center;

        return button;   
    }

    void choose_category(string file_path) {
        GameControl.file_path = file_path;
        loadingPanel.GetComponent<FadeOutPanel>().sceneIndex = 2;
        loadingPanel.SetActive(true);
    }

}
