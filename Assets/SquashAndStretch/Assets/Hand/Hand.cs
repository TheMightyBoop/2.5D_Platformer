using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public AudioSource AudioSource;
    public GameObject AnimatedHand;

    private void OnTriggerEnter(Collider other) {
        AudioSource.Play();
        Destroy(gameObject, 0.1f);
    }

}
