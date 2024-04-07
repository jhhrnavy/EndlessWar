using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    [SerializeField] private HealthBarUI _healthBar;
    [SerializeField] private ShieldBarUI _shieldBar;

    // 0 : Ranged Atk, 1 : Melee Atk, 2 : Throwing
    [SerializeField] private PlayerCombat[] _combat;

    private EquipmentManager _equipmentManager;

    private Vector3 _moveInput;
    private Vector3 _rotDir;
    [SerializeField] private LayerMask _groundLayer;
    // Spec
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private int _hp = 10;
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _shield = 100;
    [SerializeField] private int _maxShield = 100;

    private bool _isDead = false;

    // event
    public static event Action OnPlayerDeath;

    #region Properties

    public int Hp { get => _hp; set => _hp = value; }

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _equipmentManager = GetComponent<EquipmentManager>();
        _combat = new PlayerCombat[3];
        _combat[0] = GetComponent<NewPlayerShooting>();
        _combat[1] = GetComponent<PlayerMeleeAttack>();
        _combat[2] = GetComponent<BombShooter>();
    }

    private void Start()
    {
        SetInit();
    }

    private void Update()
    {
        _rotDir = GetLookDirection();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * _moveSpeed;
        if (_rotDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_rotDir);
    }

    #region Input Actions
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started) return;

        Vector2 input = context.ReadValue<Vector2>();
        _moveInput = new Vector3(input.x, 0, input.y);

        // �̵� ������ ���ΰ� ���ΰ�
        if (Vector3.Dot(_rotDir, _moveInput) >= 0)
            _anim.SetFloat("Walk", input.magnitude);
        else
            _anim.SetFloat("Walk", -input.magnitude);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(_equipmentManager.equipedWeapon == null) return;

        if (context.started)
        {
            _combat[(int)_equipmentManager.curAttackType].Attack();
        }
        else if (context.canceled)
        {
            _combat[(int)_equipmentManager.curAttackType].CancledAttack();
        }
    }

    #endregion

    #region Public Method

    public void GetHit(int damage)
    {
        if (_isDead) return;

        if (_shield > 0)
        {
            // ��ȣ���� ������ ����
            int damageToShield = Mathf.Min(damage, _shield);
            _shield -= damageToShield;
            damage -= damageToShield;
        }

        if (damage > 0)
            Hp -= damage;

        if(_healthBar != null)
            _healthBar.SetHealth(_hp);

        if(_shieldBar != null)
            _shieldBar.SetShield(_shield);

        if (_hp <= 0)
            Die();
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

        // ����� �Ͻ����� �� ����� �г� Ȱ��ȭ �̺�Ʈ �߻�
        OnPlayerDeath?.Invoke();
    }

    #endregion

    private Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
        {
            mousePosition = hit.point;
        }

        return mousePosition;
    }

    private Vector3 GetLookDirection()
    {
        Vector3 mousePoint = GetMouseHitPosition();
        Vector3 dir = Vector3.zero;

        if (mousePoint != Vector3.zero)
        {
            dir = mousePoint - transform.position;
            dir.y = 0;
            dir.Normalize();
        }

        return dir;
    }

    private void SetInit()
    {
        // Set Hp
        _hp = _maxHp;

        if (_healthBar != null)
            _healthBar.SetMaxHealth(_maxHp);

        //Set ShieldPoint
        _shield = _maxShield;

        if (_shieldBar != null)
            _shieldBar.SetInit(_maxShield);
    }
}