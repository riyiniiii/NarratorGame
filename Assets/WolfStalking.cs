using UnityEngine;
using System.Collections;

public class WolfStalking : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float stopDistance = 1.5f;
    public float fadeSpeed = 2f;
    public float spawnDistance = 2f;

    private bool stalkingActive = false;
    private SpriteRenderer sr;
    private Coroutine vanishRoutine;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!stalkingActive || player == null) return;

        if (IsPlayerLookingAtWolf())
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
            FollowPlayer();
        }
    }

    bool IsPlayerLookingAtWolf()
    {
        SpriteRenderer playerSR = player.GetComponent<SpriteRenderer>();
        if (playerSR == null) return false;

        bool facingRight = !playerSR.flipX;
        bool wolfIsRight = transform.position.x > player.position.x;

        return (facingRight && wolfIsRight) || (!facingRight && !wolfIsRight);
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= stopDistance) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }
    
    public void SpawnInFrontOfPlayer()
    {
        if (player == null) return;

        SpriteRenderer playerSR = player.GetComponent<SpriteRenderer>();
        if (playerSR == null) return;

        bool facingRight = !playerSR.flipX;

        Vector2 spawnOffset = facingRight ? Vector2.right : Vector2.left;
        Vector2 spawnPosition = (Vector2)player.position + spawnOffset * spawnDistance;

        transform.position = spawnPosition;

        sr.enabled = true;
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        Debug.Log("Wolf positioned for horror event (NOT started yet)");
    }
    
    public void StartHorrorMode()
    {
        stalkingActive = true;
        sr.enabled = true;

        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }
    
    private void Appear()
    {
        Color c = sr.color;
        c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * fadeSpeed);
        sr.color = c;
    }

    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.2f); 

        while (sr.color.a > 0.05f)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeSpeed);
            sr.color = c;
            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
    }

    public void ActivateStalking()
    {
        stalkingActive = true;
    }
}