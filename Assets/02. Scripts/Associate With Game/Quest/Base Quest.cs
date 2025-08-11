using UnityEngine;

[System.Serializable]
public abstract class BaseQuest
{
    [Header("서브 퀘스트 ID")]
    [SerializeField] private int m_subquest_id = -1;

    public int ID => m_subquest_id;
    public bool Clear { get; set; }

    public abstract string GetFormatText();
}