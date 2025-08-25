using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour {

    public GameObject player;   // Variavel publica para armazenar uma referencia ao game object jogador.

    private Vector3 offset;     // Variavel privada para armazenar a distancia de deslocamento entre o player e a camera.

    void Start()
    {
        // Calcula e armazena o valor de deslocamento, obtendo a distancia entre
        // a posiçao do player e a posiçao da camera.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Define a posiçao do transform da camera para ser a mesma que a do jogador,
        // mas compensada pela distancia de deslocamento calculada.
        transform.position = player.transform.position + offset;
    }
}
