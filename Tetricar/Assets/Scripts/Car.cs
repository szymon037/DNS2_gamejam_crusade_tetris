﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Transform t;
    private Rigidbody rb;

    public float speed;
    public float rotationDuration;
    private bool isRotating = false;
    public AnimationCurve rotationCurve;
    private float angleOfCar;
    private Vector3 scale = Vector3.zero;

    public bool gameOver = false;
    public Camera camera;
    int i = 0;
    void Start()
    {
        t = transform;
        rb = GetComponent<Rigidbody>();
        speed = 5f;
        rotationDuration = 0.2f;
        scale = t.localScale;
        
    }

    void Update()
    {
        t.Translate(Vector3.forward * speed * Time.deltaTime);

        if (!gameOver && Input.GetKeyDown(KeyCode.A) && !isRotating && angleOfCar > -90f)
        {
            StartCoroutine(Rotate(Vector3.up, -90f));
        }
        if (!gameOver && Input.GetKeyDown(KeyCode.D) && !isRotating && angleOfCar < 90f)
        {
            StartCoroutine(Rotate(Vector3.up, 90f) );
        }

        if (!gameOver && !Physics.Raycast(t.position, Vector3.down, 5f))
        {
            gameOver = true;
            GameOver();
        }
    }

    private IEnumerator Rotate(Vector3 rotationVector, float rotationAngle)     //rotate Right Left
    {
        angleOfCar += rotationAngle;
        isRotating = true;
        float rotationTimer = 0f;
        float rotatedAngle = 0f;
        float oldAngle = 0f;
        float newAngle = 0f;
        float angle = 0f;

        while (rotationTimer <= rotationDuration)
        {
            float rotationPercentage = rotationTimer / rotationDuration;
            newAngle = rotationCurve.Evaluate(rotationPercentage) * rotationAngle;
            angle = newAngle - oldAngle;

            t.Rotate(rotationVector * angle, Space.World);

            rotatedAngle += angle;

            rotationTimer += Time.fixedDeltaTime;
            oldAngle = newAngle;
            yield return null;
        }
        t.Rotate(rotationVector * (rotationAngle - rotatedAngle), Space.World);
        //yield return new WaitForSeconds(0.1f);
        isRotating = false;
    }

    private void GameOver()
    {
        camera.transform.parent = null;
        rb.constraints = 0;
        rb.useGravity = true;
    }
}