using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(0,0);
    //public float groundFriction = 2.0f;
    //public Float airFriction = 1.0f;
    public float gravity = 0.1f;
    public float airResistance = 0.2f;

    public float acceleration = 0.25f;
    public float jumpAcceleration = 0.1f;
    public float walkSpeed = 0.5f;
    public float jumpSpeed = 0.1f;

    public int walkMode = 0;

    public float lBound = -20f, rBound = 20f;
    public float dBound = -8f, uBound = 8f;

    public bool isJumping = false;
    public int maxJumpTime = 30;
    public int jumpTime;

    public float minMoveSpeed = 0.00001f;

    public Sprite[] sprites = new Sprite[18];
    public SpriteRenderer spriteRenderer;
    public int animationFrame = 0;
    public int frameDelay1 = 4, frameDelay2 = 9;
    public int delay;

    // Start is called before the first frame update
    void Start()
    {
        speed = Vector2.zero;
    }    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           // pressDelay = 12;
            walkMode = 1 - walkMode;
        }
        //if (pressDelay > 0) pressDelay--;

        if(walkMode == 0)
        {
            if (Input.GetKey(KeyCode.RightArrow)) speed.x = walkSpeed;
            if (Input.GetKey(KeyCode.LeftArrow)) speed.x = -1 * walkSpeed;

            if (Input.GetKeyDown(KeyCode.UpArrow)) speed.y = jumpSpeed;
        }else if(walkMode == 1){
            if (Input.GetKey(KeyCode.RightArrow)) speed.x += acceleration;
            if (Input.GetKey(KeyCode.LeftArrow)) speed.x -= acceleration;

            
            if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.y == dBound)
            {
                isJumping = true;
                jumpTime = maxJumpTime;
            }
            if (isJumping)
            {
                speed.y = jumpAcceleration;
                if (Input.GetKeyUp(KeyCode.UpArrow) || jumpTime == 0) isJumping = false;
                jumpTime--;
            }

            
        }

        if (transform.position.y <= dBound && speed.y < 0) speed.y = 0;

        speed.y -= gravity;
        speed.x -= airResistance * speed.x;
        speed.y -= airResistance * speed.y;

        //if (Mathf.abs(speed.x) < minMoveSpeed) speed.x = 0;
        //if (Mathf.abs(speed.y) < minMoveSpeed) speed.y = 0;

        var pos = transform.position;
        pos.x += speed.x;
        pos.y += speed.y;
        pos.x = Mathf.Clamp(pos.x, lBound, rBound);
        pos.y = Mathf.Clamp(pos.y, dBound, uBound);
        transform.position = pos;

        //rendering sprites
        if(delay == 0)
        {
            if (Mathf.Abs(speed.x) < minMoveSpeed)
            {
                if (walkMode == 0) spriteRenderer.sprite = (speed.x >= 0) ? sprites[0] : sprites[4];
                else if (walkMode == 1) spriteRenderer.sprite = (speed.x >= 0) ? sprites[10] : sprites[14];
            }
            else
            {
                if (transform.position.y <= -8) animationFrame++;
                animationFrame %= 4;


                if (walkMode == 0) spriteRenderer.sprite = (speed.x >= 0) ? sprites[0 + animationFrame] : sprites[4 + animationFrame];
                else if (walkMode == 1) spriteRenderer.sprite = (speed.x >= 0) ? sprites[10 + animationFrame] : sprites[14 + animationFrame];
            }

            delay = (walkMode == 0) ? frameDelay1 : frameDelay2;
        }
        delay--;
    }
}
