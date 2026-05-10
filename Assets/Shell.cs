using UnityEngine;
 
public class Shell : MonoBehaviour
{
    private ShellGameManager gameManager;
    private bool clickable = false;
 
    void Start()
    {
        gameManager = FindObjectOfType<ShellGameManager>();
    }
 
    public void SetClickable(bool state)
    {
        clickable = state;
    }
    void OnMouseDown()
    {
        if (!clickable) return;
        gameManager.OnShellClicked(gameObject);
    }
}