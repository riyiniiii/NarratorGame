using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class ShellGameManager : MonoBehaviour
{
    [Header("Shells")]
    [SerializeField] private GameObject[] shells;

    [Header("Settings")]
    [SerializeField] private int numberOfShuffles = 5;
    [SerializeField] private float shuffleSpeed = 0.3f;
    [SerializeField] private float liftHeight = 1.5f;
    [SerializeField] private float liftDuration = 0.4f;

    [Header("Ball")]
    [SerializeField] private GameObject ball;

    [Header("UI")]
    [SerializeField] private GameObject correctText;
    [SerializeField] private GameObject wrongText;

    private int correctShellIndex;
    private bool playerCanGuess = false;
    private bool gameFinished = false;

    private CharacterController2D characterController;
    private CinemachineCamera virtualCamera;

    void Start()
    {
        characterController = FindObjectOfType<CharacterController2D>();
        virtualCamera = FindObjectOfType<CinemachineCamera>();

        if (characterController != null)
        {
            characterController.canMove = false;
            characterController.SetHidden(true);
            Debug.Log("Player found and locked!");
        }
        else
        {
            Debug.LogWarning("Player NOT found!");
        }

        if (virtualCamera != null)
        {
            virtualCamera.enabled = false;
            Debug.Log("Camera found and locked!");
        }
        else
        {
            Debug.LogWarning("Camera NOT found!");
        }

        ball.SetActive(false);
        StartCoroutine(StartGame());
    }
    
    private IEnumerator StartGame()
    {
        correctShellIndex = Random.Range(0, shells.Length);

        // Position ball under correct shell
        ball.transform.position = shells[correctShellIndex].transform.position;
        ball.SetActive(true);

        // Lift all shells to reveal ball
        foreach (GameObject shell in shells)
        {
            StartCoroutine(LiftShell(shell));
        }
        yield return new WaitForSeconds(1.5f);

        // Lower all shells to hide ball
        foreach (GameObject shell in shells)
        {
            StartCoroutine(LowerShell(shell));
        }
        yield return new WaitForSeconds(1f);

        // Hide ball
        ball.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        // Shuffle
        yield return StartCoroutine(Shuffle());

        // Let player guess
        playerCanGuess = true;
        foreach (GameObject shell in shells)
        {
            shell.GetComponent<Shell>().SetClickable(true);
        }
    }

    private IEnumerator LiftShell(GameObject shell)
    {
        Vector3 startPos = shell.transform.position;
        Vector3 endPos = startPos + new Vector3(0f, liftHeight, 0f);
        float timer = 0f;

        while (timer < liftDuration)
        {
            timer += Time.deltaTime;
            shell.transform.position = Vector3.Lerp(startPos, endPos, timer / liftDuration);
            yield return null;
        }

        shell.transform.position = endPos;
    }

    private IEnumerator LowerShell(GameObject shell)
    {
        Vector3 startPos = shell.transform.position;
        Vector3 endPos = startPos - new Vector3(0f, liftHeight, 0f);
        float timer = 0f;

        while (timer < liftDuration)
        {
            timer += Time.deltaTime;
            shell.transform.position = Vector3.Lerp(startPos, endPos, timer / liftDuration);
            yield return null;
        }

        shell.transform.position = endPos;
    }

    private IEnumerator Shuffle()
    {
        for (int i = 0; i < numberOfShuffles; i++)
        {
            int a = Random.Range(0, shells.Length);
            int b;
            do { b = Random.Range(0, shells.Length); } while (b == a);

            yield return StartCoroutine(SwapShells(a, b));

            // Track where the correct shell moved
            if (correctShellIndex == a) correctShellIndex = b;
            else if (correctShellIndex == b) correctShellIndex = a;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SwapShells(int a, int b)
    {
        Vector3 posA = shells[a].transform.position;
        Vector3 posB = shells[b].transform.position;

        float timer = 0f;
        while (timer < shuffleSpeed)
        {
            timer += Time.deltaTime;
            float t = timer / shuffleSpeed;
            shells[a].transform.position = Vector3.Lerp(posA, posB, t);
            shells[b].transform.position = Vector3.Lerp(posB, posA, t);
            yield return null;
        }

        shells[a].transform.position = posB;
        shells[b].transform.position = posA;
    }

    public void OnShellClicked(GameObject clickedShell)
    {
        if (!playerCanGuess || gameFinished) return;

        playerCanGuess = false;
        gameFinished = true;

        int clickedIndex = System.Array.IndexOf(shells, clickedShell);
        StartCoroutine(RevealResult(clickedIndex));
    }

    private IEnumerator RevealResult(int clickedIndex)
    {
        // Show ball and lift shells
        ball.transform.position = shells[correctShellIndex].transform.position;
        ball.SetActive(true);

        foreach (GameObject shell in shells)
        {
            StartCoroutine(LiftShell(shell));
        }

        yield return new WaitForSeconds(1f);

        if (clickedIndex == correctShellIndex)
        {
            correctText.SetActive(true);
            yield return new WaitForSeconds(2f);
            correctText.SetActive(false);
        }
        else
        {
            wrongText.SetActive(true);
            yield return new WaitForSeconds(2f);
            wrongText.SetActive(false);
        }
        

        if (virtualCamera != null) virtualCamera.enabled = true;
        if (characterController != null)
        {
            characterController.canMove = true;
            characterController.SetHidden(false); // show player again
        }

        // Unlock dialogue and unload puzzle scene
        DialogueManager.GetInstance().UnlockDialogue();
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }
}