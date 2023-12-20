using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _actions;
    private Rigidbody _rb;

    private Vector3 _moveInput;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _rotateSpeed;

    private void Start()
    {
        _actions = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * _moveSpeed;
        Debug.Log(_rb.velocity);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();
        _moveInput = new Vector3(input.x, 0, input.y);
    }

    private void OnFire()
    {

    }
}
