using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class AnimateControllerInput : MonoBehaviour
{
    [Header("Press")]
    [Tooltip("Trigger(IndexFinger)")]
    public InputActionProperty pinchAnimationAction;
    [Tooltip("Grip(MiddleFinger)")]
    public InputActionProperty gripAnimationAction;
    
    [Header("Button")]
    [Tooltip("Left : X(Primary Button) Right : A(Primary Button)")]
    public InputActionProperty button01AnimationAction;
    [Tooltip("Left : Y(Secondary Button) Right : B(Secondary Button)")]
    public InputActionProperty button02AnimationAction;
    [Tooltip("Menu Button")]
    public InputActionProperty button03AnimationAction;
    
    [Header("Joystick")]
    public InputActionProperty joystickAnimationAction;
    
    public Animator controllerAnimator;

    void Update()
    {
        // 지정한 키의 입력값을 triggerValue에 대입
        // Animator의 파라미터 "Trigger"에 triggerValue값을 넣어준다. 
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        controllerAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimationAction.action.ReadValue<float>();
        controllerAnimator.SetFloat("Grip", gripValue);
        
        float button1Value = button01AnimationAction.action.ReadValue<float>();
        controllerAnimator.SetFloat("Button 1", button1Value);
        
        float button2Value = button02AnimationAction.action.ReadValue<float>();
        controllerAnimator.SetFloat("Button 2", button2Value);
        
        float button3Value = button03AnimationAction.action.ReadValue<float>();
        controllerAnimator.SetFloat("Button 3", button3Value);
        
        // 애니메이션(joystickAnimationAction)자료형이 vector2이므로 float를 넘길 수 없다. 
        Vector2 joyValue = joystickAnimationAction.action.ReadValue<Vector2>();
        controllerAnimator.SetFloat("Joy X", joyValue.x);
        controllerAnimator.SetFloat("Joy Y", joyValue.y);
    }
}