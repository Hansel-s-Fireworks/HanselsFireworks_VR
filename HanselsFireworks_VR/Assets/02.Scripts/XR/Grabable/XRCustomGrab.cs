using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable;

public class XRCustomGrab : XRGrabInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /*Transform m_OriginalSceneParent;
    protected override void Grab()
    {
        var thisTransform = transform;
        m_OriginalSceneParent = thisTransform.parent;
        thisTransform.SetParent(null);

        UpdateCurrentMovementType();
        SetupRigidbodyGrab(m_Rigidbody);

        // Reset detach velocities
        m_DetachVelocity = Vector3.zero;
        m_DetachAngularVelocity = Vector3.zero;

        // Initialize target pose
        m_TargetPose.position = m_AttachPointCompatibilityMode == AttachPointCompatibilityMode.Default ? thisTransform.position : m_Rigidbody.worldCenterOfMass;
        m_TargetPose.rotation = thisTransform.rotation;
        m_TargetLocalScale = thisTransform.localScale;
    }
    void UpdateCurrentMovementType()
    {
        // Special case where the interactor will override this objects movement type (used for Sockets and other absolute interactors).
        // Iterates in reverse order so the most recent interactor with an override will win since that seems like it would
        // be the strategy most users would want by default.
        MovementType? movementTypeOverride = null;
        for (var index = interactorsSelecting.Count - 1; index >= 0; --index)
        {
            var baseInteractor = interactorsSelecting[index] as XRBaseInteractor;
            if (baseInteractor != null && baseInteractor.selectedInteractableMovementTypeOverride.HasValue)
            {
                if (movementTypeOverride.HasValue)
                {
                    Debug.LogWarning($"Multiple interactors selecting \"{name}\" have different movement type override values set" +
                        $" ({nameof(XRBaseInteractor.selectedInteractableMovementTypeOverride)})." +
                        $" Conflict resolved using {movementTypeOverride.Value} from the most recent interactor to select this object with an override.", this);
                    break;
                }

                movementTypeOverride = baseInteractor.selectedInteractableMovementTypeOverride.Value;
            }
        }

        m_CurrentMovementType = movementTypeOverride ?? m_MovementType;
    }

    protected override void Drop()
    {
        if (m_RetainTransformParent && m_OriginalSceneParent != null && !m_OriginalSceneParent.gameObject.activeInHierarchy)
        {
#if UNITY_EDITOR
            // Suppress the warning when exiting Play mode to avoid confusing the user
            var exitingPlayMode = UnityEditor.EditorApplication.isPlaying && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
#else
                var exitingPlayMode = false;
#endif
            if (!exitingPlayMode)
                Debug.LogWarning("Retain Transform Parent is set to true, and has a non-null Original Scene Parent. " +
                    "However, the old parent is deactivated so we are choosing not to re-parent upon dropping.", this);
        }
        else if (m_RetainTransformParent && gameObject.activeInHierarchy)
            transform.SetParent(m_OriginalSceneParent);

        SetupRigidbodyDrop(m_Rigidbody);

        m_CurrentMovementType = m_MovementType;
        m_DetachInLateUpdate = true;
        EndThrowSmoothing();
    }*/
}
