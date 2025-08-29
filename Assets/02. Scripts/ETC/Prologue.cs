using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    [Header("커서 데이터베이스")]
    [SerializeField] private CursorDataBase m_cursor_db;

    [Header("프롤로그 관련 컴포넌트")]
    [Header("섬 이미지")]
    [SerializeField] private Image m_island_image;

    [Header("프롤로그 텍스트")]
    [SerializeField] private TMP_Text m_prologue_text;
    private string[] m_prologue_strings;

    private void Awake()
    {
        m_prologue_strings = new string[]
        {
            "라피는 주인님과 함께 사는 애완토끼입니다.",
            "오늘은 주인님이 게임을 하는 모습을 보며 잠에 들기로 했습니다.",
            "주인님과 함께 뛰어노는 행복한 꿈을 꾸기로 합니다.",
            "잠에서 깬 라피는 자신이 잠들었던 곳이 아닌 다른 곳에서 깨어났습니다.",
            "라피의 이야기는 여기서부터 시작됩니다."
        };

        m_prologue_text.color = new Color(1f, 1f, 1f, 0f);

        m_cursor_db.SetCursor(CursorMode.DEFAULT);
    }

    private void Start()
    {
        StartCoroutine(IntroTextStart(0));
    }

    private IEnumerator IntroTextStart(int index)
    {
        if(index == 0)
        {
            yield return new WaitForSeconds(1f);
        }

        if(index == 3)
        {
            StartCoroutine(PrintIsland());
        }

        float target_time = 2f;
        float elapsed_time = 0f;

        Color text_color = m_prologue_text.color;
        m_prologue_text.text = m_prologue_strings[index];

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            m_prologue_text.color = new Color(text_color.r, text_color.g, text_color.b, Mathf.Lerp(0f, 1f, elapsed_time / target_time));

            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        elapsed_time = 0f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            m_prologue_text.color = new Color(text_color.r, text_color.g, text_color.b, Mathf.Lerp(1f, 0f, elapsed_time / target_time));

            yield return null;
        }

        if(index + 1 <= m_prologue_strings.Length - 1)
        {
            StartCoroutine(IntroTextStart(index + 1));
        }
        else
        {
            LoadingManager.Instance.LoadScene("Sprout Island");
            yield break;
        }
    }  

    private IEnumerator PrintIsland()
    {
        yield return new WaitForSeconds(1.5f);
        float target_time = 3f;
        float elapsed_time = 0f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            m_island_image.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 0.7f, elapsed_time / target_time));

            yield return null;
        }
        m_island_image.color = new Color(1f, 1f, 1f, 0.7f);

        yield return new WaitForSeconds(3f);
        elapsed_time = 0f;

        while(elapsed_time < target_time)
        {
            elapsed_time += Time.deltaTime;

            m_island_image.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.7f, 0f, elapsed_time / target_time));

            yield return null;
        }
        m_island_image.color = new Color(1f, 1f, 1f, 0f);
    }  
}
