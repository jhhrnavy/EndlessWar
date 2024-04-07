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

        yield return new WaitForSeconds(0.4f); // 공격 모션 과 데미지 들어가는 타이밍 조절

        // 적이 범위내에 있으면 해당적 공격 ( 단일 공격)
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
