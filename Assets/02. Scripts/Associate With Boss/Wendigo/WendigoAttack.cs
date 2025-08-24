using UnityEngine;

public class WendigoAttack : BossAttack
{
    [Header("Swing")]
    [SerializeField] private GameObject[] m_swing_colliders;

    [Header("Earthquake")]
    [SerializeField] private GameObject m_earthquake_collider;

    public void DownSwingBegin()
    {
        m_swing_colliders[0].SetActive(true);
    }

    public void DownSwingEnd()
    {
        m_swing_colliders[0].SetActive(false);
    }

    public void UpSwingBegin()
    {
        m_swing_colliders[1].SetActive(true);
    }

    public void UpSwingEnd()
    {
        m_swing_colliders[1].SetActive(false);
    }

    public void LeftSwingBegin()
    {
        m_swing_colliders[2].SetActive(true);
    }

    public void LeftSwingEnd()
    {
        m_swing_colliders[2].SetActive(false);
    }

    public void RightSwingBegin()
    {
        m_swing_colliders[3].SetActive(true);
    }

    public void RightSwingEnd()
    {
        m_swing_colliders[3].SetActive(false);
    }

    public void EarthquakeBegin()
    {
        m_earthquake_collider.SetActive(true);
    }

    public void EarthquakeEnd()
    {
        m_earthquake_collider.SetActive(false);
    }
}
