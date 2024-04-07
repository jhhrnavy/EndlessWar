using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : PlayerCombat
{
    [SerializeField] public NewGun gun;
    [SerializeField] private WeaponStyle weaponStyle;
    #region Public Method

    private void Update()
    {
        // �Ѿ� �߻� ��ǥ ����
        if(gun != null && gun.isFiring)
            gun.SetTargetPosition(GetMouseHitPosition());
    }

    public override void Attack()
    {
        if (gun != null)
        {
            gun.SetTargetPosition(GetMouseHitPosition());
            gun.StartFiring();
        }
    }

    public override void CancledAttack()
    {
        gun.EndFiring();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("reload");
            gun.Reloading();
        }
    }

    public void SetGun(NewWeapon weapon)
    {
        gun = (NewGun)weapon;
    }

    #endregion

    #region Private Method

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