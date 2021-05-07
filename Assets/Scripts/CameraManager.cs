using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera _mainCam;
    public Camera _rocketCam;
    public Rocket rocket;
    private bool firstPerson = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPerson = !firstPerson;
            rocket.FirstPerson = firstPerson;
            _rocketCam.gameObject.SetActive(firstPerson);
            _mainCam.gameObject.SetActive(!firstPerson);
        }
    }
}
