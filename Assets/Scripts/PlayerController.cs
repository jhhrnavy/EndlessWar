using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private PlayerInputActions _actions;
    private Rigidbody _rb;
    private  Animator _anim;

    private Vector3 _moveInput;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _rotateSpeed = 5f;

    private Vector3 _rotDir;

    [SerializeField]
    private List<GameObject> _weapons;

    [SerializeField]
    private GunSystem _currentGun;

    private void Start()
    {
        //_actions = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _currentGun = _weapons[0].GetComponent<GunSystem>();
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
        _anim.SetFloat("Walk Forward", input.magnitude);
    }

    private void OnFire()
    {
        _currentGun.Shoot(GetMouseHitPosition());
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
}
