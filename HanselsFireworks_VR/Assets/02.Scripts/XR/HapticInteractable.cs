using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VR
{
    public class HapticInteractable : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;
        public float duration;

        public XRBaseController controller;

        // Start is called before the first frame update
        void Start()
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
            interactable.activated.AddListener(TriggerHaptic);
            controller = GetComponentInParent<ActionBasedController>();
        }

        public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
        {
            if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
            {
                TriggerHaptic(controllerInteractor.xrController);
            }
        }

        private void TriggerHaptic(XRBaseController controller)
        {
            if (intensity > 0) controller.SendHapticImpulse(intensity, duration);
        }

        public void SendHaptics()
        {
            controller.SendHapticImpulse(intensity, duration);
        }

    }

}