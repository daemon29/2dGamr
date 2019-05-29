using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))] 
public class TapControl : MonoBehaviour {
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public AudioSource Score_sound, Die_sound, Tap_sound;
    GameManager game;
    public float tap_force = 10;
    public float tillSmoth = 2;
    public Vector3 Startpos;
    Rigidbody2D rigidbody;
    Quaternion DownRot;
    Quaternion ForwRot;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        DownRot = Quaternion.Euler(0, 0, -60);
        ForwRot = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        rigidbody.simulated = true;

    }
    void OnEnable()
    {
        GameManager.OnGameStart += OnGameStart;
        GameManager.OnGameOver += OnGameOver;

    }
    void OnDisable()
    {
        GameManager.OnGameStart -= OnGameStart;
        GameManager.OnGameOver -= OnGameOver;
    }
    void OnGameStart()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }
    void OnGameOver()
    {
        transform.localPosition = Startpos;
        transform.rotation = Quaternion.identity;
    }
    // Update is called once per frame
    void Update () {
        if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0))
        {
            Tap_sound.Play();
            transform.rotation = ForwRot;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tap_force,ForceMode2D.Force/*impule*/);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, DownRot, tillSmoth*Time.deltaTime);
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            Die_sound.Play();
            rigidbody.simulated = false;
            OnPlayerDied();
        }
        if (collision.gameObject.tag == "Score")
        {
            Score_sound.Play();
            OnPlayerScored();
        }
    }
}
