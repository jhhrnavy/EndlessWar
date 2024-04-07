using UnityEngine;

public class BombShooter : PlayerCombat
{
    [SerializeField] private GameObject _bombPrefabs;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _throwSpeed = 5f;

    public override void Attack()
    {
        ThrowBomb();
    }

    public void ThrowBomb()
    {
        Vector3 direction = GetMouseHitPosition() - transform.position;
        direction.y = 0f;
        direction.Normalize();
        GameObject bomb = Instantiate(_bombPrefabs, _startPoint.position + (direction * 2f), Quaternion.identity);
        bomb.GetComponent<Rigidbody>().AddForce(direction * _throwSpeed, ForceMode.Impulse);
        bomb.GetComponent<Bomb>().Explode();
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
