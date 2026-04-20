using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSE_NPCWander : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float leftPatrolX, rightPatrolX;
    [SerializeField] private Animator anim;
    
    [SerializeField] private float minPauseTime, maxPauseTime;
    [SerializeField] private float minWalkTime, maxWalkTime;
    
    [SerializeField] private int facingDirection = -1;

    private bool isFlipping;
    private float randomTime, timer;
    private bool isWalking = true;

    private void Start()
    {
        randomTime = Random.Range(minWalkTime, maxWalkTime);
        anim.SetBool("isWalking", isWalking ? true : false);
    }
 
    void Update()
    {
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
    
    
}
