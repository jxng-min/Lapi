using System;
using System.Collections;
using KeyService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinderSlotView : MonoBehaviour
{
    private KeyCode m_origin_key_code;
    private IKeyService m_key_service;

    [Header("매핑 문자열")]
    [SerializeField] private string m_key_name;

    [Header("바인딩 버튼")]
    [SerializeField] private Button m_binding_button;

    [Header("버튼 텍스트")]
    [SerializeField] private TMP_Text m_button_text;

    [Header("예외 텍스트")]
    [SerializeField] private TMP_Text m_wrong_text;

    private Coroutine m_wrong_key_coroutine;

    private void Awake()
    {
        m_binding_button.onClick.AddListener(ModifyKey);
    }

    public void Inject(IKeyService key_service)
    {
        m_key_service = key_service;

        m_origin_key_code = m_key_service.GetKeyCode(m_key_name);
        m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
    }

    public void ModifyKey()
    {
        m_button_text.text = "-";

        StartCoroutine(Co_AssignKey());
    }

    private IEnumerator Co_AssignKey()
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(code))
                    {
                        if (m_key_service.Check(code, m_origin_key_code))
                        {
                            m_key_service.Register(code, m_key_name);
                            m_origin_key_code = code;

                            m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();
                        }
                        else
                        {
                            m_button_text.text = ((char)m_origin_key_code).ToString().ToUpper();

                            if (m_wrong_key_coroutine != null)
                            {
                                StopCoroutine(m_wrong_key_coroutine);
                                m_wrong_key_coroutine = null;
                            }
                            m_wrong_key_coroutine = StartCoroutine(Co_WrongKey());
                        }
                    }
                }

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Co_WrongKey()
    {
        float elapsed_time = 0f;
        float target_time = 1f;

        while (elapsed_time < target_time)
        {
            float delta = elapsed_time / target_time;
            SetAlpha(delta);

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
        elapsed_time = 0f;

        while (elapsed_time < target_time)
        {
            float delta = elapsed_time / target_time;
            SetAlpha(1 - delta);

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        m_wrong_key_coroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        var color = m_wrong_text.color;
        color.a = alpha;
        m_wrong_text.color = color;
    }
}
