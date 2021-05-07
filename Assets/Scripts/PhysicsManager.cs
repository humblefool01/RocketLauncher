using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public GameObject _boundary;
    public GameObject _trajectoryPoint;
    public GameObject trajectoryPointParent;
    public int numberOfPoints = 10;
    public Rocket _rocket;
    public Planet[] _planets;
    private Vector3 totalForce = Vector3.zero;
    private Vector3 initialForce = Vector3.zero;
    private static readonly float G = 6.67f * Mathf.Pow(10, -1);    // 11
    private GameObject[] trajectoryPoints;
    private void Start()
    {
        this.totalForce = this.CalculateForce(_rocket.transform.position);
        _rocket.OnThrustChanged += () => this.CalculateTrajectory();
        _rocket.OnDirectionChanged += () => this.CalculateTrajectory();
        this.initialForce = this.totalForce;
        this.totalForce = Vector3.zero;

        trajectoryPoints = new GameObject[numberOfPoints];
    }

    private void FixedUpdate()
    {
        //Debug.Log(this.initialForce * _rocket.thrust);

        if (_rocket._boundaryExceeded || !_rocket._launch)
        {
            _rocket.rb.velocity = Vector3.zero;
            return;
        }

        this.totalForce += this.CalculateForce(_rocket.transform.position);
        _rocket.rb.AddForce(this.totalForce, ForceMode.Force);
    }

    // Should be called when value of thrust / direction changes
    public void CalculateTrajectory()
    {
        if (_rocket.Thrust == 0)
        {
            return;
        }
        // s = ut + 1/2 * at^2
        // v^2 = u^2 - 2 * as
        // v = u + at

        // We have initial velocity u = LaunchDirection * Thrust
        // We have acceleraion a = initialForce / mass
        // acceleration is changing due to changing gravitational force
        // To get a at point s, we need force at point s
        // We assume for very small value of t, we assume the acceleration to remain constant 

        var u = _rocket.LaunchDirection * _rocket.Thrust;
        var v = Vector3.zero;
        var t = 0.05f;
        var s = _rocket.transform.position;
        var pos = s;
        var a = this.CalculateForce(s) / _rocket._mass;

        for (int i = 0; i < numberOfPoints; i++)
        {
            Destroy(trajectoryPoints[i]);
            trajectoryPoints[i] = Instantiate(_trajectoryPoint);
            //float scale = Mathf.Max(1f / (i + 1), 0.2f);
            //trajectoryPoints[i].transform.localScale = new Vector3(scale, scale, scale);
            trajectoryPoints[i].transform.position = new Vector3(0, 0, -1);
            s += u * t + 0.5f * a * t * t;
            v = u + a * t;
            u = v;
            a = this.CalculateForce(s) / _rocket._mass;
            trajectoryPoints[i].transform.position = new Vector3(s.x, s.y, -1f);
            trajectoryPoints[i].transform.SetParent(trajectoryPointParent.transform);
        }

        if (_rocket.Thrust == 0)
        {
            return;
        }
    }

    // Calculates force acting on the rocket when it is at point pos
    private Vector3 CalculateForce(Vector3 pos)
    {
        Vector3 F = Vector3.zero;
        foreach (Planet planet in _planets)
        {
            if (_rocket.rb.velocity != Vector3.zero)
            {
                _rocket.transform.up = _rocket.rb.velocity.normalized;
            }
            Vector3 force = planet.transform.position - pos;
            force = force.normalized;
            float R = Vector3.Distance(planet.transform.position, pos);
            float forceMagnitude = G * _rocket._mass * planet._mass / (R * R);
            force = force * forceMagnitude;
            F += force;
        }
        return F;
    }
}
