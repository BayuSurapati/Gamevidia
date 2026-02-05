using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_DialogueTrigger : MonoBehaviour
{
    public B_DialogueManager dialogueManager;
    public B_DialogueData[] dialogues;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScripts player = collision.GetComponent<PlayerScripts>();
            dialogueManager.StartDialogue(dialogues, player);
            gameObject.SetActive(false); // optional: sekali saja
        }
    }
}
