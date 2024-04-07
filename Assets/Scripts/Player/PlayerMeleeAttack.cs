using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttack : PlayerCombat
{
    [SerializeField] public NewSword weapon;
    private Animator _anim;

    [SerializeField] private bool _isAttacking = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public override void Attack()
    {
        if(weapon != null && !_isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(1);

        _anim.SetTrigger("Knife Attack");

        yield return new WaitForSeconds(0.4f); // ���� ��� �� ������ ���� Ÿ�̹� ����

        // ���� �������� ������ �ش��� ���� ( ���� ����)
        if (weapon.HitBox.IsHit())
        {
            weapon.HitBox.GetDetectedCollider().GetComponent<Enemy>().GetHit(weapon.damage);
        }

        while (!stateInfo.IsName("Right Stabbing") && stateInfo.normalizedTime < 0.8f)
        {
            stateInfo = _anim.GetCurrentAnimatorStateInfo(1);
            yield return null;
        }
        _isAttacking = false;

    }
}
