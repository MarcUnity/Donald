using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public Transform shotPrefab;
    public float shootingRate;
    public float shootCooldown;
    public Rigidbody2D rbPlug;
    public float speedPlug;

    private Transform pontoTiro;
    private PlayerController playerController;

    void Awake() {
        pontoTiro = transform.Find("firePoint");
        playerController = GetComponent<PlayerController>();
    }

    void Start() {
        shootCooldown = 0;
    }

    void Update() {
        if (shootCooldown > 0) {
            shootCooldown -= Time.deltaTime;
        }
    }

    public void Desentupidor() {
        if (shootCooldown <= 0) {
            shootCooldown = shootingRate;

            if (playerController.paraDireita) {
                Rigidbody2D bulletInstance = Instantiate(rbPlug, pontoTiro.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                bulletInstance.linearVelocity = new Vector2(speedPlug, 0);
            }
            else {
                Rigidbody2D bulletInstance = Instantiate(rbPlug, pontoTiro.position, Quaternion.Euler(new Vector3(0, 0, 180f))) as Rigidbody2D;
                bulletInstance.linearVelocity = new Vector2(-speedPlug, 0);
            }

            //Debug.Log("Atirou desentupidor!");
        }
    }

    public void Pipoca() {
        if (shootCooldown <= 0) {
            shootCooldown = shootingRate;

            var shotTransform = Instantiate(shotPrefab) as Transform;
            shotTransform.position = pontoTiro.position;

            Debug.Log("Atirou pipoca!");
        }
    }

    public void Bolha() {
        if (shootCooldown <= 0) {
            shootCooldown = shootingRate;

            var shotTransform = Instantiate(shotPrefab) as Transform;
            shotTransform.position = pontoTiro.position;

            Debug.Log("Atirou bolha!");
        }
    }
}