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

    private Joystick joystick;
    private bool isMoving = false;
    private Vector2 moveDirection;
    public float raycastDistance = 1.0f;
    public LayerMask wallLayer;

    public bool notAtOpen;
    private bool disableRaycast;

    private bool isVerticalMove;
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

        joystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Red Dot(Clone)").transform;

        
    }

    void Update()
    
    {
        if (!disableRaycast)
        {
            
            // Determine the raycast directions based on movement
            Vector2 raycastDirection1, raycastDirection2;
            if (isVerticalMove)
            {
                raycastDirection1 = Vector2.left;
                raycastDirection2 = Vector2.right;
            }
            else
            {
                raycastDirection1 = Vector2.up;
                raycastDirection2 = Vector2.down;
            }

            // Cast rays in the specified directions
            RaycastHit2D  raycastHit1 = Physics2D.Raycast(transform.position, raycastDirection1, raycastDistance, wallLayer);
            RaycastHit2D  raycastHit2 = Physics2D.Raycast(transform.position, raycastDirection2, raycastDistance, wallLayer);

            // Visualize the rays in the Scene view
            Debug.DrawRay(transform.position, raycastDirection1 * raycastDistance, raycastHit1 ? Color.red : Color.green);
            Debug.DrawRay(transform.position, raycastDirection2 * raycastDistance, raycastHit2 ? Color.red : Color.green);

            // Check if both rays hit something
            if (raycastHit1.collider != null && raycastHit2.collider != null)
            {
                notAtOpen = true;
            }
            else
            {
                notAtOpen = false;
            }

       

           

            if (!isMoving)
            {
                // Check for input to initiate automatic movement
                if (Input.GetKeyDown(KeyCode.W))
                {
                    isVerticalMove = true;
                    notAtOpen = true;
                    Move(Vector2.up);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    isVerticalMove = false;
                    notAtOpen = true;
                    Move(Vector2.left);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    isVerticalMove = true;
                    notAtOpen = true;
                    Move(Vector2.down);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    isVerticalMove = false;
                    notAtOpen = true;
                    Move(Vector2.right);
                }

            }
        }
    }

    void Move(Vector2 direction)
    {
        isMoving = true;
        moveDirection = direction;
        notAtOpen = true;

   
        StartCoroutine(StopRayCast());
        StartCoroutine(AutoMoveCoroutine());

    }
    IEnumerator StopRayCast()
    {
        disableRaycast = true;
        yield return new WaitForSeconds(0.18f);
        disableRaycast = false;
        notAtOpen = false;
    }

 
    IEnumerator AutoMoveCoroutine()
    {
        float startTime = Time.time;

        while (notAtOpen)
        {
            // Move the player continuously in the specified direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // Check if the maximum duration has passed
            if (Time.time - startTime > 2)
            {
                break; // Exit the loop after the specified duration
            }
            yield return null;
        }
        transform.Translate(moveDirection * moveSpeed * 0.025f);

        isMoving = false;
    }

    public void FindPath()
    {
        StartCoroutine(VisualizePathCoroutine());
    }

    public void AutoMove()
    {
        Destroy(rb);
        aiPath.enabled = true;
        aiPath.maxSpeed = 5;
        aiDestinationSetter.target = target;
        isAuto = true;

    }
    
    IEnumerator VisualizePathCoroutine()
    {
        Destroy(rb);
        aiPath.maxSpeed = isAuto ? 5 : 0;
        aiPath.enabled = true;
        aiDestinationSetter.target = target;
        
        yield return new WaitForSeconds(.1f);
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