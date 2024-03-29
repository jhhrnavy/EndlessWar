using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : IState
{
    private Enemy _enemy;

    public ReloadState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.currentGun.Reload();
        _enemy.ChangeState(new IdleState(_enemy));
    }

    public void Execute()
    {
    }

    public void PhysicsExecute()
    {

    }

    public void Exit()
    {

    }

}
