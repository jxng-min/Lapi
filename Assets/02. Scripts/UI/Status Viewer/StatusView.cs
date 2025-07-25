using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour, IStatusView
{
    [Header("UI 관련 컴포넌트")]
    [Header("레벨")]
    [SerializeField] private TMP_Text m_level_label;

    [Header("경험치")]
    [SerializeField] private Slider m_exp_slider;

    [Header("체력")]
    [SerializeField] private Slider m_hp_slider;

    [Header("마나")]
    [SerializeField] private Slider m_mp_slider;

    public void UpdateLV(int level, float exp_rate)
    {
        m_level_label.text = $"LV.{level}";
        StartCoroutine(UpdateSlider(m_exp_slider, exp_rate));
    }

    public void UpdateHP(float hp_rate)
    {
        StartCoroutine(UpdateSlider(m_hp_slider, hp_rate));
    }

    public void UpdateMP(float mp_rate)
    {
        StartCoroutine(UpdateSlider(m_mp_slider, mp_rate));
    }

    private IEnumerator UpdateSlider(Slider slider, float rate)
    {
        var elapsed_time = 0f;
        var target_time = 1f;

        while (elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;

            var delta = elapsed_time / target_time;
            slider.value = Mathf.Lerp(slider.value, rate, delta);

            yield return null;
        }

        slider.value = rate;
    }
}
