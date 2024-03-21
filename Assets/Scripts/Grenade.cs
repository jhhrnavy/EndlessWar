using Unity.VisualScripting;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 3f;
    public float radius = 5f;
    public float explosionForce = 700f;
    public int damage;
    private float countdown;
    private bool hasExploded;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            gameObject.SetActive( false );
        }
    }

    private void Explode()
    {
        hasExploded = true;
        GameObject expEfx = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colls = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colls) 
        {
            if (nearbyObject.CompareTag("Obstacle")) 
                continue;

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
                Destroy(nearbyObject.gameObject, 1f);
            }
        }
        Destroy(expEfx, 0.5f);
    }
}
