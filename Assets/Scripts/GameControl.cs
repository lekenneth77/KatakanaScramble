using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameControl : MonoBehaviour
{
    public GameObject choose_container;
    public GameObject ans_container;

    static Vector3[] wp;
    static GameObject[] bitter_map;
    static int count_selected;

    static bool check_flag;

    private List<string> words;
    private string file_path = "Assets/text.txt";

    private Sprite[] inter_sprites;
    private TMP_FontAsset font;
    // Start is called before the first frame update
    void Start() {
        get_words();
        foreach (string s in words) {
            Debug.Log(s);
        }
        inter_sprites = Resources.LoadAll<Sprite>("interactables/");
        TMP_FontAsset[] fonts = Resources.LoadAll<TMP_FontAsset>("SU3DJPFont/TextMeshProFont/Selected/");
        font = fonts[0];
        setup_game();
    }

    private void get_words() {
        words = new List<string>();
        StreamReader inp_stm = new StreamReader(file_path);
        while(!inp_stm.EndOfStream){
            string inp_ln = inp_stm.ReadLine( );
            if (inp_ln == "" || inp_ln == "\n") {
                continue;
            }
            words.Add(inp_ln);
        }
        inp_stm.Close( );  
    }

    private void setup_game() {
        //going to need a cleanup_game too probably to destroy gameobjects
        //randomly choose a word
        const int MAX_WORDS_ROW = 6;
        string word = words[Random.Range(0, words.Count)];
        List<int> pos = new List<int>();
        int change_y = 0;

        //create interactables
        for (int i = 0; i < word.Length; i++) {
            change_y = i < MAX_WORDS_ROW ? 0 : 225;
            if (i == MAX_WORDS_ROW) {
                pos.Clear();
            }
            GameObject inter = new GameObject("inter" + (i + 1));
            RectTransform trans = inter.AddComponent<RectTransform>();
            trans.transform.SetParent(choose_container.transform);
            trans.localScale = new Vector3(35, 35, 1);
            trans.sizeDelta = new Vector3(1, 1, 1);
            int rand_cap = i >= (word.Length - MAX_WORDS_ROW) ? (word.Length % MAX_WORDS_ROW) : MAX_WORDS_ROW;
            int x_multiplier = -1;
            while (x_multiplier == -1) {
                int random = Random.Range(0, rand_cap);
                if (!pos.Contains(random)) {
                    pos.Add(random);
                    x_multiplier = random;
                }
            }
            SpriteRenderer sprite = inter.AddComponent<SpriteRenderer>();
            sprite.sprite = inter_sprites[0];
            trans.localPosition = new Vector3(-700 + (225 * (x_multiplier )), 150 - change_y, 1);   

            GameObject letter = new GameObject("letter");
            RectTransform letter_trans = letter.AddComponent<RectTransform>();
            letter_trans.transform.SetParent(inter.transform);
            //????????????? WHY -1????
            letter_trans.localPosition = new Vector3(0, 0, -1);
            letter_trans.localScale = Vector3.one;
            letter_trans.sizeDelta = new Vector3(1, 1, 1);
            letter.transform.SetParent(inter.transform);
            TextMeshProUGUI text = letter.AddComponent<TextMeshProUGUI>();
            text.text = word[i] + "";
            text.fontSize = 4;
            text.font = font;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;    
            
            ClickLetter click = inter.AddComponent<ClickLetter>();
            click.set_id(i);
            inter.AddComponent<BoxCollider>();

            inter.transform.SetParent(choose_container.transform);
        }

        //set wps
        wp = new Vector3[word.Length];
        for (int i = 0; i < word.Length; i++) {
            change_y = i < MAX_WORDS_ROW ? 0 : 250;
            int x_multiplier = i < MAX_WORDS_ROW ? i : i - MAX_WORDS_ROW; 
            wp[i] = new Vector3(-700 + (200 * x_multiplier), 600 - change_y, 1);
        }
        

        count_selected = 0;
        check_flag = false;
        bitter_map = new GameObject[wp.Length];
    }

    public static Vector3 available_pos(GameObject that) {
        for (int i = 0; i < wp.Length; i++) {
            if (bitter_map[i] == null) {
                Debug.Log("Found Position: " + i);
                bitter_map[i] = that;
                count_selected++;
                if (count_selected >= wp.Length) {
                    check_flag = true;
                }
                return wp[i];
            }
        }
        //shouldn't get here
        return new Vector3();
    }

   

    public static void remove_pos(GameObject that) {
        for (int i = 0; i < wp.Length; i++) {
            if (GameObject.ReferenceEquals(bitter_map[i], that)) {
                // Debug.Log("Removed!");
                bitter_map[i] = null;
                count_selected--;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (check_flag) {
            check_flag = false;
            StartCoroutine("check_corr");
        }
        
    }

    private IEnumerator check_corr() {
        Debug.Log("Checking!");
        List<int> incorrect = new List<int>();
        for (int i = 0; i < wp.Length; i++) {
            ClickLetter letter = bitter_map[i].GetComponent<ClickLetter>();
            letter.set_disable(true);
            Color tmp = bitter_map[i].GetComponent<SpriteRenderer>().color;
            tmp.a = 0.5f;
            bitter_map[i].GetComponent<SpriteRenderer>().color = tmp;
            if (letter.get_id() != i) {
                incorrect.Add(i);
            }
        }
        yield return new WaitForSeconds(1f);
        if (incorrect.Count == 0) {
            Debug.Log("Correct!");
        } else {
            Debug.Log("incorrect");
            foreach (int i in incorrect) {
                Color tmp = bitter_map[i].GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                bitter_map[i].GetComponent<SpriteRenderer>().color = tmp;
                ClickLetter letter = bitter_map[i].GetComponent<ClickLetter>();
                letter.set_disable(false);
                letter.return_home();
                bitter_map[i] = null;
                count_selected--;
            }
        }
    }
}
