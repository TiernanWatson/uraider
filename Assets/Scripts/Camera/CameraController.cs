﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 120.0f;
    public float yMax = 80.0f;
    public float yMin = -45.0f;
    public float smoothing = 15.0f;
    public bool LAUTurning = true;

    public Transform target;

    private float yRot = 0.0f;
    private float xRot = 0.0f;

    private Transform pivot;
    private Vector3 pivotOrigin;
    private Vector3 targetPivotPosition;
    private Camera cam;
    private CameraState camState;

    private void Start()
    {
        camState = CameraState.Grounded;
        cam = GetComponentInChildren<Camera>();
        pivot = cam.transform.parent;
        pivotOrigin = pivot.localPosition;
        targetPivotPosition = pivot.localPosition;
    }

    private void LateUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (camState == CameraState.Grounded)
            yRot += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        else
            yRot = target.rotation.eulerAngles.y;

        xRot -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime; // Negative so mouse up = cam down
        xRot = Mathf.Clamp(xRot, yMin, yMax);

        if (LAUTurning && camState == CameraState.Grounded 
            && Mathf.Abs(Input.GetAxis("Mouse X")) == 0f)
            DoExtraRotation();

        pivot.rotation = /*Quaternion.Slerp(pivot.rotation,*/ Quaternion.Euler(xRot, yRot, 0.0f)/*, smoothing * Time.deltaTime)*/;
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, targetPivotPosition, Time.deltaTime * smoothing);
    }

    private void HandleMovement()
    {
        transform.position = /*Vector3.Lerp(transform.position,*/ target.position/*, Time.deltaTime * smoothing)*/;
    }

    private void DoExtraRotation()
    {
        yRot += 1.0f * Input.GetAxis("Horizontal");
    }

    public void PivotOnHead()
    {
        targetPivotPosition = Vector3.zero + Vector3.up * 1.7f;
    }

    public void PivotOnTarget()
    {
        targetPivotPosition = Vector3.zero;
    }

    public void PivotOnPivot()
    {
        targetPivotPosition = pivotOrigin;
    }

    public CameraState State
    {
        get { return camState; }
        set { camState = value; }
    }
}

// enum incase of future extensions
public enum CameraState
{
    Grounded,
    Combat
}
