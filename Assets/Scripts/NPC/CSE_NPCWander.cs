using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSE_NPCWander : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float leftPatrolX, rightPatrolX;
    [SerializeField] private Animator anim;
    [SerializeField] private float runSpeed = 8f;

    
    [SerializeField] private float minPauseTime, maxPauseTime;
    [SerializeField] private float minWalkTime, maxWalkTime;
    
    [SerializeField] private int facingDirection = -1;

    private bool isFlipping;
    private float randomTime, timer;
    private bool isWalking = true;
    private bool isRunningAway = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        anim.SetBool("isWalking", isWalking ? true : false);
    }
    
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying && !isRunningAway)
        {
            FacePlayer();
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        if (isRunningAway)
        {
            RunAwayMovement();
            return;
        }

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        timer += Time.deltaTime;

        if (timer >= randomTime)
            StateChange();

        if (!isFlipping && (transform.position.x > rightPatrolX || transform.position.x < leftPatrolX))
            StartCoroutine(Flip());

        if (isWalking)
            rb.linearVelocity = Vector2.right * facingDirection * speed;
    }
    
    IEnumerator Flip()
    {
        isFlipping = true;
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
        yield return new WaitForSeconds(0.5f);
        isFlipping = false;
    }
    

    void StateChange()
    {
        isWalking = !isWalking;
        anim.SetBool("isWalking", isWalking ? true : false);
        randomTime = isWalking ? Random.Range(minWalkTime , maxWalkTime) : Random.Range(minPauseTime, maxPauseTime);
        timer = 0;
    }
    
    public void RunAway()
    {
        isRunningAway = true;
    }
    
    void RunAwayMovement() // run away after dialogue
    {
        Vector2 direction = (transform.position - player.position).normalized;
        rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y);

        anim.SetBool("isWalking", true);

        if ((direction.x > 0 && facingDirection < 0) || (direction.x < 0 && facingDirection > 0))
        {
            transform.Rotate(0, 180, 0);
            facingDirection *= -1;
            
        }
        
    }
    
    private void FacePlayer()
    {
        if (player == null || isFlipping) return;

        if (player.position.x > transform.position.x && facingDirection < 0)
        {
            StartCoroutine(Flip());
        }
        else if (player.position.x < transform.position.x && facingDirection > 0)
        {
            StartCoroutine(Flip());
        }
    }
}
