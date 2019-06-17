using System;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour
{
    public float flapSpeed = 700f;
    public float forwardSpeed = 5f;
    public float maxJumpSpeed = 2f;

    public AudioClip flapSound;
    public AudioClip fallSound;
    public AudioClip pointSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isFlapped;
    private bool isDead;

    private bool gameStarted;

    private Vector2 originalBirdPosition;
    private GameObject startButton;
    
    public Texture2D scoreTexture;
    public Texture2D highScoreTexture;

    public GameObject quitButton;
    
    private int score = 0;

    public void Start()
    {
        this.forwardSpeed = 0;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.audioSource = this.GetComponent<AudioSource>();

        this.startButton = GameObject.Find("StartButton");
        this.originalBirdPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        this.rb.gravityScale = 0;

        this.animator.enabled = false;

       
    }

    public void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isDead)
            {

                if (!this.gameStarted)
                {
                    var renderer = startButton.GetComponent<SpriteRenderer>();
                    renderer.enabled = false;

                    this.forwardSpeed = 4;
                    this.rb.gravityScale = 1;
                    this.animator.enabled = true;
                    
                }

                isFlapped = true;
                this.audioSource.PlayOneShot(this.flapSound);
            }

            else
            {
                Application.LoadLevel("SampleScene");
            }
            
        }

    }

    public void FixedUpdate()
    {
        var velocity = this.rb.velocity;

        velocity.x = this.forwardSpeed;
        this.rb.velocity = velocity;

        if (this.rb.velocity.y > 0)
        {
            this.rb.MoveRotation(30);
        }
        else if (!isDead)
        {
            var angle = velocity.y * 5;

            if (angle < -90)
            {
                angle = -90;
            }

            this.rb.MoveRotation(velocity.y * 5);
        }

        if (isFlapped)
        {
            isFlapped = false;

            this.rb.AddForce(new Vector2(0, flapSpeed), ForceMode2D.Impulse);

            var updatedVelocity = this.rb.velocity;

            if (updatedVelocity.y > this.maxJumpSpeed)
            {
                updatedVelocity.y = this.maxJumpSpeed;
                this.rb.velocity = updatedVelocity;
            }

        }
    }

    public void OnGUI()
    {
        this.DisplayScore();
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Floor") || collider.gameObject.CompareTag("PipeCollision"))
        {
            this.isDead = true;

            quitButton.gameObject.SetActive(true);

            this.audioSource.PlayOneShot(this.fallSound);
            this.animator.SetBool("BirdIsDead", true);
            this.forwardSpeed = 0;

            var renderer = startButton.GetComponent<SpriteRenderer>();
            renderer.enabled = true;


            var startButtonX = Camera.main.transform.position.x;
            var startButtonY = Camera.main.transform.position.y;

            var startBtnPosition = this.startButton.transform.position;
            startBtnPosition.x = startButtonX;
            startBtnPosition.y = startButtonY;

            this.startButton.transform.position = startBtnPosition;
            
            var quitButtonX = Camera.main.transform.position.x;
            var quitButtonY = Camera.main.transform.position.y - 2.2f;

            var quitBtnPosition = this.quitButton.transform.position;
            quitBtnPosition.x = quitButtonX;
            quitBtnPosition.y = quitButtonY;

            this.quitButton.transform.position = quitBtnPosition;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pipe"))
        {
            this.score++;
            this.audioSource.PlayOneShot(this.pointSound);
        }

    }

    private void DisplayScore()
    {
        var rect = new Rect(10, 10, 60, 60);
        GUI.Label(rect, scoreTexture);

        var scoreStyle = new GUIStyle();

        scoreStyle.fontSize = 80;
        scoreStyle.fontStyle = FontStyle.Bold;
        scoreStyle.normal.textColor = Color.black;

        GUI.Label(rect, this.score.ToString(), scoreStyle);
    }

    
}
