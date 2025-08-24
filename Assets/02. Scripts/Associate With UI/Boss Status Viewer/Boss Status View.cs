using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossStatusView : MonoBehaviour, IBossStatusView
{
    [Header("HP 슬라이더")]
    [SerializeField] private Slider m_hp_slider;

    [Header("보스 이름")]
    [SerializeField] private TMP_Text m_boss_name;

    private Coroutine m_slider_coroutine;

    public void OpenUI(string name)
    {
        m_hp_slider.gameObject.SetActive(true);
        m_boss_name.text = name;
    }

    public void UpdateUI(float hp_rate)
    {
        if (m_slider_coroutine != null)
        {
            StopCoroutine(m_slider_coroutine);
            m_slider_coroutine = null;
        }

        m_slider_coroutine = StartCoroutine(UpdateSlider(hp_rate));
        m_hp_slider.value = hp_rate;

        if (hp_rate >= 1f)
        {
            CloseUI();
        }
    }

    public void CloseUI()
    {
        m_hp_slider.gameObject.SetActive(false);
    }

    private IEnumerator UpdateSlider(float hp_rate)
    {
        float elapsed_time = 0f;
        float target_time = 1f;

        while (elapsed_time <= target_time)
        {
            yield return new WaitUntil(() => GameManager.Instance.Event != GameEventType.SETTING);

            var delta = elapsed_time / target_time;
            m_hp_slider.value = Mathf.Lerp(m_hp_slider.value, hp_rate, delta);

            elapsed_time += Time.deltaTime;
            yield return null;
        }

        m_hp_slider.value = hp_rate;
    }
}
