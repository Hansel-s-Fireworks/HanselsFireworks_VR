using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;

namespace VR
{
    public enum Mode
    {
        NULL,
        normal,
        burst
    }

    public class FireGun : MonoBehaviour
    {
        [Header("Weapon Setting")]
        public GameObject bullet;
        public float attackRate = 0.1f;            // 공격 속도
        // public bool ;      // 연속 공격 여부
        [SerializeField] private Transform bulletSpawnPoint;             // 총알 생성 위치
        [SerializeField] Mode mode;

        public GameObject laser;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip audioClipFire;                // 공격 사운드
         // [SerializeField] private AudioClip burstBGM;

        GunAnimatorController animator;
        AudioSource audioSource;                // 사운드 재생 컴포넌트
        private MemoryPool bulletMemoryPool;
        float lastAttackTime = 0;
        bool isGrapped;
        bool isPressed;

        [Header("Key")]
        public InputActionProperty btnTrigger;

        public HapticInteractable haptic;

        private void Awake()
        {
            bulletMemoryPool = new MemoryPool(bullet);
        }

        private void Start()
        {
            XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
            grabbable.selectEntered.AddListener(GrapGun);
            grabbable.selectExited.AddListener(ReleaseGun);
            haptic = GetComponent<HapticInteractable>();
            mode = Mode.normal;
            laser.SetActive(false);
            isPressed = false;
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<GunAnimatorController>();
        }
        private void OnApplicationQuit()
        {
            bulletMemoryPool.DestroyObjects();
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.Stop();             // 기존에 재생중인 사운드를 정지하고 
            audioSource.clip = clip;        // 새로운 사운드 clip으로 교체 후
            audioSource.Play();             // 사운드 재생
        }


        public void GrapGun(SelectEnterEventArgs arg)
        {
            isGrapped = true;
            Debug.Log("잡음");            
        }

        public void ReleaseGun(SelectExitEventArgs arg)
        {
            isGrapped = false;
            StopCoroutine(OnAttackLoop());
            Debug.Log("놓음");
        }

        private void Update()
        {
            if (isGrapped)
            {
                UpdateMode();
            }

        }

        public void UpdateMode()
        {
            switch (mode)
            {
                case Mode.NULL:
                    break;
                case Mode.normal:
                    if (btnTrigger.action.WasPressedThisFrame())
                    {
                        OnAttack();
                    }
                    break;
                case Mode.burst:                    
                    if (btnTrigger.action.WasPressedThisFrame())
                    {
                        isPressed = true;                        
                        StartCoroutine(OnAttackLoop());
                        Debug.Log("트리거 당김");
                    }
                    else if (btnTrigger.action.WasReleasedThisFrame())
                    {
                        Debug.Log("트리거 놓음");
                        isPressed = false;
                    }
                    break;
                default:
                    break;
            }
        }


        public void BurstMode()
        {
            mode = Mode.burst;            
            GameManager.Instance.leftCase += 100;
        }

        public void AttachLaser()
        {
            laser.SetActive(true);
        }

        private IEnumerator OnAttackLoop()
        {
            while (isPressed)
            {
                OnAttack();
                if (GameManager.Instance.leftCase <= 0)
                {
                    mode = Mode.normal;
                    break;
                }
                yield return null;
            }
        }

        public void OnAttack()
        {
            if (Time.time - lastAttackTime > attackRate)
            {
                // if (animator.MoveSpeed > 0.5f) return;
                // 공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;
                // 무기 애니메이션 재생
                // 같은 애니메이션을 반복할 때, 애니메이션을 끊고 처음부터 다시 재생
                // animator.Play("Fire", -1, 0);
                haptic.SendHaptics();
                // 총구 이펙트 재생
                // StartCoroutine("OnMuzzleFlashEffect");
                // Debug.Log("Shoot Gun Anim");
                GameObject clone = bulletMemoryPool.ActivatePoolItem();

                clone.transform.position = bulletSpawnPoint.position;
                clone.transform.rotation = bulletSpawnPoint.rotation;
                clone.GetComponent<PlayerBullet>().Setup(bulletMemoryPool);
                if (mode == Mode.burst) GameManager.Instance.leftCase -= 1; // 연사일때만 총알개수 줄이기

                // 공격 사운드 재생
                PlaySound(audioClipFire);
            }
        }
    }

}

