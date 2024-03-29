using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public enum Owner
    {
        Player,
        Enemy
    }

    public Owner owner;

    private PlayerInputActions _controls;

    public int damage;
    public int leftBullet;
    public int magazineSize;
    public float fireRate, spread;
    public float reloadTime = 0.5f;
    private bool _readyToFire, _isFiring, _reloading;
    public bool allowsAutoShot;

    public Vector3 targetPosition;

    [SerializeField]
    private GameObject _bulletPref;

    [SerializeField]
    private Transform _firePos;
     
    [SerializeField]
    private float _bulletSpeed;

    public bool IsReloading { get => _reloading;}

    private void Awake()
    {
        if (gameObject.GetComponentInParent<PlayerController>())
        {
            owner = Owner.Player;
            _controls = new PlayerInputActions();
            _controls.GamePlay.Fire.started += context => StartFiring();
            _controls.GamePlay.Fire.canceled += context => EndFiring();
            _controls.GamePlay.Reload.performed += context => Reload();
        }
    }

    private void Start()
    {
        _readyToFire = true;
    }

    private void OnEnable()
    {
        if (owner == Owner.Player)
            _controls.Enable();
    }

    private void OnDisable()
    {
        if (owner == Owner.Player)
            _controls.Disable();
    }

    private void Update()
    {
        if (_readyToFire && _isFiring && !_reloading && leftBullet > 0)
        {
            if(owner == Owner.Player)
                PerformFiring(GetMouseHitPosition());
            else if (owner == Owner.Enemy)
                PerformFiring(targetPosition);
        }
    }

    public void StartFiring()
    {
        _isFiring = true;
    }

    private void PerformFiring(Vector3 target)
    {
        _readyToFire = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = target - transform.position + new Vector3(x,y,0);
        direction.y = 0;

        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * _bulletSpeed, ForceMode.Impulse);

        leftBullet--;
        if (leftBullet > 0)
        {
            Invoke("ResetFiring", fireRate);
        }

        // 단발 사격
        if (!allowsAutoShot)
        {
            EndFiring();
        }
    }

    public void EndFiring()
    {
        _isFiring = false;
    } 

    public void ResetFiring()
    {
        Debug.Log("call invoke");

        _readyToFire = true;
    }

    public void Reload()
    {
        _reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    public void ReloadFinish()
    {
        leftBullet = magazineSize;
        _reloading = false;
        ResetFiring();
    }


    private Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
        }

        return mousePosition;
    }

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }
}
