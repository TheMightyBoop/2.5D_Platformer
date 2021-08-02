using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {

    public Rigidbody Rigidbody;
    public float ForceMagnitude;

    void Update() {

        if (Input.GetKey(KeyCode.A)) {
            if (Rigidbody.velocity.x > -5f) {
                Rigidbody.velocity += -Vector3.right * Time.deltaTime * ForceMagnitude;
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            if (Rigidbody.velocity.x < 5f) {
                Rigidbody.velocity += Vector3.right * Time.deltaTime * ForceMagnitude;
            }
        }

        if (Input.GetKey(KeyCode.S)) {
            if (Rigidbody.velocity.y < 0f) {
                Rigidbody.velocity += -Vector3.up * Time.deltaTime * ForceMagnitude * .1f;
            }
        }

    }
}
