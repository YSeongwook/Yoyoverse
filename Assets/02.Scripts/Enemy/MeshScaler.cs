using UnityEngine;

public class MeshScaler : MonoBehaviour
{
    public Vector3 scale = new Vector3(1, 1, 1);
    [Range(0.01f, 90f)]
    public float viewAngle = 45f;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateProperties();
    }

    void Update()
    {
        UpdateProperties();
    }

    void UpdateProperties()
    {
        if (meshRenderer != null && meshRenderer.sharedMaterial != null)
        {
            // ���� ��忡���� Material�� ���� �������� �ʽ��ϴ�.
            if (Application.isPlaying)
            {
                meshRenderer.sharedMaterial.SetVector("_Scale", new Vector4(scale.x, scale.y, scale.z, 1));
                meshRenderer.sharedMaterial.SetFloat("_ViewAngle", viewAngle);
            }

            // Transform ������ ������Ʈ�� ���� ��忡���� �����մϴ�.
            meshRenderer.transform.localScale = scale;
        }
    }
}