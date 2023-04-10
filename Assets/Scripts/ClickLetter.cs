using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickLetter : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    bool chosen;
    bool disable;
    public bool move_flag = false;
    public Vector3 original_pos;
    public Vector3 new_pos;
    void Start()
    {
        disable = false;
        chosen = false;
    }

    public void set_new_pos(Vector3 pos) {
        new_pos = pos;
    }

    public void set_orig_pos(Vector3 pos) {
        original_pos = pos;
    }

    public void set_move_flag(bool move) {
        move_flag = move;
    }

    private void OnMouseDown() {
        if (!disable && !move_flag) {
            GameControl.on_hover.SetActive(false);
            if (chosen) {
                GameControl.remove_pos(this.gameObject);
                new_pos = original_pos;
                move_flag = true;
                chosen = false;
            } else {
                new_pos = GameControl.available_pos(this.gameObject);
                move_flag = true;
                chosen = true;
            }
        }
    }

    private void OnMouseOver() {
        if (!move_flag) {
            if (chosen) {
                GameControl.on_hover.transform.localPosition = original_pos;
                GameControl.on_hover.SetActive(true);
            } else {
                GameControl.on_hover.transform.localPosition = GameControl.hover_avail_pos();
                GameControl.on_hover.SetActive(true);
            }
        }
    }

    private void OnMouseExit() {
        GameControl.on_hover.SetActive(false);
    }

    public void set_disable(bool val) {
        disable = val;
        chosen = false; //lmao kinda lazy
    }

    public void set_id(int val) {
        id = val;
    }
    public int get_id() {
        return id;
    }

    public Vector3 get_pos() {
        return original_pos;
    }

    public void return_home() {
        new_pos = original_pos;
        move_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (move_flag) {
            move();
        }
        
    }

    void move() {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, new_pos, 2000f * Time.deltaTime);
        if (Vector2.Distance(transform.localPosition, new_pos) < 0.1f) {
            move_flag = false;
        }

    }
}
