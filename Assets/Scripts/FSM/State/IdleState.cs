using UnityEngine;

public class IdleState : IState
{
    private Enemy _enemy;

    public IdleState(Enemy enemy)
    {
        _enemy = enemy;
        _enemy.hasArrivedAtLastPoint = false;
    }

    public void Enter()
    {
        Debug.Log("Enter Idle State!");
    }

    public void Exit()
    {
        Debug.Log("Exit Idle State!");
    }

    public void Execute()
    {
        Debug.Log("´ë±âÁß");
        if (_enemy.Fow.visibleTargets.Count > 0)
        {
            _enemy.ChangeState(new ChaseState(_enemy));
        }
    }

    public void PhysicsExecute()
    {
    }
}
