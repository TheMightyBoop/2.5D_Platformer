using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour {
    
    public Transform Target;
    public float Speed = 10f;
    
    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * Speed);
    }
    
}
