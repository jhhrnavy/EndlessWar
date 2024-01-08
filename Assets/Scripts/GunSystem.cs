using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public int damage;
    public int leftBullet;
    public float timeBetweenShooting, spread;
    public bool canShoot = false;


    [SerializeField]
    private GameObject _bulletPref;

    [SerializeField]
    private Transform _firePos;
     
    [SerializeField]
    private float _bulletSpeed;

    public void Shoot()
    {
        if (!canShoot)
            return;

        // Spread
        float x = Random.Range(-spread, spread);

        // Calculate Direction with spread
        Vector3 direction = transform.forward + new Vector3(x, 0, 0);

        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction * _bulletSpeed, ForceMode.Impulse);

        canShoot = false;
        leftBullet--;
        Invoke("ResetShot", timeBetweenShooting);
    }

    public void ResetShot()
    {
        canShoot = true;
    }

}
