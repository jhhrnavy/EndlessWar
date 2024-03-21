using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private Enemy _enemy;

    public ChaseState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Execute()
    {
        Debug.Log("ÃßÀûÁß");
        if (_enemy.Fow.visibleTargets.Count == 0)
        {

            if (_enemy.hasArrivedAtLastPoint)
            {
                _enemy.ChangeState(new IdleState(_enemy));
                return;
            }
        }
        else
        {
            if (_enemy.isfacingTarget)
            {
                //Attack
                _enemy.ChangeState(new AttackState(_enemy));
            }
        }
        //_enemy.Rotate();


    }
    public void PhysicsExecute()
    {
        if (_enemy.Fow.visibleTargets.Count == 0 && _enemy.isfacingTarget)
            _enemy.Move();

            _enemy.Rotate();

    }

    public void Exit()
    {
    }
}
