using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class NextSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector pd)
    {
        SceneManager.LoadScene(sceneName);
    }
}