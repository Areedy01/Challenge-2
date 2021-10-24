using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    Animator anim;
    public float speed;

    public Text score;
    public Text lives;
    public GameObject winText;
    public GameObject loseText;

    private bool facingRight = true;

    private bool gameOver;
    private int scoreValue = 0;
    private int livesValue = 3;

    public AudioClip background;
    public AudioClip winning;
    public AudioSource musicSource;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();

        anim = GetComponent<Animator>();

        musicSource.clip = background;
        musicSource.Play();
        musicSource.loop = true;

        gameOver = false;
        winText.SetActive(false);
        loseText.SetActive(false);
    } 
    

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (hozMovement < 0 || hozMovement > 0)
        {
            anim.SetInteger("State", 1);
        }
        if (hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }

        if (vertMovement < 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }
        if (vertMovement > 0 && isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

        if (scoreValue >= 8 && gameOver == false)
        {
            winText.SetActive(true);
            gameOver = true;

            musicSource.loop = false;
            musicSource.Stop();
            musicSource.clip = winning;
            musicSource.Play();

        } 
        else if (livesValue == 0)
        {
            loseText.SetActive(true);
            speed = 0;
        }

        if (Input.GetKey("escape"))
            {
                Application.Quit();
            }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue == 4)
        {
            transform.position = new Vector2(35.0f, 2.0f);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse);
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
