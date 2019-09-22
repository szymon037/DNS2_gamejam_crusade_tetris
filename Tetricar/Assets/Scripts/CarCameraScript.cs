using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    private Transform t;
    public Transform car;
    private Vector3 difference;

    void Start()
    {
        t = transform;
        difference = t.position - car.position;

    }

    // Update is called once per frame
    void Update()
    {
        t.position = car.position + difference;
    }
}
