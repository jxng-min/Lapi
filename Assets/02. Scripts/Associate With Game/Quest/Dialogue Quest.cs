using UnityEngine;

[System.Serializable]
public class DialogueQuest : BaseQuest
{
    [Header("NPC 코드")]
    [SerializeField] private NPCCode m_npc_code;

    [Header("총 대화 횟수")]
    [SerializeField] private int m_total_dialogue_count;

    private int m_current_dialogue_count;

    public int Total => m_total_dialogue_count;
    public int Current => m_current_dialogue_count;

    public override string GetFormatText()
    {
        return $"{Mathf.Clamp(Current, 0, Total)}/{Total}";
    }
}
