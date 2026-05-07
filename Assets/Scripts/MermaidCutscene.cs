using UnityEngine;

public class MermaidCutscene : MonoBehaviour
{
    public Transform player;
    public Transform stopPoint;
    public float speed = 2f;
    

    private bool isCutsceneActive = false;

    private CharacterController2D playerController;
    private Animator playerAnimator;
    private SpriteRenderer playerSprite;
    private Rigidbody2D playerRb;

    void Start()
    {
        playerController = player.GetComponent<CharacterController2D>();
        playerAnimator = player.GetComponent<Animator>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    public void StartCutscene()
    {
        isCutsceneActive = true;
        
        playerController.canMove = false;

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Update()
    {
        if (!isCutsceneActive) return;

        // Move player toward stop point
        player.position = Vector2.MoveTowards(
            player.position,
            stopPoint.position,
            speed * Time.deltaTime
        );

        // Animation
        playerAnimator.SetBool("isRunning", true);

        // Reached destination
        if (Vector2.Distance(player.position, stopPoint.position) < 0.05f)
        {
            isCutsceneActive = false;

            playerAnimator.SetBool("isRunning", false);

           
            if (playerRb != null)
            {
                playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }
}