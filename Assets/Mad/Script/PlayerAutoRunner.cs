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

    [Header("Intro Waypoints")]
    public Transform[] introWaypoints;
    [field: SerializeField]
    public bool IsDead { get; private set; }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public IEnumerator MoveTo(Vector3 target)
    {
        if (IsDead) yield break;

        SetWalking(true);

        while (!IsDead && Vector3.Distance(transform.position, target) > 0.02f)
        {
            Vector3 dir = target - transform.position;

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

        SetWalking(false);
    }

    public void SetWalking(bool value)
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
    public IEnumerator FollowWaypointsIntro()
    {
        if (introWaypoints == null || introWaypoints.Length == 0) yield break;

        foreach (var wp in introWaypoints)
        {
            yield return MoveTo(wp.position);

            if (wp.CompareTag("Trap"))
            {
                GameManager.Instance.introPlayerDead = true;
                yield break; // BERHENTI TOTAL
            }
        }
    }
    public void Die()
    {
        if (IsDead) return;

        IsDead = true;

        SetWalking(false);

        if (anim != null)
        {
            anim.ResetTrigger("Death"); // safety
            anim.SetTrigger("Death");
        }
    }
    public void Revive()
    {
        IsDead = false;

        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
            anim.Play("idle", -1, 0f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            // intro trap ditangani lewat waypoint, bukan trigger
            if (GameManager.Instance.isIntro) return;

            GameManager.Instance.PlayerFall();
            return;
        }

        if (other.CompareTag("Portal"))
        {
            GameManager.Instance.PlayerReachPortal();
        }
    }
}
