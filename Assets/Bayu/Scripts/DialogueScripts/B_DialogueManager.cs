using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class B_DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public GameObject leftSpeaker;
    public GameObject rightSpeaker;

    public Image leftPotrait;
    public TMP_Text leftSpeakerText;

    public Image rightPotrait;
    public TMP_Text rightSpeakerText;

    public TMP_Text dialogueText;

    [Header("Animation")]
    public float popDuration = .25f;

    [Header("Timing")]
    public float speakerDelay = 1f;
    public float dialogPopDuration = 1f;

    private int index;
    private B_DialogueData[] dialogues;

    private PlayerScripts player;
    private bool dialogStarted;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        dialoguePanel.SetActive(false);
        leftSpeaker.SetActive(false);
        rightSpeaker.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        inputActions.Dialog.Enable();
        inputActions.Dialog.Next.performed += OnNextPerformed;
    }

    private void OnDisable()
    {
        inputActions.Dialog.Next.performed -= OnNextPerformed;
        inputActions.Dialog.Disable();
    }

    void OnNextPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!dialoguePanel.activeSelf) return;
        NextDialogue();
    }

    public void StartDialogue(B_DialogueData[] newDialogues, PlayerScripts playerRef)
    {
        dialogues = newDialogues;
        index = 0;
        dialogStarted = false;
        player = playerRef;

        player.canMove = false;

        StartCoroutine(DialogSequence());
    }

    IEnumerator DialogSequence()
    {
        if (!dialogStarted)
        {
            dialoguePanel.SetActive(true);
            yield return StartCoroutine(PopUp(dialoguePanel.transform));
            dialogStarted = true;
        }
        ShowLine();
    }

    public void NextDialogue()
    {
        index++;

        if (index >= dialogues.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        B_DialogueData line = dialogues[index];

        leftSpeaker.SetActive(false);
        rightSpeaker.SetActive(false);

        StartCoroutine(ShowSpeaker(line));
        dialogueText.text = line.dialogueText;
    }

    IEnumerator ShowSpeaker(B_DialogueData line)
    {
        yield return new WaitForSeconds(speakerDelay);

        if (line.isLeftSpeaker)
        {
            leftSpeaker.SetActive(true);
            leftSpeakerText.text = line.speakerName;
            leftPotrait.sprite = line.speakerSprite;
            yield return StartCoroutine(PopUp(leftSpeaker.transform));
        }
        else
        {
            rightSpeaker.SetActive(true);
            rightSpeakerText.text = line.speakerName;
            rightPotrait.sprite = line.speakerSprite;
            yield return StartCoroutine(PopUp(rightSpeaker.transform));
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        leftSpeaker.SetActive(false);
        rightSpeaker.SetActive(false);

        player.canMove = true;
    }
    IEnumerator PopUp(Transform target)
    {
        target.localScale = Vector3.zero;
        float t = 0;

        while (t < popDuration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / popDuration);
            yield return null;
        }

        target.localScale = Vector3.one;
    }
}
