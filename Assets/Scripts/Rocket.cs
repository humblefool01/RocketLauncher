using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    public float _mass;
    [HideInInspector]
    public Rigidbody rb;
    [HideInInspector]
    public bool _boundaryExceeded = false;
    [HideInInspector]
    public bool _launch = false;
    private float thrust = 0f;
    private Vector3 launchDirection;
    private Vector3 mouseWorldPositionRight;
    private Vector3 mouseWorldPositionLeft;

    public UnityAction OnThrustChanged { get; set; }
    public UnityAction OnDirectionChanged { get; set; }

    public float Thrust
    {
        get
        {
            return this.thrust;
        }
        set
        {
            if (this.thrust != value)
            {
                this.OnThrustChanged?.Invoke();
                this.thrust = value;
            }
        }
    }

    public Vector3 LaunchDirection
    {
        get
        {
            return this.launchDirection;
        }
        set
        {
            if (this.launchDirection != value)
            {
                this.OnDirectionChanged?.Invoke();
                this.launchDirection = value;
            }
        }
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mouseWorldPositionRight = this.transform.position;
        mouseWorldPositionLeft = this.transform.position;
        this.LaunchDirection = this.transform.up;
    }

    
    private void FixedUpdate()
    {
        // Direction
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            mouseWorldPositionRight  = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            this.LaunchDirection = mouseWorldPositionRight - this.transform.position;
            this.transform.up = this.LaunchDirection;  // Make the rocket face towards right mouse click point
        }
        // Thrust
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            mouseWorldPositionLeft = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            this.Thrust = Vector3.Distance(mouseWorldPositionLeft, this.transform.position);
            Debug.DrawRay(mouseWorldPositionLeft, this.transform.position - mouseWorldPositionLeft, Color.blue);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!this._launch)
            {
                this._launch = true;
                rb.velocity = this.LaunchDirection * this.Thrust;
            }
        }
    }


    // Restart game when collided with Planet/Moon
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Boundary")
        {
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Restart game when gone out of boundary
    private void OnTriggerExit(Collider other)
    {
        this._boundaryExceeded = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
