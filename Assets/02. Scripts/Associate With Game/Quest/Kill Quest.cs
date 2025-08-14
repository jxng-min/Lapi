using UnityEngine;

[System.Serializable]
public class KillQuest : BaseQuest
{
    [Header("몬스터 코드")]
    [SerializeField] private EnemyCode m_enemy_code;

    [Header("총 처치 횟수")]
    [SerializeField] private int m_total_kill_count;

    private int m_current_kill_count;

    public EnemyCode Code => m_enemy_code;
    public int Total => m_total_kill_count;
    public int Current => m_current_kill_count;

    public override string GetFormatText()
    {
        return $"[{Mathf.Clamp(Current, 0, Total)}/{Total}]";
    }
}
