using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Transform chaoVerificador;
    public float duracaoPiscar;
    public float tempoPiscar;
    public float forcaPulo;
    public float velocidade;
    public bool _pisca;
    public bool estaNoChao;
    public bool estaAndando;
    public bool estaAbaixado;
    public bool paraDireita;
    public bool estaAtacando;
    public bool arrastou;
    public bool tomaDano;
    public bool vulneravel;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private WeaponScript weapon;
    private WeaponScript arma2;
    private WeaponScript arma3;

    void Start() {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        vulneravel = true;
        paraDireita = true;

        weapon = GetComponent<WeaponScript>();
    }

    IEnumerator VoltarVulneravel() {
        yield return new WaitForSeconds(3.0f);
        vulneravel = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update() {
        /*
        float horiz = Input.GetAxisRaw("Horizontal");
        estaAndando = Mathf.Abs(horiz) > 0f;
        */

        float horiz = Input.GetAxis("Horizontal");
        playerAnimator.SetFloat("anda", Mathf.Abs(horiz));

        playerRb.linearVelocity = new Vector2(horiz * velocidade, playerRb.linearVelocity.y);

        float speedY = playerRb.linearVelocity.y;

        if ((horiz > 0 && !paraDireita) || (horiz < 0 && paraDireita)) {
            Flip();
        }

        // === ATIRA NO CHAO, NO AR E ENQUANTO ESTÁ ABAIXADO ===
        if (Input.GetKeyDown(KeyCode.N) && estaNoChao && !estaAtacando && !estaAbaixado) {
            estaAtacando = true;
            playerRb.linearVelocity = new Vector2(0, 0);
            playerAnimator.SetTrigger("atira");
            weapon.Desentupidor();
        }

        if (Input.GetKeyDown(KeyCode.N) && estaNoChao && !estaAtacando && estaAbaixado && !arrastou) {
            //estaAtacando = true;
            playerAnimator.SetTrigger("atiraBaixo");
            weapon.Desentupidor();
        }

        // == PULA ===
        if (Input.GetKeyDown(KeyCode.M) && estaNoChao && !estaAtacando && !estaAbaixado) {
            playerRb.AddForce(new Vector2(0, forcaPulo));
        }

        // === ABAIXA ===
        if (Input.GetKey(KeyCode.DownArrow) && estaNoChao && !estaAtacando && !estaAndando) {
            estaAbaixado = true;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = false;

            if (Input.GetKeyDown(KeyCode.M)) {
                arrastou = true;
                playerAnimator.SetTrigger("arrasta");
            }
        }
        else {
            estaAbaixado = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        // === CORRE ===
        if (Input.GetKey(KeyCode.B)) {
            playerAnimator.speed = 1.5f;
            velocidade = 5;
        }
        else {
            playerAnimator.speed = 1f;
            velocidade = 4;
        }


        if (!estaAtacando && !tomaDano) {
            playerRb.linearVelocity = new Vector2(horiz * velocidade, speedY);
        }

        if (!vulneravel) {
            Piscar();
        }

        playerAnimator.SetBool("abaixou", estaAbaixado);
        playerAnimator.SetBool("chao", !estaNoChao);
        //playerAnimator.SetInteger("h", (int)horiz);
        //playerAnimator.SetFloat("anda", Mathf.Abs(horiz));
    }

    void FixedUpdate() {
        estaNoChao = Physics2D.OverlapCircle(chaoVerificador.position, 0.02f);
    }

    void Flip() {
        paraDireita = !paraDireita;
        float x = transform.localScale.x;
        x =- x;     // o mesmo que: x *= -1;  o operador =- inverte o valor da variavel.
        transform.localScale = new Vector3(x, transform.localScale.y);
    }
    
    public void AoEncerrarAtaque() {
        estaAtacando = false;
    }

    public void AoEncerrarHit() {
        tomaDano = false;
        vulneravel = false;
    }

    public void AoAcabarArrasta() {
        arrastou = false;
        estaAtacando = false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag.Equals("inimigo")) {
            if (vulneravel) {
                tomaDano = true;
                playerRb.linearVelocity = new Vector2(0, 0);
                playerAnimator.SetTrigger("hit");
                StartCoroutine(VoltarVulneravel());
            }
        }

        if (col.gameObject.tag.Equals("coletavel")) {
            //
        }
    }

    void Piscar() {
        tempoPiscar += Time.deltaTime;

        if (tempoPiscar >= duracaoPiscar) {
            tempoPiscar = 0;

            if (_pisca) {
                _pisca = false;
            }
            else {
                _pisca = true;
            }
        }

        if (_pisca) {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}