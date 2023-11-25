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

        public XRBaseController r_controller;
        public XRBaseController l_controller;

        // Start is called before the first frame update
        void Start()
        {
            XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
            interactable.activated.AddListener(TriggerHaptic);
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
            r_controller.SendHapticImpulse(intensity, duration);
            // l_controller.SendHapticImpulse(intensity, duration);
        }

    }

}