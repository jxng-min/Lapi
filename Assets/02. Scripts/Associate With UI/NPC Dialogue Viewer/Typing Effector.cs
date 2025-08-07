using TMPro;
using UnityEngine;

public class TypingEffector : MonoBehaviour
{
    [Header("컨텍스트 라벨")]
    [SerializeField] private TMP_Text m_context_label;

    [Header("1초 당 출력될 문자의 개수")]
    [SerializeField] private int m_cps;

    private string m_target_string;
    private int m_current_index;
    private float m_interval;
    private bool m_is_effecting;

    #region Properties
    public bool IsEffecting { get => m_is_effecting; }
    #endregion Properties

    #region Helper Method
    public void SetContext(string context)
    {
        if (m_is_effecting)
        {
            m_context_label.text = m_target_string;

            CancelInvoke();
            EffectExit();
        }

        m_target_string = context;

        EffectEnter();
    }

    private void EffectEnter()
    {
        m_context_label.text = "";
        m_current_index = 0;
        m_is_effecting = true;

        m_interval = 1f / m_cps;

        Invoke("Effecting", m_interval);
    }

    private void Effecting()
    {
        if (m_context_label.text == m_target_string)
        {
            EffectExit();
            return;
        }

        m_context_label.text += m_target_string[m_current_index++];

        Invoke("Effecting", m_interval);
    }

    private void EffectExit()
    {
        m_is_effecting = false;
    }
    #endregion Helper Method
}