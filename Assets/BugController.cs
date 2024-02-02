using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BugController : MonoBehaviour
{
    private Transform target;
    public LineRenderer lineRenderer;
    public AIPath aiPath; 
    private AIDestinationSetter aiDestinationSetter;
    
    
    public float moveSpeed = 10f;

    private Rigidbody2D rb;

    private bool isAuto;

    private bool isMoving = false;
    private Vector2 moveDirection;
    public float raycastDistance = 0.5f;
    public LayerMask wallLayer;

    public bool AtOpen;
    private bool disableRaycast;

    [SerializeField]
    private bool isVerticalMove;

    private RaycastHit2D raycastHitLeft;
    private RaycastHit2D raycastHitRight;
    private RaycastHit2D raycastHitUp;
    private RaycastHit2D raycastHitDown;

    public Sprite bugHSprite;
    public Sprite bugVSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        aiPath.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        
        
        Button findButton = GameObject.Find("FindBtn").GetComponent<Button>();
        findButton.onClick.AddListener(FindPath);
        
        Button moveButton = GameObject.Find("MoveBtn").GetComponent<Button>();
        moveButton.onClick.AddListener(AutoMove);
        
        Button moveUpBtn = GameObject.Find("Up").GetComponent<Button>();
        moveUpBtn.onClick.AddListener(MoveUp);
        Button moveDownBtn = GameObject.Find("Down").GetComponent<Button>();
        moveDownBtn.onClick.AddListener(MoveDown);
        Button moveLeftBtn = GameObject.Find("Left").GetComponent<Button>();
        moveLeftBtn.onClick.AddListener(MoveLeft);
        Button moveRightBtn = GameObject.Find("Right").GetComponent<Button>();
        moveRightBtn.onClick.AddListener(MoveRight);

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Red Dot(Clone)").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    
    {
        if (!disableRaycast)
        {
            // Determine the raycast directions based on movement
            Vector2 raycastDirectionLeft, raycastDirectionRight, raycastDirectionUp, raycastDirectionDown;

                raycastDirectionLeft = Vector2.left;
                raycastDirectionRight = Vector2.right;
                raycastDirectionUp = Vector2.up;
                raycastDirectionDown = Vector2.down;
        

            // Cast rays in the specified directions
             raycastHitLeft = Physics2D.Raycast(transform.position, raycastDirectionLeft, raycastDistance, wallLayer);
             raycastHitRight = Physics2D.Raycast(transform.position, raycastDirectionRight, raycastDistance, wallLayer);
             raycastHitUp = Physics2D.Raycast(transform.position, raycastDirectionUp, raycastDistance, wallLayer);
             raycastHitDown = Physics2D.Raycast(transform.position, raycastDirectionDown, raycastDistance, wallLayer);

            // Visualize the rays in the Scene view
            Debug.DrawRay(transform.position, raycastDirectionLeft * raycastDistance, raycastHitLeft ? Color.red : Color.green);
            Debug.DrawRay(transform.position, raycastDirectionRight * raycastDistance, raycastHitRight ? Color.red : Color.green);
            Debug.DrawRay(transform.position, raycastDirectionUp * raycastDistance, raycastHitUp ? Color.red : Color.green);
            Debug.DrawRay(transform.position, raycastDirectionDown * raycastDistance, raycastHitDown ? Color.red : Color.green);

            if (isVerticalMove)
            {
                // Check if all rays hit something
                if (raycastHitLeft.collider != null && raycastHitRight.collider != null)
                {
                    AtOpen = false;
                }
                else
                {
                    AtOpen = true;
                }
            }
            else 
                if (raycastHitUp.collider != null && raycastHitDown.collider != null)
                {
                    AtOpen = false;
                }
                else
                {
                    AtOpen = true;
                }
        }
    }

    private void MoveUp()
    {
        if (!isMoving && raycastHitUp.collider == null)
        {
            spriteRenderer.sprite = bugVSprite;
            spriteRenderer.flipY = false;
            isVerticalMove = true;
            Move(Vector2.up);
        }
    }
    private void MoveDown()
    {
        if (!isMoving && raycastHitDown.collider == null)
        {
            spriteRenderer.sprite = bugVSprite;
            spriteRenderer.flipY = true;
            isVerticalMove = true;
            Move(Vector2.down);
        }
    }
    private void MoveLeft()
    {
        if (!isMoving && raycastHitLeft.collider == null)
        {
            spriteRenderer.sprite = bugHSprite;
            spriteRenderer.flipX = true;
            isVerticalMove = false;
            Move(Vector2.left);
        }
    }
    private void MoveRight()
    {
        if (!isMoving && raycastHitRight.collider == null)
        {
            spriteRenderer.sprite = bugHSprite;
            spriteRenderer.flipX = false;
            isVerticalMove = false;
            Move(Vector2.right);
        }
    }

    void Move(Vector2 direction)
    {
        isMoving = true;
        moveDirection = direction;
        AtOpen = false;
        StartCoroutine(StopRayCast());
        StartCoroutine(AutoMoveCoroutine());

    }
    IEnumerator AutoMoveCoroutine()
    {
        float startTime = Time.time;
        Vector2 currentMoveDirection = moveDirection;

        while (!AtOpen)
        {
            // Move the player continuously in the specified direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // Check if the raycast in the current direction hits something
            RaycastHit2D currentRaycastHit = Physics2D.Raycast(transform.position, currentMoveDirection, 1f, wallLayer);
            if (currentRaycastHit.collider != null)
            {
                Debug.Log("stop ne");
                AtOpen = false;
                break;
            }
            
            // // Check if the maximum duration has passed
            // if (Time.time - startTime > 2)
            // {
            //     break; // Exit the loop after the specified duration
            // }
            yield return null;
        }
        transform.Translate(moveDirection * moveSpeed * 0.05f);

        isMoving = false;
    }
    IEnumerator StopRayCast()
    {
        disableRaycast = true;
        yield return new WaitForSeconds(0.18f);
        disableRaycast = false;
    }

 
  

    public void FindPath()
    {
        StartCoroutine(VisualizePathCoroutine());
    }

    public void AutoMove()
    {
        spriteRenderer.sprite = bugVSprite;
        spriteRenderer.flipX = false;
        spriteRenderer.flipY = false;

        Destroy(rb);
        aiPath.enabled = true;
        aiPath.maxSpeed = moveSpeed;
        aiDestinationSetter.target = target;
        isAuto = true;

    }
    
    IEnumerator VisualizePathCoroutine()
    {
        Destroy(rb);
        aiPath.maxSpeed = isAuto ? moveSpeed : 0;
        aiPath.enabled = true;
        aiDestinationSetter.target = target;
        
        yield return new WaitForSeconds(.25f);
        VisualizePath();

        if (!isAuto)
        {
            AddRigbody();
            aiPath.enabled = false;
        }
    }

    void VisualizePath()
    {
        lineRenderer.positionCount = aiPath.path.vectorPath.Count;
        for (int i = 0; i < aiPath.path.vectorPath.Count; i++) {
            lineRenderer.SetPosition (i, aiPath.path.vectorPath [i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Red Dot(Clone)")
        {
            FindObjectOfType<LevelManager>().GameEnd();
        }
    }

    private void AddRigbody()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

    }
}