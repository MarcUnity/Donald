using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayer : MonoBehaviour {

	public int vidas;
	public int saude;
	public int temperamento;
	public float speed;
	public float forcaPulo;
	public float forcaArrasto;
	public float forcaAndar;
	public float tempoPiscar;
	public float duracaoInvulneravel;
	public float tempoInvulneravel;
	private float inicio = 0.05f;
	public Transform chaoVerificador;
	public GameObject shotPrefab;
	public bool estaNoChao;
	public bool paraDireita;
	public bool paraCima;
	public bool estaAndando;
	public bool estaAtirando;
	public bool estaAbaixado;
    public bool olhaParaCima;
    public bool pulou;
	public bool pulouEmMovimento;
	public bool invulneravel;
	public bool pisca;
	public bool arrastou;
	public bool ficouImpaciente;
	public float tempoImpaciente;
	public LayerMask groundLayer;

	private SpriteRenderer sprite;
	private Rigidbody2D rigidB;
	private Animator playerAnimator;
	private Transform firePoint;

	void Start() {
		rigidB = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		firePoint = GameObject.Find("firePoint").transform;
		sprite = GetComponent<SpriteRenderer>();

		paraDireita = true;
		paraCima = false;
        olhaParaCima = false;
        invulneravel = false;
		pisca = false;
        ficouImpaciente = false;
    }

	void Update() {
		float horizontal = Input.GetAxis("Horizontal");
		float velocidadeY = rigidB.linearVelocity.y;


		if (horizontal != 0) {
			estaAndando = true;
			tempoImpaciente = 0;
		}
		else {
			estaAndando = false;
			tempoImpaciente += Time.deltaTime;
			
		}
		
		if (!estaAtirando) {
			rigidB.linearVelocity = new Vector2(horizontal * speed, rigidB.linearVelocity.y);
		}


		if (horizontal > 0 && !paraDireita || horizontal < 0 && paraDireita) {
			Flip();
		}

		playerAnimator.SetFloat("speedY", velocidadeY);
		playerAnimator.SetFloat("anda", Mathf.Abs(horizontal));


		// ======== PULANDO PARADO OU EM MOVIMENTO ========
		if (Input.GetButtonDown("Jump") && estaNoChao && !estaAtirando && !estaAbaixado) {
			pulou = true;
			
		}

        // ======== OLHA PARA CIMA ========
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			olhaParaCima = true;
        }
		else {
			olhaParaCima = false;
		}

		// === ABAIXA ===
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) && estaNoChao && !estaAndando) {
			estaAbaixado = true;
			playerAnimator.SetBool("abaixa", true);
			GetComponent<CapsuleCollider2D>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = true;
			firePoint.localPosition = new Vector2(0.881f, -0.489f);

			// === ARRASTA ===
			if (Input.GetButtonDown("Jump")) {
				arrastou = true;
				playerAnimator.SetTrigger("arrasta");
				//rigidB.AddForce(new Vector2(0, forcaPulo));
			}
		}
		else {
			estaAbaixado = false;
			playerAnimator.SetBool("abaixa", false);
			GetComponent<CapsuleCollider2D>().enabled = true;
			GetComponent<PolygonCollider2D>().enabled = false;
			firePoint.localPosition = new Vector2(0.579f, 0);
		}

		playerAnimator.SetBool("chao", estaNoChao);


		if (tempoImpaciente >= 3 && !ficouImpaciente) {
            //playerAnimator.SetTrigger("impaciente");
            ficouImpaciente = true;
		}
		else {
            ficouImpaciente = false;
        }

        // === ATIRA NO CHAO, PARA CIMA E ABAIXADO ===
        if (Input.GetKeyDown(KeyCode.N) && estaNoChao && !estaAtirando) {
            if (!estaAbaixado && !olhaParaCima) {
                estaAtirando = true;
                rigidB.constraints = RigidbodyConstraints2D.FreezePosition;
                playerAnimator.SetTrigger("atira");
                //weapon.Desentupidor();
            }
            else if (!estaAbaixado && olhaParaCima) {
                estaAtirando = true;
                playerAnimator.SetTrigger("atiraPraCima");
            }
			else if (estaAbaixado && !olhaParaCima) {
                estaAtirando = true;
                playerAnimator.SetTrigger("atiraBaixo");
                //weapon.Desentupidor();
            }
            //shotAnimator.SetTrigger("disparo");
        }
        // === ATIRA ENQUANTO PULA ===
        else if (Input.GetKeyDown(KeyCode.N) && !estaNoChao && !estaAtirando) {
            estaAtirando = true;
            playerAnimator.SetTrigger("atiraPulando");
        }
    }

	void FixedUpdate() {
		// Verifica se estamos no chao - se nao, entao estamos caindo.
		estaNoChao = Physics2D.OverlapCircle(chaoVerificador.position, 0.02f);

		if (pulou) {
			rigidB.AddForce(new Vector2(0, forcaPulo));
			pulou = false;
		}

		if (rigidB.linearVelocity.y > 0f && !Input.GetButton("Jump")) {
			rigidB.linearVelocity += Vector2.up * -0.8f;
		}

		if (pisca) {
			PiscarSprite();
		}

		FicarInvulneravel();
	}

	void Flip() {
		paraDireita = !paraDireita;
		float x = transform.localScale.x;
		x *= -1;
		transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag.Equals("enemy")) {
			if (!invulneravel) {
				invulneravel = true;
				playerAnimator.SetTrigger("hit");
				rigidB.constraints = RigidbodyConstraints2D.FreezePosition;
			}
		}

        if (col.gameObject.tag.Equals("item")) {
			//
        }
	}

	public void FicarInvulneravel() {
		if (invulneravel) {
			tempoInvulneravel += Time.deltaTime;
		}

		if (tempoInvulneravel >= duracaoInvulneravel) {
			invulneravel = false;
			pisca = false;
			tempoInvulneravel = 0;
			sprite.enabled = true;
		}
	}

	public void PiscarSprite() {
		tempoPiscar -= Time.deltaTime;

		if (tempoPiscar <= 0) {
			sprite.enabled = !sprite.enabled;
			tempoPiscar = inicio;
		}
	}

	public void AoEncerrarAtaque() {
		estaAtirando = false;
		rigidB.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void OnHitEnded() {
		pisca = true;
		rigidB.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	public void OnDragEnded() {
		arrastou = false;
	}

	public void Impatient() {
		playerAnimator.SetTrigger("impaciente");
	}
}
