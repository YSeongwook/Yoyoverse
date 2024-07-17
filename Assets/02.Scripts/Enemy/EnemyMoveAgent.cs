using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool patrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void StartPatrolling()
    {
        patrolling = true;
        // ���⿡ ���� ������ �߰��մϴ�.
    }

    public void StopPatrolling()
    {
        patrolling = false;
        // ���⿡ ���� ���� ������ �߰��մϴ�.
    }
}


