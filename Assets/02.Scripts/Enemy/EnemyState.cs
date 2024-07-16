using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void ExecuteOnUpdate();
    void Exit();
}

public class IdleState : IState
{
    private EnemyAI enemyAI;
    private Coroutine rotateCoroutine;

    public IdleState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        enemyAI.animator.SetBool(enemyAI.Idle, true);
        rotateCoroutine = enemyAI.StartCoroutine(RotateCoroutine());
    }

    public void ExecuteOnUpdate()
    {
        // Add any other update logic if needed
    }

    public void Exit()
    {
        enemyAI.animator.SetBool(enemyAI.Idle, false);
        if (rotateCoroutine != null)
        {
            enemyAI.StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            float targetAngle = enemyAI.transform.eulerAngles.y + enemyAI.RotationAngle;
            yield return RotateToAngle(targetAngle);

            yield return new WaitForSeconds(1f); // Wait for 1 second before rotating back

            targetAngle = enemyAI.transform.eulerAngles.y - enemyAI.RotationAngle;
            yield return RotateToAngle(targetAngle);

            yield return new WaitForSeconds(1f); // Wait for 1 second before rotating again
        }
    }

    private IEnumerator RotateToAngle(float targetAngle)
    {
        float currentAngle = enemyAI.transform.eulerAngles.y;
        float startAngle = currentAngle;
        float t = 0f;
        float duration = 2f; // 2 second duration for rotation

        while (t < duration)
        {
            t += Time.deltaTime;
            float angle = Mathf.Lerp(startAngle, targetAngle, t / duration);
            enemyAI.transform.eulerAngles = new Vector3(enemyAI.transform.eulerAngles.x, angle, enemyAI.transform.eulerAngles.z);
            yield return null;
        }

        enemyAI.transform.eulerAngles = new Vector3(enemyAI.transform.eulerAngles.x, targetAngle, enemyAI.transform.eulerAngles.z);
    }
}

public class PatrolState : IState
{
    private EnemyAI enemyAI;

    public PatrolState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        enemyAI.moveAgent.patrolling = true;
        enemyAI.animator.SetBool(enemyAI.hashMove, true);
    }

    public void ExecuteOnUpdate()
    {
        // ���� ������Ʈ �ڵ�
    }

    public void Exit()
    {
        enemyAI.moveAgent.patrolling = false;
        enemyAI.animator.SetBool(enemyAI.hashMove, false);
    }
}

public class TraceState : IState
{
    private EnemyAI enemyAI;

    public TraceState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        enemyAI.animator.SetBool(enemyAI.hashMove, true);
    }

    public void ExecuteOnUpdate()
    {
        enemyAI.moveAgent.traceTarget = enemyAI.playerTr.position;
    }

    public void Exit()
    {
        enemyAI.animator.SetBool(enemyAI.hashMove, false);
    }
}

public class AttackState : IState
{
    private EnemyAI enemyAI;

    public AttackState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        // enemyfire�� ������Ʈ �� �����ϰԲ� ����
        enemyAI.enemyFire.StartFiring();
    }

    public void ExecuteOnUpdate()
    {

    }

    public void Exit()
    {
        //enemyAI.enemyFire.StopFiring();
    }
}

public class DieState : IState
{
    private EnemyAI enemyAI;

    public DieState(EnemyAI enemyAI)
    {
        this.enemyAI = enemyAI;
    }

    public void Enter()
    {
        enemyAI.isDie = true;
        enemyAI.enemyFire.isFire = false;
        enemyAI.moveAgent.Stop();

        enemyAI.animator.SetBool(enemyAI.hashDie, true);
    }

    public void ExecuteOnUpdate()
    {
        // ��� ���� ������Ʈ �ڵ�
    }

    public void Exit()
    {
        // ��� ���� ���� �ڵ�
    }
}
