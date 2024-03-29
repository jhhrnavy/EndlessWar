using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _controls;

    private Rigidbody _rb;
    private  Animator _anim;

    private Vector3 _moveInput;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _rotateSpeed = 5f;

    private Vector3 _rotDir;

    public GameObject[] _weapons = new GameObject[4];

    [SerializeField]
    private GunSystem _currentGun;

    public Transform _trsfGunPivot;

    public Transform[] _trsfLHandMount;   //  총에 왼손이 위치할 지점
    public Transform[] _trsfRHandMount;   //  총에 오른손이 위치할 지점


    WeaponType _weaponType = WeaponType.Main;

    enum WeaponType
    {
        Main, // 주 무기 : 라이플 등
        Sub, // 보조 무기 : 권총
        melee, // 근접 무기
        Grenade, // 투척물
    }

    private void Awake()
    {
        _controls = new PlayerInputActions();
        _controls.GamePlay.WeaponSwitch.performed += context => SwitchWeapons((int)context.ReadValue<float>() - 1);
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        //_actions = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        SwitchWeapons(0);
    }

    private void Update()
    {
        _rotDir = GetLookDirection();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * _moveSpeed;
        if(_rotDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_rotDir);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        if (value.started) return;

        Vector2 input = value.ReadValue<Vector2>();
        _moveInput = new Vector3(input.x, 0, input.y);

        // 이동 방향이 앞인가 뒤인가
        if(Vector3.Dot(_rotDir,_moveInput) >= 0)
            _anim.SetFloat("Walk", input.magnitude);
        else
            _anim.SetFloat("Walk", -input.magnitude);

        Debug.Log(input.magnitude);
    }

    public void OnSwitchWeapons(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log(value.ReadValue<float>());
            int index = (int)value.ReadValue<float>() - 1;
            SwitchWeapons(index);
        }
    }

    //public void OnFire(InputAction.CallbackContext value)
    //{
    //    if(value.performed)
    //        _currentGun.Fire(GetMouseHitPosition());
    //}

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

    private Vector3 GetLookDirection()
    {
        Vector3 mousePoint = GetMouseHitPosition();
        if (mousePoint != Vector3.zero)
        {
            Vector3 dir = mousePoint - transform.position;
            dir.y = 0;
            dir.Normalize();
            return dir;
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, GetLookDirection());
    }

    public void SwitchWeapons(int index)
    {
        _weaponType = (WeaponType)index;
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].SetActive(false);
        }

        if(index < 2)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 0f);
        }
        else if(index == 2)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 1f);
        }
        else if(index == 3)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 1f);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(_weaponType == WeaponType.Main || _weaponType == WeaponType.Sub)
        {
            _trsfGunPivot.position = _anim.GetIKHintPosition(AvatarIKHint.RightElbow);

            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            _anim.SetIKPosition(AvatarIKGoal.LeftHand, _trsfLHandMount[(int)_weaponType].position);
            _anim.SetIKRotation(AvatarIKGoal.LeftHand, _trsfLHandMount[(int)_weaponType].rotation);

            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            _anim.SetIKPosition(AvatarIKGoal.RightHand, _trsfRHandMount[(int)_weaponType].position);
            _anim.SetIKRotation(AvatarIKGoal.RightHand, _trsfRHandMount[(int)_weaponType].rotation);
        }
        else if(_weaponType == WeaponType.melee)
        {
            _trsfGunPivot.position = _anim.GetIKHintPosition(AvatarIKHint.RightElbow);

            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);

            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            _anim.SetIKPosition(AvatarIKGoal.RightHand, _trsfRHandMount[(int)_weaponType].position);
            _anim.SetIKRotation(AvatarIKGoal.RightHand, _trsfRHandMount[(int)_weaponType].rotation);
        }

    }
}
