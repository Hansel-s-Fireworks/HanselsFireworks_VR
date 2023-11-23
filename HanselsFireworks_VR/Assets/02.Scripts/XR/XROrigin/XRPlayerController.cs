using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class XRPlayerController : MonoBehaviour
{
    [SerializeField] InputActionProperty m_LeftHandMoveAction;
    [SerializeField] InputActionProperty m_RightHandMoveAction;
    [SerializeField] private Vector2 leftHandValue;
    [SerializeField] private Vector2 rightHandValue;

    public float moveSpeed;
    public float turnSpeed;
    public Transform forwardSource;
    public List<List<double>> rotationMatrix;

    [Header("Debugging")] 
    public float cameraAngleInRadians;
    public Vector2 des;
    public double cosTheta;
    public double sinTheta;
    
    public InputActionProperty leftHandMoveAction
    {
        get => m_LeftHandMoveAction;
        set => SetInputActionProperty(ref m_LeftHandMoveAction, value);
    }
    
    public InputActionProperty rightHandMoveAction
    {
        get => m_RightHandMoveAction;
        set => SetInputActionProperty(ref m_RightHandMoveAction, value);
    }

    private void Start()
    {
        des = new Vector2(0, 0);
    }

    private void Update()
    {
        Move();
        Turn();
    }

    public void Move()
    {
        cameraAngleInRadians = forwardSource.rotation.eulerAngles.y * Mathf.Deg2Rad;
        // Vector2 temp = new Vector2(1, 1);
        
        cosTheta = Math.Cos(cameraAngleInRadians);
        sinTheta = Math.Sin(cameraAngleInRadians);

        leftHandValue = m_LeftHandMoveAction.action.ReadValue<Vector2>();
        /*des.x = (float)(-leftHandValue.y * cosTheta + leftHandValue.x * sinTheta);
        des.y = (float)(leftHandValue.x * cosTheta + leftHandValue.y * sinTheta);*/
        des.x = (float)(-leftHandValue.x * cosTheta + leftHandValue.y * sinTheta);
        des.y = (float)(leftHandValue.y * cosTheta + leftHandValue.x * sinTheta);
        // des.x = (float)( - rotationMatrix[0][0] * leftHandValue.y + rotationMatrix[0][1] * leftHandValue.y);
        // des.y = (float)(rotationMatrix[1][0] * leftHandValue.x + rotationMatrix[1][1] * leftHandValue.y);
        transform.position += new Vector3(- des.x * moveSpeed * Time.deltaTime, 0, des.y * moveSpeed * Time.deltaTime);
    }
    
    public void Turn()
    {
        rightHandValue = m_RightHandMoveAction.action.ReadValue<Vector2>();
        Quaternion joystickRotation = Quaternion.Euler(0, rightHandValue.x * turnSpeed * Time.deltaTime, 0);
        transform.rotation *= joystickRotation;
    }
    
    void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying) property.DisableDirectAction();
    
        property = value;
    
        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }
    
    
    
}
