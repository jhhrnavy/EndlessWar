using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private IState currentState;
    private FieldOfView _fow;
    private Rigidbody _rb;
    private NavMeshAgent _pathfinder;

    public int hp = 10;
    public float moveSpeed = 5f;
    public float rotSpeed = 5f;

    public float minDotThreshold = 0.1f;

    private bool _isDead = false;
    public bool isfacingTarget = false;
    public bool hasArrivedAtLastPoint = false;

    // 접촉한 벽 따라서 이동하기
    public float collisionCheckDistance = 1f;
    public LayerMask obstacleMask;
    RaycastHit[] hits;
    public FieldOfView Fow { get => _fow; set => _fow = value; }

    private void Start()
    {
        currentState = new IdleState(this);
        _fow = gameObject.GetComponent<FieldOfView>();
        _rb = gameObject.GetComponent<Rigidbody>();
        _pathfinder = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        currentState.Execute();
    }

    private void FixedUpdate()
    {
        currentState.PhysicsExecute();
    }

    public void ChangeState(IState newState)
    {
        currentState.Exit();

        currentState = newState;

        currentState.Enter();
    }

    public void GetHit()
    {
        --hp;
        if(hp <= 0)
        {
            _isDead = true;
            ChangeState(new DeadState(this));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !_isDead)
        {
            GetHit();
        }
    }

    public void Move()
    {

        //Vector3 direction = _fow.targetLastPosition - transform.position;
        //direction.y = 0f;

        //_rb.MovePosition(_rb.position + direction.normalized * moveSpeed * Time.fixedDeltaTime);

        //float distance = Vector3.Distance(_rb.position, _fow.targetLastPosition);

        //if (distance <= minDotThreshold)
        //{
        //    hasArrivedAtLastPoint = true;
        //}

        _pathfinder.SetDestination(_fow.targetLastPosition);

        if (_pathfinder.velocity == Vector3.zero)
        {
            hasArrivedAtLastPoint = true;
        }
    }

    public void Rotate()
    {

        Vector3 direction = _fow.targetLastPosition - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.Lerp(_rb.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(rotation);
        float dot = Vector3.Dot(transform.forward, direction.normalized);
        if(dot >= 0.95f)
            isfacingTarget = true;
        else
            isfacingTarget = false;
    }

    public void Die()
    {
        StartCoroutine(DelayedDie());
    }

    private IEnumerator DelayedDie()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
