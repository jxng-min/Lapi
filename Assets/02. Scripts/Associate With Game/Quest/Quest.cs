using InventoryService;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "SO/Create New Quest")]
public class Quest : ScriptableObject
{
    [Header("퀘스트 ID")]
    [SerializeField] private int m_quest_id;
    public int ID => m_quest_id;

    [Space(50f)]
    [Header("퀘스트 조건")]
    [SerializeField] private KillQuest[] m_kill_quest_list;
    public KillQuest[] KillQuest => m_kill_quest_list;

    [SerializeField] private ItemQuest[] m_item_quest_list;
    public ItemQuest[] ItemQuest => m_item_quest_list;

    [SerializeField] private DialogueQuest[] m_dialogue_quest_list;
    public DialogueQuest[] DialogueQuest => m_dialogue_quest_list;

    [Space(10f)]
    [Header("퀘스트와 관련된 대화")]
    [Header("퀘스트 시작")]
    [SerializeField] private int m_start_diaglogue_id;
    public int StartDialogue => m_start_diaglogue_id;

    [Header("퀘스트 중")]
    [SerializeField] private int m_progress_dialogue_id;
    public int ProgressDialogue => m_progress_dialogue_id;

    [Header("퀘스트 완료 가능")]
    [SerializeField] private int m_can_clear_dialogue_id;
    public int CanClearDialogue => m_can_clear_dialogue_id;

    [Space(10f)]
    [Header("퀘스트 해금")]
    [Header("퀘스트 해금 레벨")]
    [SerializeField] private int m_quest_level;
    public int Level => m_quest_level;

    [Header("선행 퀘스트 목록")]
    [SerializeField] private Quest[] m_previous_quest_list;
    public Quest[] Previous => m_previous_quest_list;

    [Space(10f)]
    [Header("퀘스트 보상")]
    [Header("골드")]
    [SerializeField] private int m_gold;
    public int RewardGold => m_gold;

    [Header("경험치")]
    [SerializeField] private int m_exp;
    public int RewardEXP => m_exp;

    [Header("아이템")]
    [SerializeField] private ItemData[] m_item_list;
    public ItemData[] RewardItem => m_item_list;
}
