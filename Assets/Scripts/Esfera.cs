using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour {

    public float forcaMovimento;
    public float speed;
    private Rigidbody2D rBody;


    void Start() {
        rBody = GetComponent<Rigidbody2D>();
    }


    void Update() {
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        //rBody.velocity = new Vector2(horiz * speed, vert * speed);


        
        if (Input.GetAxis("Horizontal") > 0) {
            rBody.AddForce(new Vector2(forcaMovimento, 0));
        }

        if (Input.GetAxis("Horizontal") < 0) {
            rBody.AddForce(new Vector2(-forcaMovimento, 0));
        }
    }
}