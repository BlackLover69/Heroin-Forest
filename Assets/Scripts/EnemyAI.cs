using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Ionic.Zip;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 25f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 2.5f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = .1f;

    [Header("Custom Behavior")]
    public bool jumpEnabled = true;


    public BoxCollider2D coll;
    [SerializeField] private LayerMask groundLayer;
    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded = false;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance())
            PathFollow();
    }

    private void UpdatePath()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        RaycastHit2D isGrounded = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        //Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        //Jump
        if (jumpEnabled && isGrounded)
        {
            if (target.position.y - 1f > rb.transform.position.y && rb.velocity.y == 0 && path.path.Count < 20)
            {
                rb.AddForce(Vector2.up * speed * jumpModifier);
                
            }
        }

        //Movement
        rb.AddForce(force);

        if (rb.velocity.x > speed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else if (rb.velocity.x < speed * (-1))
        {
            rb.velocity = new Vector2(speed * (-1), rb.velocity.y);
        }
        //Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;

        //Direction Graphics Handling

        if (rb.velocity.x > .05f)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < -.05f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
            


    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }
        
    private void OnPathComplete(Path p) 
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    

}
