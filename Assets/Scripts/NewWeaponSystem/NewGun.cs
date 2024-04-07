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
    public int reserveAmmo; // 잔여 총알
    public float fireRate, spread;
    public float reloadTime = 0.5f;
    public bool allowsAutoShot;
    public Transform firePoint;

    public bool isFiring = false;
    private bool _isReloading = false;
    private bool _readyToFire = true;
    private Vector3 _targetPosition;

    public static event System.Action<int, int, int> OnAmmoChanged;

    #region Public Methods
    // 연발 사격 함수
    public void StartFiring()
    {
        isFiring = true;
        StartCoroutine(PerformFiring());
    }

    public void EndFiring()
    {
        isFiring = false;
    }

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

    public void Reloading()
    {
        _isReloading = true;
        StartCoroutine(ReloadRoutine());
    }
    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    #endregion

    #region Private Method

    private Vector3 CalculateSpreadRange()
    {
        float x = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);

        return new Vector3(x, 0, z);
    }

    private void PerformReloading()
    {
        int index = (int)weaponStyle;
        int temp = magazineSize - currentAmmo;

        if (temp < reserveAmmo)
        {
            currentAmmo += temp;
            reserveAmmo -= temp;
        }
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }

        // UpdateUI
        OnAmmoChanged?.Invoke(currentAmmo, magazineSize, reserveAmmo);
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(reloadTime);

        PerformReloading();

        _isReloading = false;
    }

    private void ResetFiring()
    {
        _readyToFire = true;
    }

    private IEnumerator PerformFiring()
    {
        while (_readyToFire && isFiring && currentAmmo > 0)
        {
            _readyToFire = false;

            // 발사
            Fire(_targetPosition);

            currentAmmo--;

            // 연사 속도 조절
            //Invoke("ResetFiring", gun.fireRate);
            yield return new WaitForSeconds(fireRate);
            ResetFiring();

            // 단발 사격
            if (!allowsAutoShot)
            {
                EndFiring();
            }
        }
    }

    #endregion
}
