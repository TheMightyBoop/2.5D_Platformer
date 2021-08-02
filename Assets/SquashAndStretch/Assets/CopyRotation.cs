using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour {

    public Transform CopyFrom;

    void LateUpdate() {
        transform.rotation = CopyFrom.rotation;
    }

}
