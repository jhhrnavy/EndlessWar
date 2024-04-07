using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGun : NewWeapon, IFireable
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Space, Header("Gun")]
    public int currentAmmo;
    public int magazineSize;
    public int reserveAmmo; // ÀÜ¿© ÃÑ¾Ë
    public float fireRate, spread;
    public float reloadTime = 0.5f;
    public bool allowsAutoShot;
    public Transform firePoint;

    public void Fire(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - firePoint.position;
        direction.y = 0;

        // Calculate Spread
        Vector3 spread = CalculateSpreadRange();
        direction += spread;

        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * bulletSpeed, ForceMode.Impulse);
    }

    private Vector3 CalculateSpreadRange()
    {
        float x = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        return new Vector3(x, 0, z);
    }
}
