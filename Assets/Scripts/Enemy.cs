using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 10;

    public void Die()
    {
        Destroy(gameObject);
    }

    public void GetHit()
    {
        --hp;
        if(hp <= 0)
        {
            Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GetHit();
        }
    }
}
