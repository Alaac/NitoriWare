﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlanePaperPlane : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float turningSpeed = 0.1f;
    [SerializeField]
    float maxTurningSpeed = 0.5f;
    [SerializeField]
    float spriteChangeThreshold = 0.5f;
    [SerializeField]
    float delay = 0.5f;
    [SerializeField]
    Vector2 upperBounds = new Vector2(-7, 7);
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    Vector2 velocity;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);
        velocity = Vector2.zero;
        Invoke("setVelocity", delay);
	}
	
    void setVelocity()
    {
        spriteRenderer.gameObject.SetActive(true);
        velocity = new Vector2(speed, 0);
        spriteRenderer.sprite = sprites[1];
    }

    void Update()
    {
        if (velocity != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity.y += -turningSpeed * Time.deltaTime;
                clampSpeed();
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity.y += turningSpeed * Time.deltaTime;
                clampSpeed();
            }
            else
            {
                if (velocity.y < -0.5f)
                {
                    velocity.y -= -turningSpeed * Time.deltaTime;
                    clampSpeed();
                }
                else if (velocity.y > 0.5f)
                {
                    velocity.y -= turningSpeed * Time.deltaTime;
                    clampSpeed();
                }
                else velocity.y = 0;
            }
            
            transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0);
            if (transform.position.y < upperBounds.x) transform.position = new Vector3(transform.position.x, upperBounds.x, transform.position.z);
            else if (transform.position.y > upperBounds.y) transform.position = new Vector3(transform.position.x, upperBounds.y, transform.position.z);
        }

        if (velocity.y > spriteChangeThreshold)
            spriteRenderer.sprite = sprites[0];

        else if (velocity.y < -spriteChangeThreshold)
            spriteRenderer.sprite = sprites[2];

        else
            spriteRenderer.sprite = sprites[1];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        velocity = Vector2.zero;

        if (collision.gameObject.tag == "MicrogameTag1")
        {
            MicrogameController.instance.setVictory(false, true);
        }
        if (collision.gameObject.tag == "MicrogameTag2")
        {
            MicrogameController.instance.setVictory(true, true);
        }

    }

    void clampSpeed()
    {
        if (velocity.y > maxTurningSpeed)
            velocity.y = maxTurningSpeed;
        if (velocity.y < -maxTurningSpeed)
            velocity.y = -maxTurningSpeed;
    }
}
