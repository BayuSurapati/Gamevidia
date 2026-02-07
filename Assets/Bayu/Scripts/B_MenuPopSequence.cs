using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_MenuPopSequence : MonoBehaviour
{
    public Animator[] popObjects;
    public float delayBetweenObjects = .2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlaySequence()
    {
        foreach (Animator anim in popObjects)
        {
            anim.gameObject.SetActive(true);
            anim.SetTrigger("Pop");
            B_AudioManager.Instance.PlaySFX(10);
            yield return new WaitForSeconds(delayBetweenObjects);
        }
    }
}
