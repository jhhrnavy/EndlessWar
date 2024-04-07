using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    private Inventory _inventory;
    private WeaponIKController _weaponIKController;
    private PlayerShooting _playerShooting;
    private PlayerMeleeAttack _playerMeleeAttack;

    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private GameObject[] _equipments = new GameObject[4];


    public NewWeapon equipedWeapon;
    public AttackType curAttackType; 
    private Rigidbody _curWeaponRb;
    private Collider _curWeaponCollider;
    private float dropForwardForce = 3f;
    private float dropUpwardForce = 3f;

    public Transform WeaponHolder { get => _weaponHolder;}

    private void Start()
    {
        _weaponIKController = GetComponent<WeaponIKController>();
        _inventory = GetComponent<Inventory>();
        _playerShooting = GetComponent<PlayerShooting>();
        _playerMeleeAttack = GetComponent<PlayerMeleeAttack>();

        //// �κ��丮�� �̹� �ִ� ������ �ν��Ͻ�
        //for (int i = 0; i < _equipments.Length; i++)
        //{
        //    NewWeapon weapon = _inventory.GetItem(i);
        //    if (weapon != null)
        //    {
        //        var obj = Instantiate(weapon.prefab, _weaponHolder);
        //        obj.transform.localPosition = weapon.localPosition;
        //        obj.transform.localRotation = Quaternion.Euler(weapon.localRotation);
        //        obj.transform.localScale = weapon.localScale;

        //        _equipments[i] = obj;

        //        obj.SetActive(false);
        //    }
        //}
    }

    public void OnSwitchWeapons(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int index = (int)context.ReadValue<float>() - 1;
            if (_equipments[index] != null)
            {
                EquipWeapon(index);
                // ���� ��� ��ȯ
                ChangeAttackMode((WeaponStyle)index);
            }
            
            if(index == 3)
            {
                EquipWeapon(index);
                ChangeAttackMode((WeaponStyle)index);
            }
        }
    }

    public void OnDropWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed || 
            equipedWeapon == null ||
            equipedWeapon.weaponStyle == WeaponStyle.Melee ||
            equipedWeapon.weaponStyle == WeaponStyle.Throwing) return;

        Debug.Log("ȣ��");
        Drop();

        // �κ��丮���� ���� ����
        _inventory.RemoveItem((int)equipedWeapon.weaponStyle);

        // Unequipped
        UpdateEquipmentInfo(null, null, null);
        ChangeAttackMode(WeaponStyle.None);

        // �ִϸ��̼� IK ����
        _weaponIKController.SetInit();
    }
    private void Drop()
    {
        // Physics Drop weapon
        equipedWeapon.transform.SetParent(null);
        _curWeaponRb.isKinematic = false;
        _curWeaponCollider.isTrigger = false;
        _curWeaponRb.velocity = GetComponent<Rigidbody>().velocity;
        _curWeaponRb.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
        _curWeaponRb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);
        equipedWeapon.gameObject.layer = 10;
    }

    public void EquipWeapon(int weaponStyle)
    {
        if (equipedWeapon != null) 
            equipedWeapon.gameObject.SetActive(false);

        if(_equipments[weaponStyle] != null)
        {
            _equipments[weaponStyle].SetActive(true);
            equipedWeapon = _equipments[weaponStyle].GetComponent<NewWeapon>();
            UpdateEquipmentInfo(_equipments[weaponStyle], _equipments[weaponStyle].GetComponent<Rigidbody>(), _equipments[weaponStyle].GetComponent<Collider>());
        }

        if (_curWeaponRb != null)
            _curWeaponRb.isKinematic = true;
        if (_curWeaponCollider != null)
            _curWeaponCollider.isTrigger = true;

        // �ִϸ��̼� IK ����
        _weaponIKController.ChangeWeaponIK(equipedWeapon.transform, equipedWeapon.trsfRHandMount, equipedWeapon.trsfLHandMount);
    }

    public void AddEquipment(GameObject equipment)
    {
        equipment.transform.SetParent(_weaponHolder);
        var weaponInfo = equipment.GetComponent<NewWeapon>();
        equipment.transform.localPosition = weaponInfo.localPosition;
        equipment.transform.localRotation = Quaternion.Euler(weaponInfo.localRotation);
        equipment.transform.localScale = weaponInfo.localScale;

        _equipments[(int)weaponInfo.weaponStyle] = equipment;
    }

    private void UpdateEquipmentInfo(GameObject weapon, Rigidbody rb, Collider coll)
    {
        if(weapon != null)
            equipedWeapon = weapon.GetComponent<NewWeapon>();
        else
            equipedWeapon = null;

        _curWeaponRb = rb;
        _curWeaponCollider = coll;
    }

    private void ChangeAttackMode(WeaponStyle weaponStyle)
    {
        switch (weaponStyle)
        {
            case WeaponStyle.None:
                curAttackType = AttackType.None;
                break;

            case WeaponStyle.Primary:
            case WeaponStyle.Secondary:
                curAttackType = AttackType.Shooting;
                _playerShooting.SetGun(equipedWeapon);
                break;

            case WeaponStyle.Melee:
                curAttackType = AttackType.Melee;
                _playerMeleeAttack.weapon = equipedWeapon as NewSword;
                break;

            case WeaponStyle.Throwing:
                curAttackType = AttackType.Throwing;
                break;
        }
    }

}
