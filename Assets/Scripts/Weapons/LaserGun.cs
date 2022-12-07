using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public float aimTime;
    public float fireTime;
    public float windDown;
    public float tickDamage;
    public float tickFrequency;

    [Space]
    public AnimationCurve aimLineAngle;
    public LineRenderer leftLine;
    public LineRenderer rightLine;
    public LineRenderer laser;
    public ParticleSystem[] fireFX;
    public GameObject hitFX;

    bool firing = false;
    float nextTickTime;

    private void Start()
    {
        leftLine.gameObject.SetActive(false);
        rightLine.gameObject.SetActive(false);
        laser.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts the routine for firing the big laser, includes chargeup
    /// </summary>
    public void Shoot ()
    {
        if (!firing)
            StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        firing = true;
        float percent = 0.0f;

        leftLine.gameObject.SetActive(true);
        rightLine.gameObject.SetActive(true);

        laser.gameObject.SetActive(false);

        foreach (var fx in fireFX) fx.Play();

        while (percent < 1.0f)
        {
            leftLine.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, aimLineAngle.Evaluate(percent));
            rightLine.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -aimLineAngle.Evaluate(percent));

            LimitLine(leftLine);
            LimitLine(rightLine);

            percent += Time.deltaTime / aimTime;
            yield return null;
        }

        leftLine.gameObject.SetActive(false);
        rightLine.gameObject.SetActive(false);

        laser.gameObject.SetActive(true);

        percent = 0.0f;
        while (percent < 1.0f)
        {
            LimitLine(laser, out RaycastHit2D hit);

            if (hit)
            {
                if (hit.transform.TryGetComponent(out Health health))
                {
                    if (Time.time > nextTickTime)
                    {
                        health.Damage(new DamageArgs(transform.root.gameObject, tickDamage));

                        nextTickTime = Time.time + 1.0f / tickFrequency;
                    }
                }

                hitFX.SetActive(true);
                hitFX.transform.position = hit.point;
            }
            else
            {
                hitFX.SetActive(false);
            }

            percent += Time.deltaTime / fireTime;
            yield return null;
        }

        hitFX.SetActive(false);

        leftLine.gameObject.SetActive(false);
        rightLine.gameObject.SetActive(false);
        laser.gameObject.SetActive(false);

        yield return new WaitForSeconds(windDown);
        firing = false;
    }

    private void LimitLine(LineRenderer lineRenderer) => LimitLine(lineRenderer, out _);
    private void LimitLine(LineRenderer lineRenderer, out RaycastHit2D hit)
    {
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        hit = Physics2D.Raycast(lineRenderer.transform.position, lineRenderer.transform.right);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, lineRenderer.transform.right * 500.0f);
        }
    }
}
