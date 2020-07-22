using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector2 direction;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        this.direction = Vector2.one.normalized; // direction is (1, 1), so magnitude is root(2), which isn't good. normalized means magnitude is also 1.
        this.radius = transform.localScale.x / 2; // diameter / 2
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.direction * this.speed * Time.deltaTime); // this '* Time.deltaTime' makes input time independent. So if there is lag, it wont slow down.

        // restrict movement
        // If paddle is too high up, and hits the wall, then invert the direction
        if (transform.position.y < (GameManager.bottomLeft.y + this.radius) && this.direction.y < 0)
        {
            // 'this.direction.y < 0' because moving down is indicated by negative move
            // '+ this.radius' because we want the bottom to not go below the minimum limit
            this.direction.y = -this.direction.y;
        }

        // If paddle is too low down, and user continues to move down, stop movement (set move = 0)
        if (transform.position.y > (GameManager.topRight.y - this.radius) && this.direction.y > 0)
        {
            // 'this.direction.y > 0' because moving up is indicated by positive move
            // '+ this.radius' because we want the bottom to not go below the minimum limit
            this.direction.y = -this.direction.y;
        }

        // Game Over
        // If paddle hits left wall, right player wins
        if (transform.position.x < (GameManager.bottomLeft.x + this.radius) && this.direction.x < 0)
        {
            Debug.Log("Right Player Wins!!");

            // Freeze time and end game
            Time.timeScale = 0;
            enabled = false; // Stops the script. I.e.: update doesn't happen.
        }

        // If paddle hits right wall, right player wins
        if (transform.position.x > (GameManager.topRight.x - this.radius) && this.direction.x > 0)
        {
            Debug.Log("left Player Wins!!");

            // Freeze time and end game
            Time.timeScale = 0;
            enabled = false; // Stops the script. I.e.: update doesn't happen.
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Paddle")
        {
            bool isRight = other.GetComponent<Paddle>().isRight;

            // this.direction.x > 0 means we are moving to the right side
            // is hitting right paddle, change direction
            if (isRight && this.direction.x > 0)
            {
                this.direction.x = -this.direction.x;
            }

            // this.direction.x < 0 means we are moving to the left side
            // is hitting left paddle, change direction
            if (!isRight && this.direction.x < 0)
            {
                this.direction.x = -this.direction.x;
            }
        }
    }
}
