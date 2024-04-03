using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private  Animator _anim;

    [SerializeField]
    private HealthBarUI _healthBar;
    [SerializeField]
    private ShieldBarUI _shieldBar;

    private Vector3 _moveInput;

    // Spec
    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private int _hp = 10;
    [SerializeField]
    private int _maxHp = 100;

    private bool _isDead = false;

    [SerializeField]
    private int _shield = 100;

    [SerializeField]
    private int _maxShield = 100;

    private Vector3 _rotDir;

    // Weapon
    [SerializeField]
    private GameObject[] _weapons = new GameObject[4];

    [SerializeField]
    private GunSystem _currentGun;

    //Anim IK
    [SerializeField]
    private Transform _trsfGunPivot;

    [SerializeField]
    private Transform[] _trsfLHandMount;   //  총에 왼손이 위치할 지점

    [SerializeField]
    private Transform[] _trsfRHandMount;   //  총에 오른손이 위치할 지점

    WeaponType _weaponType = WeaponType.Main;

    // event
    public static event Action OnPlayerDeath;


    enum WeaponType
    {
        Main, // 주 무기 : 라이플 등
        Sub, // 보조 무기 : 권총
        melee, // 근접 무기
        Grenade, // 투척물
    }

    #region Properties

    public int Hp { get => _hp; set => _hp = value; }

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        // Set default waepon
        SwitchWeapons(0);

        // Set Hp
        _hp = _maxHp;
        _healthBar.SetMaxHealth(_maxHp);

        //Set ShieldPoint
        _shield = _maxShield;
        _shieldBar.SetInit(_maxShield);
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

    #region Input Actions
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
    //    if (value.performed)
    //        _currentGun.Fire(GetMouseHitPosition());
    //}
    #endregion

    #region Public Method

    public void SwitchWeapons(int index)
    {
        _weaponType = (WeaponType)index;
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].SetActive(false);
        }

        if (index < 2)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 0f);
        }
        else if (index == 2)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 1f);
        }
        else if (index == 3)
        {
            _weapons[index].SetActive(true);
            _anim.SetLayerWeight(1, 1f);
        }
    }

    public void GetHit(int damage)
    {
        if (_isDead) return;

        if(_shield > 0)
        { 
            // 보호막이 데미지 흡수
            int damageToShield = Mathf.Min(damage, _shield);
            _shield -= damageToShield;
            damage -= damageToShield;
        }

        if(damage > 0)
            Hp -= damage;

        _healthBar.SetHealth(_hp);
        _shieldBar.SetShield(_shield);

        if (_hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        _hp += amount;

        if (_hp > _maxHp)
            _hp = _maxHp;

        _healthBar.SetHealth(_hp);
    }

    public void GetShield(int amount)
    {
        _shield += amount;

        if (_shield > _maxShield)
            _shield = _maxShield;

        _shieldBar.SetShield(_shield);

    }

    public void Die()
    {
        _isDead = true;

        // 사망시 일시정지 및 재시작 패널 활성화 이벤트 발생
        OnPlayerDeath?.Invoke();
    }

    #endregion

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
