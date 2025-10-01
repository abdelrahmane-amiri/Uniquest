using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    private bool endLine = false;
    public bool selectClass = false;
    public bool endDialog = false;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (endLine && Input.GetKeyDown(KeyCode.Space))
        {
            if (index >= lines.Length - 1)
            {
                selectClass = true;
                return;
            }
            endLine = false;
            StartCoroutine(NextLine());
            Debug.Log("Prochaine Ligne");
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        endLine = true;
        Debug.Log("Fin Ligne");
    }

    IEnumerator NextLine()
    {
        index++;
        textComponent.text = string.Empty;
        yield return StartCoroutine(TypeLine());
    }
}
