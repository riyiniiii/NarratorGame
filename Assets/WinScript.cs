using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private int pointsToWin;
    private int currentPoints;

    public GameObject Scales;

    [SerializeField] private string puzzleSceneName = "SRN_PUZZLE";

    private bool puzzleCompleted = false;

    void Start()
    {
        pointsToWin = Scales.transform.childCount;
    }

    void Update()
    {
        if (!puzzleCompleted && currentPoints >= pointsToWin)
        {
            puzzleCompleted = true;
            PuzzleComplete();
        }
    }

    public void AddPoints()
    {
        currentPoints++;
        Debug.Log("Points: " + currentPoints + "/" + pointsToWin);
    }

    private void PuzzleComplete()
    {
        Debug.Log("Puzzle Completed!");

        // Unlock dialogue and trigger post-puzzle conversation
        DialogueManager.GetInstance().UnlockDialogue();

        // Unload puzzle scene
        SceneManager.UnloadSceneAsync(puzzleSceneName);
    }
}