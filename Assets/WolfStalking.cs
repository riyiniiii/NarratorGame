using UnityEngine;
using System.Collections;

public class WolfStalking : MonoBehaviour
{
    public Transform player;

    public float normalSpeed = 5f;
    public float slowedSpeed = 0.5f;
    public float speedChangeRate = 3f;

    public float stopDistance = 1.5f;
    public float fadeSpeed = 2f;

    private float currentSpeed;
    private bool stalkingActive;

    private SpriteRenderer sr;
    private Coroutine vanishRoutine;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        if (!stalkingActive || player == null) return;

        bool isLooking = IsPlayerLookingAtWolf();

        float targetSpeed = isLooking ? slowedSpeed : normalSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedChangeRate);

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

    private bool IsPlayerLookingAtWolf()
    {
        SpriteRenderer psr = player.GetComponent<SpriteRenderer>();
        if (psr == null) return false;

        bool facingRight = !psr.flipX;
        bool wolfRight = transform.position.x > player.position.x;

        return (facingRight && wolfRight) || (!facingRight && !wolfRight);
    }

    private void Appear()
    {
        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * fadeSpeed);
        sr.color = c;
    }

    private IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.2f);

        while (sr.color.a > 0.05f)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeSpeed);
            sr.color = c;
            yield return null;
        }

        sr.enabled = false;
    }

    // RESET FOR HORROR EVENT
    public void ResetForHorrorEvent()
    {
        gameObject.SetActive(true);

        sr.enabled = true;

        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        stalkingActive = false;

        if (vanishRoutine != null)
        {
            StopCoroutine(vanishRoutine);
            vanishRoutine = null;
        }
    }

    public void ActivateStalking()
    {
        stalkingActive = true;
    }
}