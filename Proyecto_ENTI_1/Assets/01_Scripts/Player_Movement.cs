using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement Logic")]
    [Space]
    [SerializeField, Range(6, 40)] int maxPlayerSpeed = 3;
    [SerializeField, Range(6, 40)] int acceleration = 3;
    [SerializeField, Range(6, 20)] int deceleration = 5;
    [SerializeField, Range(2, 8)] int gravityDecelerator = 4;
    [SerializeField, Range(4,30)] int directionChangeSpeed = 10;
    [SerializeField, Range(1,20)] int jumpForce = 5;
    [SerializeField, Range(0.1f, 20)] float rayDistance = 0.6f;
    float gravity;

    [Header("Collisions Checker")]
    [Space]
    [SerializeField] DrawChecker rightChecker;
    [SerializeField] DrawChecker leftChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [Header("Animations Logic")]
    [Space]
    Animator anim;
    float currentSpeed = 0;
    bool collided_RightWall;
    bool collided_LeftWall;
    bool jumpedFromFloor = false;
    [Header("Attack Logic")]
    [Space]
    [SerializeField] float chargingTime;
    [SerializeField] int dynamicObject_CheckDistance;
    [SerializeField] LayerMask dynamicObject;

    Transform playerTransform;
    Rigidbody2D playerRigidbody;
    TimerObject wallJumpTimer;
    TimerObject attackTimer;
    TimerObject chargeTimer;
    AnimationObject animationObject;

    void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        wallJumpTimer = new TimerObject(this);
        attackTimer = new TimerObject(this);
        chargeTimer = new TimerObject(this);
        animationObject = new AnimationObject(this);

        gravity = playerRigidbody.gravityScale;
    }
    void Update()
    {
        PlayerJump();
        PlayerMove();
    }
    bool IsGrounded ( )
    {
        
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayDistance, Color.red);
        if (raycastHit2D.collider == null)
        {
            return false;
        }
        return true;
    }
    void PlayerMove ( )
    {
        bool movingRight = Input.GetKey(KeyCode.D);
        bool movingLeft = Input.GetKey(KeyCode.A);
        if (Input.GetKey(KeyCode.D) && !collided_RightWall) //Si right move y no me he golpeado con una pared en la derecha
        {
            movingRight = true;
            movingLeft = false;
            currentSpeed += IsGrounded() ? acceleration * Time.deltaTime : acceleration * 3 * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A) && !collided_LeftWall)  //Si left move y no me he golpeado con una pared en la izquierda
        {
            
            movingLeft = true;
            movingRight = false;
            currentSpeed -= IsGrounded() ? acceleration * Time.deltaTime : acceleration * 3 * Time.deltaTime;
        }

        if ((movingRight && currentSpeed < 0 || movingLeft && currentSpeed > 0)) //Si me estoy moviendo y la velocidad es negativa o positiva
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, directionChangeSpeed * Time.deltaTime);
        }
        else if (!IsGrounded()) //Si no estoy en el suelo
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, (deceleration / 3) * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxPlayerSpeed, maxPlayerSpeed);
        anim.SetFloat(animationObject.MOVE_ANIMATOR_TAG, currentSpeed);
        playerRigidbody.linearVelocity = new Vector2 (currentSpeed, playerRigidbody.linearVelocityY);

    }
    void PlayerJump ( )
    {
        collided_RightWall = rightChecker.CheckColisions(wallLayer) != null;
        collided_LeftWall = leftChecker.CheckColisions(wallLayer) != null;
        bool collidedAnyWall = collided_RightWall || collided_LeftWall;

        if (collidedAnyWall)
        {
            currentSpeed = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!wallJumpTimer.IsTimerActive())
            {
                if (IsGrounded())
                {
                    wallJumpTimer.StartTimer(.75f);
                    if (collided_RightWall || collided_LeftWall)
                    {
                        playerRigidbody.AddForceY(jumpForce / gravityDecelerator, ForceMode2D.Impulse);
                    }
                    else
                    {
                        playerRigidbody.AddForceY(jumpForce, ForceMode2D.Impulse);
                    }
                    jumpedFromFloor = true;
                }
                else
                {
                    if (collided_RightWall || collided_LeftWall)
                    {
                        wallJumpTimer.StartTimer(.75f);
                        jumpedFromFloor = false;
                        float impulse = collided_LeftWall ? jumpForce : -jumpForce;
                        playerRigidbody.AddForceY(jumpForce / 2, ForceMode2D.Impulse);
                        playerRigidbody.AddForceX(impulse, ForceMode2D.Impulse);
                    }
                }
            }
        }
        if ((collided_RightWall || collided_LeftWall) && !IsGrounded())
        {
            playerRigidbody.gravityScale = gravity / gravityDecelerator;
        }
        else
        {
            playerRigidbody.gravityScale = gravity;
        }
    }
    void PlayerAttack()
    {
        float counter;
        if (!attackTimer.IsTimerActive()) //Ataque normal
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackTimer.StartTimer(.5f);
                anim.SetTrigger(animationObject.ATTACK_ANIMATOR_TAG);
            }
        }
        if (Input.GetMouseButton(0)) //Ataque cargado
        {
            counter += Time.deltaTime;
            if (counter > chargingTime)
            {
                counter = 0;
                anim.SetTrigger(animationObject.CHARGED_ATTACK_ANIMATOR_TAG);
            }
        }
    }
    void ChargedAttack()
    {
        RayCastHit2D rayCastHit2D(transform.position,transform.right, dynamicObject_CheckDistance, dynamicObject)

        if(rayCastHit2D == null) return;

        Transform objectTransform = rayCastHit2D.transform;

        objectTransform.parent = playerTransform;
        objectTransform.position = playerTransform.position + playerTransform.forward * 3;

    }
}
