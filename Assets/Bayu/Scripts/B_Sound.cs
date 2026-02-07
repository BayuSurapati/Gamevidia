using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Sound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        B_AudioManager.Instance.PlayBGM(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
