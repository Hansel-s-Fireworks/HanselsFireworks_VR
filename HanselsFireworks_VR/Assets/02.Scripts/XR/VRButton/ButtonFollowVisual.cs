using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    public Vector3 localAxis;
    public float resetSpeed = 5;
    public float followAngleThreshold = 45;
    
    private bool freeze = false;
    
    private Vector3 initialLocalPos;

    private Vector3 offset;
    private Transform pokeAttachTransform;
    
    private XRBaseInteractable interactable;
    private bool isFollowing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;
        
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset); 
        interactable.selectEntered.AddListener(Freeze);
    }

    // 손가락이 버튼 따라 내려감
    public void Follow(BaseInteractionEventArgs hover)
    {
        Debug.Log("Follow");
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;

            float pokeAngle = Vector3.Angle(offset, visualTarget.TransformDirection(localAxis));

            if (pokeAngle < followAngleThreshold)
            {
                isFollowing = true;
                freeze = false;
            }
        }
    }

    // 원래자리로 돌아옴
    public void Reset(BaseInteractionEventArgs hover)
    {
        Debug.Log("Reset");
        if (hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }
    
    // 완전히 누를 때 트리거
    public void Freeze(BaseInteractionEventArgs hover)
    {
        Debug.Log("Freeze");
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (freeze) return;
        
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);

            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition =
                Vector3.Lerp(visualTarget.localPosition, initialLocalPos, Time.deltaTime * resetSpeed); // 버튼 위치 초기화
        }

        if (visualTarget.localPosition.y < -0.0544f)
        {
            Debug.Log("y");
            visualTarget.localPosition = new Vector3(visualTarget.localPosition.x, -0.05f, visualTarget.localPosition.z);
        }


    }
}