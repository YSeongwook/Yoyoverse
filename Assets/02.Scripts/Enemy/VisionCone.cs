using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Transform player; // �÷��̾� Ʈ������
    public float viewAngle = 45f; // �þ߰�
    public float viewDistance = 10f; // �þ� �Ÿ�
    public LayerMask obstacleMask; // ��ֹ� ���̾� ����ũ
    public LayerMask playerMask; // �÷��̾� ���̾� ����ũ
    private MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        // �þ� �Ÿ� �� ��� �ݶ��̴� ����
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, playerMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // �÷��̾ �þ߰� �ȿ� �ִ��� Ȯ��
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // �÷��̾�� �þ� ���̿� ��ֹ��� ������ Ȯ��
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    Debug.Log("�÷��̾� ������");
                    // �÷��̾� ���� �� �߰� ���� �ۼ�
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
