using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerShooting : PlayerCombat
{
    [SerializeField] public NewGun gun;
    [SerializeField] private WeaponStyle weaponStyle;
    private bool _isFiring = false;
    private bool _readyToFire = true;
    private bool _isReloading = false;

    public static event Action<int, int, int> OnAmmoChanged;

    public void Update()
    {
        if (_readyToFire && _isFiring && gun.currentAmmo > 0)
        {
            PerformFiring();
        }
    }

    #region Public Method

    public override void Attack()
    {
        if (gun != null)
            StartFiring();
    }

    public override void CancledAttack()
    {
        EndFiring();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("reload");
            StartReloading();
        }
    }

    public void SetGun(NewWeapon weapon)
    {
        gun = (NewGun)weapon;
        _isFiring = false;
        _readyToFire = true;
    }

    #endregion

    #region Private Method

    private void StartFiring()
    {
        _isFiring = true;
    }

    private void PerformFiring()
    {
        _readyToFire = false;

        // 발사
        gun.Fire(GetMouseHitPosition());

        gun.currentAmmo--;

        // 연사 속도 조절
        //Invoke("ResetFiring", gun.fireRate);
        StartCoroutine(FireDelay());

        // 단발 사격
        if (!gun.allowsAutoShot)
        {
            EndFiring();
        }
    }

    private void EndFiring()
    {
        _isFiring = false;
    }

    private void ResetFiring()
    {
        _readyToFire = true;
    }

    private void StartReloading()
    {
        _isReloading = true;
        StartCoroutine(ReloadRoutine());
    }

    private void PerformReloading()
    {
        int index = (int)weaponStyle;
        int temp = gun.magazineSize - gun.currentAmmo;

        if (temp < gun.reserveAmmo)
        {
            gun.currentAmmo += temp;
            gun.reserveAmmo -= temp;
        }
        else
        {
            gun.currentAmmo += gun.reserveAmmo;
            gun.reserveAmmo = 0;
        }

        // UpdateUI
        OnAmmoChanged?.Invoke(gun.currentAmmo, gun.magazineSize, gun.reserveAmmo);
    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(gun.fireRate);
        ResetFiring();
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(gun.reloadTime);

        PerformReloading();

        _isReloading = false;
    }

    private Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
        }

        return mousePosition;
    }

    #endregion
}
