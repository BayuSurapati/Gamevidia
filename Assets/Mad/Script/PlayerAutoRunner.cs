using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoRunner : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 3f;

    [Header("Visual")]
    public SpriteRenderer sr;
    public Animator anim;

    [Header("Animator parameters")]
    public string walkBool = "isWalking";

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public IEnumerator MoveTo(Vector3 target)
    {
        SetWalking(true);

        while (Vector3.Distance(transform.position, target) > 0.02f)
        {
            Vector3 dir = (target - transform.position);

            // flip sprite sesuai arah
            if (sr != null)
            {
                if (dir.x > 0.01f) sr.flipX = false;
                else if (dir.x < -0.01f) sr.flipX = true;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target;
        SetWalking(false);
    }

    void SetWalking(bool value)
    {
        if (anim != null)
            anim.SetBool(walkBool, value);
    }
    // Start is called before the first frame update
    public IEnumerator FollowPath(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            yield return MoveTo(points[i]);
        }
    }
    public IEnumerator IntroEnter(Vector3 from, Vector3 to)
    {
        SetPosition(from);
        yield return MoveTo(to);
    }

}
