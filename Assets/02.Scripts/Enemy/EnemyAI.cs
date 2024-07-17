using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using EnumTypes;
using EventLibrary;

public enum EnemyState
{
    Idle,
    PATROL,
    TRACE,
    ATTACK,
    DIE
}

public enum EnemyType
{
    Sniper,
    Paladin
}


public class EnemyAI : MonoBehaviour
{
    public IState CurrentStateInstance { get; private set; }
    public EnemyState EnemyCurstate = EnemyState.Idle;
    public Transform playerTr;
    public Transform enemyTr;
    public Transform firePos; // firePos �߰�
    public Animator animator;
    public EnemyMoveAgent enemyMoveAgent;
    public EnemyFire enemyFire;

    public float attackDist = 8.0f; // ���� �Ÿ�
    public float traceDis = 15.0f; // ���� �Ÿ�
    public float staticTraceDis = 15.0f; // ���� ���� �Ÿ�
    public int RotationAngle;
    public bool isDie = false;

    // �ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽ� ���� �̸� ����
    public readonly int Idle = Animator.StringToHash("IsIdle");
    public readonly int hashDie = Animator.StringToHash("IsDie");

    public float modelRotationOffset = 30f; // ���� ȸ�� ������

    public Vector3 initialPosition { get; private set; }
    public Quaternion initialRotation { get; private set; }

    private void Awake()
    {
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemyMoveAgent = GetComponent<EnemyMoveAgent>();
        enemyFire = GetComponent<EnemyFire>();

        initialPosition = enemyTr.position;
        initialRotation = enemyTr.rotation;
    }

    private void OnEnable()
    {
        EventManager<EnemyEvents>.StartListening(EnemyEvents.ChangeEnemyStateAttack, ChangeAttackState);
    }

    private void OnDisable()
    {
        EventManager<EnemyEvents>.StopListening(EnemyEvents.ChangeEnemyStateAttack, ChangeAttackState);
    }

    private void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ChangeState(EnemyState.Idle);
    }


    private void Update()
    {
        CurrentStateInstance?.ExecuteOnUpdate();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentStateInstance?.Exit(); // ���� ���� ����

        EnemyCurstate = newState;

        CurrentStateInstance = CreateStateInstance(newState); // ���ο� ���� �ν��Ͻ� ����
        CurrentStateInstance?.Enter(); // ���ο� ���� ����
    }

    private IState CreateStateInstance(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                return new IdleState(this);
            case EnemyState.PATROL:
                return new PatrolState(this);
            case EnemyState.TRACE:
                return new TraceState(this);
            case EnemyState.ATTACK:
                return new AttackState(this);
            case EnemyState.DIE:
                return new DieState(this);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }

    // ������ eventmanager�� �ҷ��� �Լ� 
    private void ChangeAttackState()
    {
        ChangeState(EnemyState.ATTACK);
    }

    public void ResetToInitialTransform()
    {
        enemyTr.position = initialPosition;
        enemyTr.rotation = initialRotation;
    }
}


