using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankMovement : MonoBehaviour
{
    public Transform leftTrack;
    public Transform rightTrack;
    public Transform turret;

    [Space]
    public float maxTrackForce;
    public float trackSpeedSmoothing;
    public float trackFriction;

    [Space]
    public float turretMaxSpeed;
    public float turretSmoothTime;

    public float ThrottleInput { get; set; }
    public float TurnInput { get; set; }
    public Vector2 ShootPoint { get; set; }

    float leftTrackSpeed;
    float rightTrackSpeed;
    float turretRotation;

    float leftTrackSpeedVelocity;
    float rightTrackSpeedVelocity;
    float turretRotationVelocity;

    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RotateTurret();

        float leftTrackInput = Mathf.Clamp(ThrottleInput + TurnInput, -1.0f, 1.0f);
        float rightTrackInput = Mathf.Clamp(ThrottleInput - TurnInput, -1.0f, 1.0f);

        UpdateTrack(leftTrack, leftTrackInput, ref leftTrackSpeed, ref leftTrackSpeedVelocity);
        UpdateTrack(rightTrack, rightTrackInput, ref rightTrackSpeed, ref rightTrackSpeedVelocity);
    }

    private void RotateTurret()
    {
        Vector2 direction = ShootPoint - (Vector2)turret.position;
        float targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        turretRotation = Mathf.SmoothDampAngle(turretRotation, targetRotation, ref turretRotationVelocity, turretSmoothTime, turretMaxSpeed);
        turret.rotation = Quaternion.Euler(0.0f, 0.0f, turretRotation);
    }

    private void UpdateTrack(Transform track, float input, ref float speed, ref float speedVelocity)
    {
        speed = Mathf.SmoothDamp(speed, input * maxTrackForce, ref speedVelocity, trackSpeedSmoothing);

        Vector2 current = rigidbody.GetPointVelocity(track.position);
        Vector2 target = track.right * speed;
        Vector2 force = (target - current) * trackFriction;
        rigidbody.AddForceAtPosition(force, track.position);
    }
}
