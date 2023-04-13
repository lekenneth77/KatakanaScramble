using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumble : MonoBehaviour
{

    float smooth = 10.0f;
    float tiltAngle = 60.0f;
    bool trigger;
    // Start is called before the first frame update
    void Start()
    {
        trigger = true;
    }

    IEnumerator rumble() {
        yield return new WaitForSeconds(1f);
        for (int jerks = 0; jerks < 2; jerks++) {
            Quaternion rot = transform.rotation;
            for (float i = 0.2f; i < 0.3f; i += 0.005f) {
                float tiltAroundZ =  i * tiltAngle;
                // Rotate the cube by converting the angles into a quaternion.
                Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

                // Dampen towards the target rotation
                transform.rotation = Quaternion.Slerp(rot, target,  Time.deltaTime * smooth);
                yield return new WaitForSeconds(0.05f);
            }

            rot = transform.rotation;
            for (float i = 0.2f; i < 0.24f; i += 0.005f) {
                float tiltAroundZ =  -1 * (i * tiltAngle);
                // Rotate the cube by converting the angles into a quaternion.
                Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

                // Dampen towards the target rotation
                transform.rotation = Quaternion.Slerp(rot, target,  Time.deltaTime * smooth);
                yield return new WaitForSeconds(0.05f);
            }
        }
        trigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger) {
            trigger = false;
            StartCoroutine("rumble");
        }
    }
}
