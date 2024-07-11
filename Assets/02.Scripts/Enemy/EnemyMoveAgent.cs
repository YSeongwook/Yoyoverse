using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAgent : MonoBehaviour
{
    // ���� �������� �����ϱ� ���� List Ÿ�� ����
    public List<Transform> wayPoints;

    // ���� ���� ������ �迭�� Index
    public int nextIdx = 0;
    public int endIdx = 0; // ���������� ���� Ƚ��

    private NavMeshAgent agent;
    private Transform enemyTr;

    private float damping = 1.0f;

    private float moveSpeed = 1.5f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }

    private bool _patrolling;

    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                // agent.speed = patrollSpeed;
                agent.speed = moveSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    // ���� ����� ��ġ�� �����ϴ� ����
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            // agent.speed = traceSpeed;
            agent.speed = moveSpeed * 2;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = moveSpeed;

        var group = GameObject.Find("WayPointGroup");

        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            // ù��° ��ҿ� �θ��� transform�� ���� ���� -> waypointgroup�� transform�� ��, point�鸸 ���� 
            wayPoints.RemoveAt(0);

            nextIdx = Random.Range(0, wayPoints.Count);
        }

        MoveWayPoint();
    }

    private void MoveWayPoint()
    {
        //�ִ� �Ÿ� ��� ����� ������ �ʾ����� ������ �������� ����
        if (agent.isPathStale) return;

        if (nextIdx < wayPoints.Count)
        {
            //���� �������� wayPoints �迭���� ������ ��ġ�� ���� �������� ����
            agent.destination = wayPoints[nextIdx].position;
            agent.isStopped = false;
        }
    }

    void Update()
    {
        if (agent.isStopped == false)
        {
            //NavMeshAgent�� ������ ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // ���� �Լ��� ����� ������ ȸ��
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);

        }

        if (!_patrolling) return;

        // NavMeshAgent�� �̵��ϰ� �ְ� �������� �����ߴ��� ���� ��� 
        // velocity�� �̿��ؼ� ����ӵ��� �޾ƿ���, �������� ����� ���ٰ� �ǴܵǸ� ���� �������� �ε����� ���� -> ���� �������� �̵�
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            #region ���������� �̵� 
            //if (nextIdx == wayPoints.Count) endIdx++;
            //if (nextIdx == 0) endIdx++;
            //// ���� ������ �迭 ÷�ڸ� ��� 
            //if(endIdx !=0 && endIdx %2 !=0 && nextIdx < wayPoints.Count) 
            //    nextIdx = ++nextIdx;
            //else if (endIdx != 0 && endIdx % 2 == 0 && nextIdx <= wayPoints.Count)
            //    nextIdx = --nextIdx;
            #endregion
            nextIdx = Random.Range(0, wayPoints.Count);

            // ���� �������� �̵� ��� ����
            MoveWayPoint();
        }
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }
}
