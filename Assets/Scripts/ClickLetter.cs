using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickLetter : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    bool chosen;
    bool disable;
    public bool move_flag;
    public Vector3 original_pos;
    public Vector3 new_pos;
    void Start()
    {
        disable = false;
        chosen = false;
        original_pos = this.gameObject.transform.localPosition;
    }

    private void OnMouseDown() {
        // Debug.Log("Test!");
        if (!disable) {
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
            Debug.Log("Hey!");
        }

    }
}
