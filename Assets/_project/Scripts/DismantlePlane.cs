using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismantlePlane : MonoBehaviour
{
    public static DismantlePlane instance;
    public List<Rigidbody> meshRigidbodies; // Rigidbodies attached to mesh-containing objects
    public List<Collider> colliders; // Colliders from the child objects
    public float explosionForce = 0.03f;
    public float explosionRadius = 15f;
    public float upwardsModifier = 0.03f;
    public GameObject explosionEffect;

    private bool exploded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Disable physics simulation for all mesh parts initially
        foreach (var rb in meshRigidbodies)
        {
            rb.isKinematic = true; // Disable physics simulation for mesh-containing objects
        }

        // Disable physics simulation for all colliders initially
        foreach (var col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics simulation for collider objects
            }
        }
    }

    public void TriggerExplosion()
    {
        if (exploded) return;
        exploded = true;

        // Instantiate explosion effect at the car's position
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Apply explosion force to all mesh-containing objects
        foreach (var rb in meshRigidbodies)
        {
            rb.isKinematic = false; // Enable physics simulation
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Acceleration);
        }

        // Apply explosion force to all colliders' Rigidbodies (if needed)
        foreach (var col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics simulation
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
        // Optionally, destroy the car after some time
        //Destroy(gameObject, 5f);
    }
}
