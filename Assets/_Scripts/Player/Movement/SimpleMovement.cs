﻿using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour
{
    private Vector3 movement;
    private float x;
    private float z; 
    private Rigidbody rigidBody;
    private float speed = 10;
    private float dash = 2.0f;
    [HideInInspector] public Transform playerPos;
    public static SimpleMovement move;
    private bool exhausted = false;
    private HandleInput input;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        input = GameObject.FindObjectOfType<HandleInput>();
        move = this;
    }

    void Update()
    {
        if (input.controller=="xbox")
        {
            x = Input.GetAxisRaw("LeftStickX");
            z = Input.GetAxisRaw("LeftStickY");
        }
        else
        {
            x = Input.GetAxisRaw ("Horizontal");
            z = Input.GetAxisRaw ("Vertical");
        }
        movement = transform.forward.normalized * z;
    }

    void FixedUpdate()
    {
        if (Stamina.stam.curStamina >= Stamina.stam.maxStamina*0.7f)
        {
            exhausted = false;
        }
        if (Stamina.stam.curStamina <= 5)
        {
            exhausted = true;
            speed = 10;
        }
        if (Input.GetButton("Sprint")&&Stamina.stam.curStamina>=5)
        {
            if (exhausted == false)
            {
                if (z > 0)
                {
                    Stamina.stam.curStamina--;
                    speed = 20;
                }
            }
        }
        else
        {
            speed = 10;
        }

        if (Input.GetButtonDown("Dash") && Stamina.stam.curStamina >=10)
        {
            Vector3 velocity = movement.normalized * dash;
            rigidBody.MovePosition(rigidBody.position + velocity);
            Stamina.stam.curStamina -= Random.Range(16.0f, 19.0f);
        }
        else
        {
            Vector3 velocity = movement * speed * Time.fixedDeltaTime;
            rigidBody.rotation = Quaternion.Euler(rigidBody.rotation.eulerAngles + new Vector3(0f, 2* x, 0f));
            rigidBody.MovePosition(rigidBody.position + velocity);
        }
    }
}