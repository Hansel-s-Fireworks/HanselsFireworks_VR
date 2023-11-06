using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace tutorial {
    public class ActivateTeleportationRay : MonoBehaviour
    {
        public GameObject leftTeleportation;
        public GameObject rightTeleportation;

        public InputActionProperty leftActivate;
        public InputActionProperty rightActivate;

        public InputActionProperty leftCancel;
        public InputActionProperty rightCancel;


        void Update()
        {
            // leftActivate를 누르고 leftCancel을 누르지 않은 경우 점멸 광선 활성화
            // 즉, 물건을 집은 상태(leftCancel 누름)일 때 해당 손으로 점멸 광선은 나갈 수 없다. 
            leftTeleportation.SetActive(leftCancel.action.ReadValue<float>() == 0 && leftActivate.action.ReadValue<float>() > 0.1f);
            rightTeleportation.SetActive(rightCancel.action.ReadValue<float>() == 0 && rightActivate.action.ReadValue<float>() > 0.1f);
        }
    }

}
