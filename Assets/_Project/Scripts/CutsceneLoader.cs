using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map";
    [SerializeField] private PlayableDirector director;

    void Start()
    {
        if (director != null)
            director.stopped += OnCutsceneFinished;
    }

    void OnCutsceneFinished(PlayableDirector pd)
    {
        if (pd.time >= pd.duration)
            LoadNextScene();
    }

    public void SkipCutscene()
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (director != null)
            director.stopped -= OnCutsceneFinished;

        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnCutsceneFinished;
    }
}