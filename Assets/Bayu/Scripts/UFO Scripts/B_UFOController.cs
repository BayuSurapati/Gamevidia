using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static B_DailogueData;
using static B_UFOState;
using Unity.VisualScripting;

public class B_UFOController : MonoBehaviour
{
    [Header("Dialogue")]
    public B_DailogueData[] dialogues;
    public TextMeshProUGUI[] dialogueTextUI;

    [Header("DialogueBox")]
    public GameObject[] dialogueBox;

    [Header("TypingSpee")]
    public float typingSpeed = .25f;

    private GameObject currentBox;
    private TextMeshProUGUI currentText;
    private Coroutine typingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        HideAllDialogue();
        //B_AudioManager.Instance.SetSFXVolume(.5f);
        //B_AudioManager.Instance.PlayLoopSFX(7);
        B_AudioManager.Instance.PlayBGM(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    public void ShowDialogue(B_UFOState state)
    {
        B_DailogueData data = GetDialogueByState(state);
        if (data == null) return;

        PickRandomDialogueBox();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentBox.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(data.dialogueText));
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (currentBox != null)
            currentBox.SetActive(false);
    }

    void PickRandomDialogueBox()
    {
        int index = Random.Range(0, dialogueBox.Length);

        currentBox = dialogueBox[index];
        currentText = dialogueTextUI[index];

        currentText.text = "";
    }

    void HideAllDialogue()
    {
        foreach (GameObject box in dialogueBox)
            box.SetActive(false);
    }

    B_DailogueData GetDialogueByState(B_UFOState state)
    {
        foreach (B_DailogueData d in dialogues)
        {
            if (d.State == state)
                return d;
        }
        return null;
    }

    IEnumerator TypeText(string text)
    {
        currentText.text = "";

        foreach (char c in text)
        {
            currentText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
