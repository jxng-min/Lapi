using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
    #region Variables
    [Header("캔버스 그룹")]
    [SerializeField] private CanvasGroup m_canvas_group;

    [Header("로딩 애니메이터")]
    [SerializeField] private Animator m_loading_animator;

    private string m_target_scene;
    #endregion Variables

    #region Properties
    public string Scene { get => m_target_scene; }
    #endregion Properties

    #region Helper Methods
    public void LoadScene(string scene_name)
    {
        gameObject.SetActive(true);

        SceneManager.sceneLoaded += OnSceneLoaded;

        m_target_scene = scene_name;

        DIContainer.Clear();

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        m_canvas_group.interactable = true;
        m_canvas_group.blocksRaycasts = true;

        yield return StartCoroutine(Fade(true));

        m_loading_animator.SetBool("Loading", true);

        var op = SceneManager.LoadSceneAsync(m_target_scene);
        op.allowSceneActivation = false;

        float elapsed_time = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(op.progress / 1f);

                m_loading_animator.SetFloat("Progress", progress);
            }
            else
            {
                elapsed_time += Time.unscaledDeltaTime;

                float wait_progress = Mathf.Clamp01(0.9f + (0.1f * elapsed_time / 3f));
                m_loading_animator.SetFloat("Progress", wait_progress);

                if (elapsed_time >= 3f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private IEnumerator Fade(bool is_fade_in)
    {
        float elapsed_time = 0f;
        float target_time = 1f;


        while(elapsed_time <= target_time)
        {
            elapsed_time += Time.deltaTime;
            yield return null;

            m_canvas_group.alpha = is_fade_in ? Mathf.Lerp(0f, 1f, elapsed_time) : Mathf.Lerp(1f, 0f, elapsed_time);
        }

        if(!is_fade_in)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == m_target_scene)
        {
            m_loading_animator.SetBool("Loading", false);
            
            m_canvas_group.interactable = false;
            m_canvas_group.blocksRaycasts = false;

            StartCoroutine(Fade(false));
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    #endregion Helper Methods
}