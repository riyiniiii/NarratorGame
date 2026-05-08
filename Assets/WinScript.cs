using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private int pointsToWin;

    private int currentPoints;

    public GameObject Scales;

    [SerializeField] private string puzzleSceneName = "SRN_PUZZLE";
    [SerializeField] private string postPuzzleKnot;
    [SerializeField] private TextAsset postPuzzleDialogue;

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

        // UNLOCK DIALOGUE
        DialogueManager.GetInstance().UnlockDialogue();

        // UNLOAD PUZZLE
        SceneManager.UnloadSceneAsync(puzzleSceneName);
    }
    
    
}