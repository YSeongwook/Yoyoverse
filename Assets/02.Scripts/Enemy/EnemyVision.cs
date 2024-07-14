using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float viewAngle = 60f;
    public float viewDistance = 5f;
    public float viewHeight = 15f;
    public float forwardOffset = 1f; // ����� ���� ��ġ�� Z ������
    public LayerMask playerLayer;
    public int detectionResolution = 10;

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector3 detectionStart = transform.position + transform.forward * forwardOffset;

        for (float y = -viewHeight; y <= viewHeight; y += viewHeight / detectionResolution)
        {
            for (float angle = -viewAngle / 2; angle <= viewAngle / 2; angle += viewAngle / detectionResolution)
            {
                Vector3 dir = DirFromAngle(angle, false);
                Vector3 rayStart = detectionStart + Vector3.up * y;

                if (Physics.Raycast(rayStart, dir, out RaycastHit hit, viewDistance, playerLayer))
                {
                    Debug.Log("�÷��̾ �����Ǿ����ϴ�!");
                    // ���⿡ �÷��̾� ���� �� ������ ������ �߰��ϼ���.
                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 detectionStart = transform.position + transform.forward * forwardOffset;
        Gizmos.color = Color.yellow;

        // ������ ������ �׸���
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        // �Ʒ��� ������
        Gizmos.DrawLine(detectionStart - Vector3.up * viewHeight,
                        detectionStart - Vector3.up * viewHeight + viewAngleA * viewDistance);
        Gizmos.DrawLine(detectionStart - Vector3.up * viewHeight,
                        detectionStart - Vector3.up * viewHeight + viewAngleB * viewDistance);

        // ���� ������
        Gizmos.DrawLine(detectionStart + Vector3.up * viewHeight,
                        detectionStart + Vector3.up * viewHeight + viewAngleA * viewDistance);
        Gizmos.DrawLine(detectionStart + Vector3.up * viewHeight,
                        detectionStart + Vector3.up * viewHeight + viewAngleB * viewDistance);

        // ���� ��
        Gizmos.DrawLine(detectionStart - Vector3.up * viewHeight,
                        detectionStart + Vector3.up * viewHeight);
        Gizmos.DrawLine(detectionStart - Vector3.up * viewHeight + viewAngleA * viewDistance,
                        detectionStart + Vector3.up * viewHeight + viewAngleA * viewDistance);
        Gizmos.DrawLine(detectionStart - Vector3.up * viewHeight + viewAngleB * viewDistance,
                        detectionStart + Vector3.up * viewHeight + viewAngleB * viewDistance);

        // ������ ���� ���� �׸���
        for (float y = -viewHeight; y <= viewHeight; y += viewHeight / detectionResolution)
        {
            for (float angle = -viewAngle / 2; angle <= viewAngle / 2; angle += viewAngle / detectionResolution)
            {
                Vector3 dir = DirFromAngle(angle, false);
                Vector3 pos = detectionStart + Vector3.up * y;
                Gizmos.DrawLine(pos, pos + dir * viewDistance);
            }
        }
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}