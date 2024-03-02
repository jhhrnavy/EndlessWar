using Unity.VisualScripting;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    private PlayerInputActions _controls;

    public int damage;
    public int leftBullet;
    public int magazineSize;
    public float fireRate, spread;
    public float reloadTime = 0.5f;
    private bool _readyToFire, _isFiring, _reloading;
    public bool _allowsAutoShot;

    [SerializeField]
    private GameObject _bulletPref;

    [SerializeField]
    private Transform _firePos;
     
    [SerializeField]
    private float _bulletSpeed;

    private void Awake()
    {
        _readyToFire = true;
        _controls = new PlayerInputActions();

        _controls.GamePlay.Fire.started += context => StartFiring();
        _controls.GamePlay.Fire.canceled += context => EndFiring();

        _controls.GamePlay.Reload.performed += context => Reload();
    }

    private void Update()
    {
        if (_readyToFire && _isFiring && !_reloading && leftBullet > 0)
            PerformFiring();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }


    //public void Fire(Vector3 mousePoint)
    //{
    //    if (!_readyToFire)
    //        return;

    //    // Spread
    //    float x = Random.Range(-spread, spread);

    //    // Calculate Direction with spread
    //    Vector3 dir = (mousePoint - _firePos.position).normalized + new Vector3(x, 0, 0);
    //    dir.y = 0;

    //    var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
    //    bullet.GetComponent<Rigidbody>().AddForce(dir * _bulletSpeed, ForceMode.Impulse);

    //    _readyToFire = false;
    //    leftBullet--;
    //    Invoke("ResetShot", fireRate);
    //}


    private void StartFiring()
    {
        _isFiring = true;
    }

    private void PerformFiring()
    {
        _readyToFire = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = GetMouseHitPosition() - transform.position + new Vector3(x,y,0);

        direction.y = 0;

        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * _bulletSpeed, ForceMode.Impulse);

        leftBullet--;

        if (leftBullet >= 0)
        {
            Invoke("ResetFiring", fireRate);
        }

        // 단발 사격
        if (!_allowsAutoShot)
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

    private void Reload()
    {
        _reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        leftBullet = magazineSize;
        _reloading = false;
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
}
