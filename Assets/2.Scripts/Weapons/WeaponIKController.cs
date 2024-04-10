using UnityEngine;

public class WeaponIKController : MonoBehaviour
{
    private Animator _anim;
    public Transform _trsfWeaponPivot;
    public Transform _trsfRHandMount;
    public Transform _trsfLHandMount;
    public Transform currentWeapon;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(1);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if(_anim != null)
        {
            _trsfWeaponPivot.position = _anim.GetIKHintPosition(AvatarIKHint.RightElbow);

            if (currentWeapon)
            {

                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                _anim.SetIKPosition(AvatarIKGoal.RightHand, _trsfRHandMount.position);
                _anim.SetIKRotation(AvatarIKGoal.RightHand, _trsfRHandMount.rotation);

                if(_trsfLHandMount != null)
                {
                    _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                    _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                    _anim.SetIKPosition(AvatarIKGoal.LeftHand, _trsfLHandMount.position);
                    _anim.SetIKRotation(AvatarIKGoal.LeftHand, _trsfLHandMount.rotation);
                }
                else
                {
                    _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                    _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
                }
            }
        }
    }

    public void ChangeWeaponIK(Transform newWeapon, Transform trsfRHandMount, Transform trsfLHandMount)
    {
        currentWeapon = newWeapon;
        _trsfRHandMount = trsfRHandMount;
        _trsfLHandMount = trsfLHandMount;

        // 상체 모션 변경
        if (newWeapon.GetComponent<NewWeapon>() is NewSword)
        {
            _anim.SetLayerWeight(1, 1f);
            _anim.SetLayerWeight(2, 0f);

        }
        else if (newWeapon.GetComponent<NewWeapon>() is Bomb)
        {
            _anim.SetLayerWeight(2, 1f);
            _anim.SetLayerWeight(1, 0f);
        }
        else
        {
            _anim.SetLayerWeight(1, 0f);
            _anim.SetLayerWeight(2, 0f);
        }
    }

    public void SetInit()
    {
        currentWeapon = null;
        _trsfRHandMount = null;
        _trsfLHandMount = null;
    }

}
