using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class HelicopterMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;


    [SerializeField] float turnSpeed;

    public Vector2 MoveInput { get; set; }
    public float Rotation { get; set; }

    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector2 target = MoveInput * moveSpeed;
        Vector2 force = (target - rigidbody.velocity) * acceleration;

        rigidbody.velocity += force * Time.deltaTime;
    }

    private void Turn()
    {
        rigidbody.rotation = Rotation;
    }
}
