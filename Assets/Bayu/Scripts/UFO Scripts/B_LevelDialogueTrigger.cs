using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LevelDialogueTrigger : MonoBehaviour
{
    public B_UFOState triggerState;
    private bool isTriggered;
    private B_UFOController ufo;
    // Start is called before the first frame update
    void Start()
    {
        ufo = FindObjectOfType<B_UFOController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ufo.ShowDialogue(triggerState);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        ufo.HideDialogue();
    }
}
