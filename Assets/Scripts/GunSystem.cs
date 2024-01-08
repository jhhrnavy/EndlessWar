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

    public void Shoot(Vector3 mousePoint)
    {
        if (!canShoot)
            return;

        // Spread
        float x = Random.Range(-spread, spread);

        // Calculate Direction with spread
        Vector3 dir = (mousePoint - _firePos.position).normalized + new Vector3(x, 0, 0);
        dir.y = 0;

        var bullet = Instantiate(_bulletPref, _firePos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(dir * _bulletSpeed, ForceMode.Impulse);

        canShoot = false;
        leftBullet--;
        Invoke("ResetShot", timeBetweenShooting);
    }

    public void ResetShot()
    {
        canShoot = true;
    }

}
