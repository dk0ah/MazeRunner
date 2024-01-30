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
    
    
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private bool isAuto;

    private Joystick joystick;
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

    void FixedUpdate()
    {
        if (!aiPath.enabled)
        {
            float horizontalInput = joystick.Horizontal;
            float verticalInput = joystick.Vertical;

            Vector2 movement = new Vector2(horizontalInput, verticalInput);
            movement.Normalize(); // Ensure diagonal movement isn't faster

            rb.velocity = movement * moveSpeed;

            if (movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(-movement.x, movement.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
       
        
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