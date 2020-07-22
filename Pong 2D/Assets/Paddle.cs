using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float speed;
    float height;

    private string input;
    public bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;
        //speed = 5f; // some constant value
    }

    public void Init(bool isRightPaddle)
    {
        isRight = isRightPaddle; 

        Vector2 pos = Vector2.zero;

        if (isRightPaddle)
        {
            // if it is a right paddle place on right of the screen
            // however this moves the center of the paddle to the right.
            pos = new Vector2(GameManager.topRight.x, 0);

            // we want the right edge to be at the right, so we remove the right hand side of the position vector with the width of x (x axis is right/left plane)
            pos -= Vector2.right * transform.localScale.x;

            this.input = "PaddleRight";
        }
        else
        {
            // else it is a left paddle place on left of the screen
            // however this moves the center of the paddle to the left.
            pos = new Vector2(GameManager.bottomLeft.x, 0);

            // we want the left edge to be at the left, so we add the left hand side of the position vector with the width of x (x axis is right/left plane)
            pos += Vector2.right * transform.localScale.x;

            this.input = "PaddleLeft";
        }

        // Update this paddles position
        // Every game object has a transform, here we set transform to the position relative to the screen.
        transform.position = pos;

        transform.name = this.input;
    }

    // Update is called once per frame
    void Update()
    {
        // To move the paddle

        // GetAxis is a number between -1 and 1 (-1 for down, 1 for up)
        float move = Input.GetAxis(this.input) * Time.deltaTime * this.speed; // this '* Time.deltaTime' makes input time independent. So if there is lag, it wont slow down.

        // restrict movement
        // If paddle is too high up, and user continues to move up, stop movement (set move = 0)
        if (transform.position.y < (GameManager.bottomLeft.y + this.height / 2) && move < 0)
        {
            // 'move < 0' because moving down is indicated by negative move
            // '+ this.height / 2' because we want the bottom to not go below the minimum limit
            move = 0;
        }
        
        // If paddle is too low down, and user continues to move down, stop movement (set move = 0)
        if (transform.position.y > (GameManager.topRight.y - this.height / 2) && move > 0)
        {
            // 'move < 0' because moving up is indicated by positive move
            // '+ this.height / 2' because we want the bottom to not go below the minimum limit
            move = 0;
        }

        transform.Translate(move * Vector2.up);
    }
}
