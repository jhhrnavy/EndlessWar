using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _actions;
    private Rigidbody _rb;

    private Vector3 _moveInput;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _rotateSpeed = 5f;

    private Vector3 _rotDir;
    // fire
    [SerializeField]
    private GameObject _bulletPref;

    [SerializeField]
    private Transform _firePos;

    [SerializeField]
    private float _bulletSpeed;

    private void Start()
    {
        _actions = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rotDir = GetLookDirection();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * _moveSpeed;

        transform.rotation = Quaternion.LookRotation(_rotDir);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();
        _moveInput = new Vector3(input.x, 0, input.y);
    }

    private void OnFire()
    {
        FireBullet();
    }

    private Vector3 GetLookDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePosition = hit.point;
            Vector3 dir = mousePosition - transform.position;
            dir.y = 0;
            dir.Normalize();
            return dir;
        }

        return Vector3.zero;
    }

    public void FireBullet()
    {
        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * _bulletSpeed,ForceMode.Impulse);
    }
}
