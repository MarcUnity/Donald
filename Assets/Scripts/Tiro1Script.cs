using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro1Script : MonoBehaviour {

    public float speedBala;
    private Rigidbody2D rbBala;

    void Start() {
        rbBala = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rbBala.linearVelocity = transform.right * speedBala;
    }

    void OnBecameInvisible() {
        Destroy(transform.gameObject);
    }
}