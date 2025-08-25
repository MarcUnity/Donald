using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour {

    public float raio;
    public bool pegaItem;
    public LayerMask whatIsItem;

    void Start() {
        
    }


    void Update() {
        pegaItem = Physics2D.OverlapCircle(transform.position, raio, whatIsItem);

        if (pegaItem) {
            Debug.Log("PEGOU ITEM!");
            pegaItem = false;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, raio);
    }
}
