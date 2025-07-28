using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("추적 대상")]
    [SerializeField] private Transform m_transform;

    private void LateUpdate()
    {
        transform.position = new Vector3(m_transform.position.x, m_transform.position.y, -10f);
    }
}
