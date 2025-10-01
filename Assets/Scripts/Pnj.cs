using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pnj : MonoBehaviour
{
    public GameObject panel;
    public GameObject nameBox;
    public TextMeshProUGUI nameComponent;
    public GameObject imageBox;
    public GameObject npc;
    public bool isPlay;
    private bool playerIn = false;
    private string npcName;

    [SerializeField] Dialogue Dialogue;

    void OnTriggerEnter2D(Collider2D other)
    {
        playerIn = true;
        Debug.Log("Entrer du Joueur");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        playerIn = false;
        Debug.Log("Sortie du joueur");
    }

    void Awake()
    {
        npcName = gameObject.name;
        nameComponent.text = npcName;
    }
    void Update()
    {
        if (playerIn && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Debut du dialogue");
            isPlay = true;
            panel.SetActive(true);
            nameBox.SetActive(true);
            imageBox.SetActive(true);
            npc.SetActive(true);
        }

        if (Dialogue.endDialog)
        {
            isPlay = false;
            panel.SetActive(false);
            nameBox.SetActive(false);
            imageBox.SetActive(false);
            npc.SetActive(false);
        }
    }
}
