using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_HazardKill : MonoBehaviour
{
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
            B_PlayerHealthController health =
                collision.GetComponent<B_PlayerHealthController>();

            if (health != null)
            {
                health.Die();
            }
        }
    }
}
