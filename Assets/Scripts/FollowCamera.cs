using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform _target;
    [SerializeField]
    private Vector3 _offset;

    public float moveSpeed = 5f;

    private void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 desiredPosition = _target.position + _offset;

            // Vector3 smoothedPosition = Vector3.Slerp(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            // transform.position = smoothedPosition;

            transform.position = desiredPosition;
        }
    }
}
