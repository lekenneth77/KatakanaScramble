using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GameControl : MonoBehaviour
{

    public AudioSource deal_card, failure_sfx, success_sfx, buttonclick_sfx, kids_cheer;

    public GameObject next;
    public Image seacreature_img;
    public GameObject correct_img;
    public GameObject incorrect_img;
    public TextMeshProUGUI round_counter;
    public TextMeshProUGUI timer_text;
    public static GameObject on_hover;
    public GameObject congrats_con;

    public GameObject choose_container;
    public GameObject ans_container;

    const int MAX_WORDS_ROW = 8;
    const int INTER_WIDTH = 25;

    static Vector3[] wp;
    static GameObject[] bitter_map;
    static int count_selected;
    int rounds_completed = 0;
    float secs_passed = 0f;
    bool start = false;
    string cur_word;

    

    static bool check_flag;

    private List<string> words;
    private List<int> words_selected;
    // public static string file_path;
    public static string file_path = "Assets/Texts/test.txt";


    private Sprite[] inter_sprites;
    private Sprite[] seacreatures;
    private TMP_FontAsset font;
    // Start is called before the first frame update
    void Start() {
        get_words();
        setup_music();
        inter_sprites = Resources.LoadAll<Sprite>("interactables/");
        seacreatures = Resources.LoadAll<Sprite>("seacreatures/");
        seacreature_img.sprite = seacreatures[Random.Range(0, seacreatures.Length)];
        TMP_FontAsset[] fonts = Resources.LoadAll<TMP_FontAsset>("SU3DJPFont/TextMeshProFont/Selected/");
        font = fonts[0];
        rounds_completed = 0;
        timer_text.text = secs_passed.ToString("0.00");
        words_selected = new List<int>();
        next.GetComponent<Button>().onClick.AddListener(next_round);
        on_hover = GameObject.Find("on_hover");
        on_hover.SetActive(false);
        StartCoroutine("setup_game");
    }

    private void get_words() {
        words = new List<string>();
        StreamReader inp_stm = new StreamReader(file_path);
        string inp_ln = inp_stm.ReadLine( );
        while(!inp_stm.EndOfStream){
            inp_ln = inp_stm.ReadLine( );
            if (inp_ln == "" || inp_ln == "\n") {
                continue;
            }
            words.Add(inp_ln);
        }
        inp_stm.Close( );  
    }

    private IEnumerator setup_game() {
        //randomly choose a word
        bool found_new_word = false;
        int random = -1;
        while (!found_new_word) {
            random = Random.Range(0, words.Count);
            if (!words_selected.Contains(random)) {
                found_new_word = true;
                words_selected.Add(random);
            }
        }
        cur_word = words[random];
        Debug.Log(cur_word);
        //create interactables
        List<GameObject> interactables = create_interactables(cur_word);
        yield return new WaitForSeconds(0.5f);
        //set wps
        wp = new Vector3[cur_word.Length];
        int from_back = cur_word.Length - 1;
        int y_multiplier = 0;
        int x_multiplier = 0;
        for (int i = 0; i < cur_word.Length; i++) {
            if (i % MAX_WORDS_ROW == 0) {
                y_multiplier++;
                x_multiplier = 0;
            }

            wp[i] = new Vector3(-800 + (INTER_WIDTH * 6 * x_multiplier), 800 - y_multiplier * (INTER_WIDTH * 6), 1);

            interactables[from_back].GetComponent<ClickLetter>().set_move_flag(true);
            from_back--;
            if (from_back != -1) {
                interactables[from_back].transform.GetChild(0).gameObject.SetActive(true);
            }
            deal_card.Play();
            yield return new WaitForSeconds(0.3f);
            x_multiplier++;
        }

        round_counter.text = (rounds_completed + 1).ToString() + "/" + words.Count.ToString();
        count_selected = 0;
        check_flag = false;
        interactables.Clear();
        bitter_map = new GameObject[wp.Length];
        start = true;
    }

    public static Vector3 available_pos(GameObject that) {
        for (int i = 0; i < wp.Length; i++) {
            if (bitter_map[i] == null) {
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
                bitter_map[i] = null;
                count_selected--;
                return;
            }
        }
    }

    public static Vector3 hover_avail_pos() {
        for (int i = 0; i < wp.Length; i++) {
            if (bitter_map[i] == null) {
                return wp[i];
            }
        }
        return new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (check_flag) {
            check_flag = false;
            StartCoroutine("check_corr");
        }

        if (start) {
            secs_passed += Time.deltaTime;
            timer_text.text = secs_passed.ToString("0.00");
        }
        
    }

    private IEnumerator check_corr() {
        List<int> incorrect = new List<int>();
        for (int i = 0; i < wp.Length; i++) {
            ClickLetter letter = bitter_map[i].GetComponent<ClickLetter>();
            letter.set_disable(true);
            Color tmp = bitter_map[i].GetComponent<SpriteRenderer>().color;
            tmp.a = 0.5f;
            bitter_map[i].GetComponent<SpriteRenderer>().color = tmp;
            string cur_letter = bitter_map[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
            if (cur_letter != cur_word[i] + "") {
                incorrect.Add(i);
            }
        }
        yield return new WaitForSeconds(1f);
        if (incorrect.Count == 0) {
            Debug.Log("Correct!");
            start = false;
            foreach(GameObject letter in bitter_map) {
                letter.GetComponent<ClickLetter>().allow_hover = false;
            }

            correct_img.SetActive(true);
            success_sfx.Play();
            if (rounds_completed < words.Count) {
                rounds_completed++;
                if (rounds_completed >= words.Count) {
                    yield return new WaitForSeconds(2f);
                    display_finish();
                } else {
                    next.SetActive(true);
                }
            } 
        } else {
            Debug.Log("incorrect");
            failure_sfx.Play();
            incorrect_img.SetActive(true);
            yield return new WaitForSeconds(3f);
            incorrect_img.SetActive(false);

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
            deal_card.Play();
        }
    }

    private void next_round() {
        buttonclick_sfx.Play();
        if (rounds_completed != words.Count) {
            for (int i = 0; i < bitter_map.Length; i++) {
                Destroy(bitter_map[i]);
            }
            correct_img.SetActive(false);
            next.SetActive(false);
            StartCoroutine("setup_game");
        }
    }

    private void display_finish() {
        kids_cheer.Play();
        for (int i = 0; i < bitter_map.Length; i++) {
                Destroy(bitter_map[i]);
            }
        correct_img.SetActive(false);
        GameObject.Find("Pause").SetActive(false);
        GameObject.Find("RoundContainer").SetActive(false);
        congrats_con.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>()
            .text = "You finished " + rounds_completed + " rounds in "+ secs_passed.ToString("0.00") + " seconds!";
        congrats_con.SetActive(true);
    }
    
    private List<GameObject> create_interactables(string word) {
        List<GameObject> ret = new List<GameObject>();
        List<int> pos = new List<int>();
        List<int> rand_letter = new List<int>();
        int y_multiplier = 0;
        for (int i = 0; i < word.Length; i++) {
            if (i % MAX_WORDS_ROW == 0) {
                y_multiplier++;
                pos.Clear();
            }
            GameObject inter = new GameObject("inter" + (i + 1));
            RectTransform trans = inter.AddComponent<RectTransform>();
            trans.transform.SetParent(choose_container.transform);
            trans.localScale = new Vector3(INTER_WIDTH, INTER_WIDTH, 1);
            trans.sizeDelta = new Vector3(1, 1, 1);
            int rand_cap = -1;
            if (word.Length <= MAX_WORDS_ROW) {
                rand_cap = word.Length;
            } else {
                rand_cap = i >= word.Length - (word.Length % MAX_WORDS_ROW) ? (word.Length % MAX_WORDS_ROW) : MAX_WORDS_ROW;
                if (rand_cap == 0) {
                    rand_cap = MAX_WORDS_ROW;
                }
            }

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
            trans.localPosition = new Vector3(-200, 500, -1);

            GameObject letter = new GameObject("letter");
            if (i < word.Length - 1) {
                letter.SetActive(false);
            }
            RectTransform letter_trans = letter.AddComponent<RectTransform>();
            letter_trans.transform.SetParent(inter.transform);
            letter_trans.localPosition = new Vector3(0, 0, -1);  //????????????? WHY -1????
            letter_trans.localScale = Vector3.one;
            letter_trans.sizeDelta = new Vector3(1, 1, 1);
            letter.transform.SetParent(inter.transform);
            TextMeshProUGUI text = letter.AddComponent<TextMeshProUGUI>();
            int word_index = -1;
            while (word_index == -1) {
                int random = Random.Range(0, word.Length);
                if (!rand_letter.Contains(random)) {
                    rand_letter.Add(random);
                    word_index = random;
                }
            }

            text.text = word[word_index] + "";
            text.fontSize = 4;
            text.font = font;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;    
            
            ClickLetter click = inter.AddComponent<ClickLetter>();
            Vector3 vec_pos = new Vector3(-800 + (INTER_WIDTH * (6 * x_multiplier)), 300 - y_multiplier * (INTER_WIDTH * 6), -1);
            click.set_new_pos(vec_pos);
            click.set_orig_pos(vec_pos);
            click.deal_card = deal_card;
            inter.AddComponent<BoxCollider>();
            inter.transform.SetParent(choose_container.transform);

            ret.Add(inter);
        }
        return ret;
    }

    void setup_music() {
        deal_card = audio_src_comp("DealCard");
        success_sfx = audio_src_comp("Nice");
        failure_sfx = audio_src_comp("Failure");
        buttonclick_sfx = audio_src_comp("ButtonClick");
        kids_cheer = audio_src_comp("KidsCheer");
    }    
    
    AudioSource audio_src_comp(string gameobj_name) {
        return GameObject.Find(gameobj_name).GetComponent<AudioSource>();
    }

    
    
}
