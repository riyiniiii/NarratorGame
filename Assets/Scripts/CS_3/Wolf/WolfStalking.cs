using UnityEngine;
using System.Collections;

public class WolfStalking : MonoBehaviour
{
    public Transform player;

    public float normalSpeed = 5f;
    public float slowedSpeed = 0.5f;
    public float speedChangeRate = 3f;

    public float stopDistance = 1.5f;

    private float currentSpeed;

    public bool stalkingActive;
    public bool eventControlled = false;

    private SpriteRenderer sr;
    private Coroutine vanishRoutine;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (player == null) return;
        if (!stalkingActive) return;

        bool isLooking = IsPlayerLookingAtWolf();

  
        if (stalkingActive)
        {
            float targetSpeed = isLooking ? slowedSpeed : normalSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }

        FollowPlayer();

        HandleVisibility(isLooking);
    }



    private void FollowPlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= stopDistance) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * currentSpeed * Time.deltaTime);
    }

    

    private void HandleVisibility(bool isLooking)
    {
        if (!stalkingActive) return;

        if (isLooking)
        {
            if (vanishRoutine == null)
                vanishRoutine = StartCoroutine(Vanish());
        }
        else
        {
            if (vanishRoutine != null)
            {
                StopCoroutine(vanishRoutine);
                vanishRoutine = null;
            }

            Appear();
        }
    }

    private void Appear()
    {
        if (!sr.enabled) sr.enabled = true;

        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * 4f);
        sr.color = c;
    }

    private IEnumerator Vanish()
    {
        float duration = 0.25f;
        float t = 0f;

        Color start = sr.color;

        while (t < duration)
        {
            t += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            sr.color = new Color(start.r, start.g, start.b, alpha);

            yield return null;
        }

        sr.color = new Color(start.r, start.g, start.b, 0f);

       
        sr.enabled = false;

        vanishRoutine = null;
    }

    

    private bool IsPlayerLookingAtWolf()
    {
        SpriteRenderer psr = player.GetComponent<SpriteRenderer>();
        if (psr == null) return false;

        bool facingRight = !psr.flipX;
        bool wolfRight = transform.position.x > player.position.x;

        return (facingRight && wolfRight) || (!facingRight && !wolfRight);
    }

    

    public void ActivateStalking()
    {
        stalkingActive = true;
        eventControlled = false;
    }
    
    public void SetEventMode()
    {
        stalkingActive = false;
        eventControlled = true;

        // stop any vanish coroutine so wolf is fully visible for cutscene
        if (vanishRoutine != null)
        {
            StopCoroutine(vanishRoutine);
            vanishRoutine = null;
        }

        // ensure renderer is on and visible
        if (sr != null)
        {
            sr.enabled = true;
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }
    
    public void ForceVanish()
    {
        stalkingActive = false;
        eventControlled = false;

        if (vanishRoutine != null)
        {
            StopCoroutine(vanishRoutine);
            vanishRoutine = null;
        }

        StartCoroutine(Vanish());
    }

    public void ResetForHorrorEvent()
    {
        gameObject.SetActive(true);

        sr.enabled = true;
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        stalkingActive = false;
        eventControlled = false;

        if (vanishRoutine != null)
        {
            StopCoroutine(vanishRoutine);
            vanishRoutine = null;
        }
    }
}