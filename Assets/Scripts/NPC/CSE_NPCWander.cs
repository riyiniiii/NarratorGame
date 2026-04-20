using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSE_NPCWander : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float leftPatrolX, rightPatrolX;
    
    [SerializeField] private int facingDirection = -1;
 
    void Update()
    {
        if (transform.position.x > rightPatrolX || transform.position.x < leftPatrolX)
            Flip();

        rb.linearVelocity = Vector2.right * facingDirection * speed;

    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingDirection *= -1;
    }
}
