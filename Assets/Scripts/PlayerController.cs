using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Singleton;

    public float force;

    [HideInInspector] public bool basicGravity = true;

    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject jumpPS;
    [SerializeField] GameObject deathPS;

    Rigidbody2D rb;
    float firstPressTime;
    bool pressed = false;
    bool onGround = false;

    private void Start()
    {
        Singleton = this;
        rb = GetComponent<Rigidbody2D>();
    }
     
    void Update()
    {
        //second press key check
        if (Input.GetKeyDown(KeyCode.Space) && pressed)
        {
            if (Time.time - firstPressTime < .2f)
            { 
                SwapGravity();
            }

            pressed = false;
        }

        //jump + detecting ground
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
            firstPressTime = Time.time;

            if (!basicGravity)
                SwapGravity();
            else
                pressed = true;
        }
    }

    public void Jump()
    {
        //Spawn particles as a child of second part in list
        int gravIdx = basicGravity ? -1 : 1;
        Debug.Log(basicGravity);
        var temp = Instantiate(jumpPS, transform.position + gravIdx * new Vector3(0, .5f, 0), jumpPS.transform.rotation);
        temp.transform.parent = LevelGenerator.Singleton.transform.GetChild(1).transform;
        //jump
        rb.velocity = Vector2.up * force;
    }

    public void SwapGravity()
    {
        rb.gravityScale = -rb.gravityScale;
        basicGravity = !basicGravity;
    }

    private void GameOver()
    {
        Instantiate(deathPS, transform.position, deathPS.transform.rotation);
        LevelGenerator.Singleton.GameOver();
        gameOverMenu.SetActive(true);
        Destroy(gameObject);
    }

    // ---- collisions ----
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Check if on groud
        if (collision.gameObject.CompareTag("Ground"))
            onGround = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if on groud
        if (collision.gameObject.CompareTag("Ground"))
            onGround = true;
        //check if touch spikes
        if (collision.gameObject.CompareTag("Spikes"))
            GameOver();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //check if leave ground
        if (collision.gameObject.CompareTag("Ground"))
            onGround = false;
    }
}
