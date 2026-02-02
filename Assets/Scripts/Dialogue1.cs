using UnityEngine;
using TMPro;

public class Dialogue1 : MonoBehaviour
{
    public TextMeshProUGI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    void StartDialogue()
    {
        index = 0;
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index]ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
