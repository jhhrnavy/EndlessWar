using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy _enemy;

    public AttackState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        if(_enemy.isfacingTarget)
            Debug.Log("АјАн!!");

        _enemy.Rotate();

        if (_enemy.Fow.visibleTargets.Count == 0)
        {
            _enemy.ChangeState(new ChaseState(_enemy));
        }
    }
    public void PhysicsExecute()
    {
    }

    public void Exit()
    {
    }

}
