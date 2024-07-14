using UnityEngine;

public class NPCDetection : MonoBehaviour
{
    public Transform npcTransform; // NPC�� Transform
    public Transform playerTransform; // �÷��̾��� Transform
    public float fovAngle = 30.0f; // �þ߰� ���� (60���� ����)
    public float detectionRange = 5.0f; // ���� �Ÿ�
    public float verticalRange = 2.0f; // ���� ���� ����

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Vector3 npcPosition = npcTransform.position;
        Vector3 playerPosition = playerTransform.position;

        // ���� �Ÿ� �� ���� �Ÿ� ���
        Vector3 directionToPlayer = playerPosition - npcPosition;
        float horizontalDistance = new Vector3(directionToPlayer.x, 0, directionToPlayer.z).magnitude;
        float verticalDistance = Mathf.Abs(directionToPlayer.y);

        // ���� ���� ���� �ִ��� Ȯ��
        if (verticalDistance > verticalRange)
        {
            Debug.Log("�÷��̾ ���� ���� �ۿ� �ֽ��ϴ�.");
            return;
        }

        // �þ߰� ���� �ִ��� Ȯ��
        float angleToPlayer = Vector3.Angle(npcTransform.forward, directionToPlayer);
        if (angleToPlayer <= fovAngle && horizontalDistance <= detectionRange)
        {
            Debug.Log("�÷��̾ NPC�� �þ߰� ���� �ֽ��ϴ�.");
        }
        else
        {
            Debug.Log("�÷��̾ NPC�� �þ߰� �ۿ� �ֽ��ϴ�.");
        }
    }

    void OnDrawGizmos()
    {
        if (npcTransform == null) return;

        Gizmos.color = Color.blue;

        Vector3 forward = npcTransform.forward;
        Vector3 npcPosition = npcTransform.position;

        // �þ߰��� ������ �׸���
        for (float y = -verticalRange; y <= verticalRange; y += 0.5f)
        {
            Vector3 heightOffset = Vector3.up * y;
            for (float angle = -fovAngle; angle <= fovAngle; angle += 1.0f)
            {
                Vector3 direction = Quaternion.Euler(0, angle, 0) * forward * detectionRange;
                Gizmos.DrawRay(npcPosition + heightOffset, direction);
            }
        }

        // ���� ������ ��ܰ� �ϴܿ� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawLine(npcPosition + Vector3.up * verticalRange, npcPosition + forward * detectionRange + Vector3.up * verticalRange);
        Gizmos.DrawLine(npcPosition - Vector3.up * verticalRange, npcPosition + forward * detectionRange - Vector3.up * verticalRange);
    }
}
