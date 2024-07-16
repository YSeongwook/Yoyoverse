using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyFire : MonoBehaviour
{
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;
    private EnemyAI enemyAI;

    private readonly int hashFire = Animator.StringToHash("Fire");

    private float nextFire = 0f; // �ʱ� ��Ÿ�� ����
    private readonly float fireRate = 2.0f; // ��Ÿ���� 2�ʷ� ����

    [SerializeField] private readonly float reloadTime = 3.0f;
    [SerializeField] private readonly int maxBullet = 10;

    public bool isFire = false;
    public bool isFireAnimIng = false;
    private bool pendingIdleState = false; // Idle ���� ��ȯ ��� �÷���

    [Header("Bullet")]
    public GameObject bullet;
    public GameObject bullet_Shell;

    [SerializeField] private Transform firePos;

    [SerializeField] private float detectionRange = 10.0f; // �����Ÿ� ����

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();

        // ���� �����Ÿ� ������ ���� SphereCollider �߰�
        SphereCollider rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = detectionRange;

        // �÷��̾� Transform ��������
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isFire && Time.time >= nextFire && !isFireAnimIng)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // �ִϸ��̼� ���
        isFireAnimIng = true;
        animator.SetTrigger(hashFire);
        FireBullet();
    }

    public void FireBullet()
    {
        // �÷��̾��� ��ġ�� �������� �Ѿ��� ������ ����
        Vector3 direction = (playerTr.position - firePos.position).normalized;

        // �Ѿ� �ν��Ͻ� ����
        GameObject bulletInstance = ObjectPool.Instance.DequeueObject(bullet);
        bulletInstance.transform.position = firePos.position;
        bulletInstance.transform.rotation = Quaternion.LookRotation(direction);

        // �Ѿ� �߻� ���� �߰� (��: Rigidbody ���)
        Rigidbody bulletRb = bulletInstance.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = direction * 500;
        }

        //EffectManager.Instance.FireEffectGenerate(firePos.position, firePos.rotation);
        //gunShot.PlayOneShot(gunShot.clip);
    }

    // �ִϸ��̼� ������ �� ȣ��� �޼���
    public void OnFireAnimationEnd()
    {
        // ���� �߻� �ð��� ���� �ð� + ��Ÿ������ ����
        nextFire = Time.time + fireRate;
        isFireAnimIng = false;

        // �ִϸ��̼��� ���� �� Idle ���·� ��ȯ
        if (pendingIdleState)
        {
            enemyAI.ChangeState(EnemyState.Idle);
            pendingIdleState = false;
        }
    }

    public void StartFiring()
    {
        isFire = true;
        pendingIdleState = false; // Fire ���¿��� ������� ����
    }

    public void StopFiring()
    {
        isFire = false;
        if (isFireAnimIng)
        {
            pendingIdleState = true; // �ִϸ��̼��� ���� ���̸� Idle ���� ��ȯ ���
        }
        else
        {
            enemyAI.ChangeState(EnemyState.Idle); // �ٷ� Idle ���·� ��ȯ
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopFiring();
        }
    }

    // ����� ����Ͽ� �����Ÿ��� �ð�ȭ
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // �Ѿ� �߻� ��ġ�� ������ �ð�ȭ
        if (firePos != null && playerTr != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(firePos.position, playerTr.position);
            Gizmos.DrawSphere(playerTr.position, 0.2f); // �÷��̾� ��ġ�� ���� �� ǥ��
        }
    }
}
