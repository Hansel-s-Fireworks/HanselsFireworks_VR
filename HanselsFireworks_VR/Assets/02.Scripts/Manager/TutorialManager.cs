using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using tutorial;
using VR;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public float timer;
    public float lerpValue;
    public ParticleSystem leftHandTransition;
    public ParticleSystem rightHandTransition;
    public List<ParticleSystem> l_clickEffects;
    public List<ParticleSystem> r_clickEffects;


    [Header("UI Image")] 
    public Sprite[] m_wellcomeImgData;
    public Sprite[] m_tutorialImgData;
    public Sprite[] m_entrancetImgData;

    [Header("Audio")]
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    public AudioSource l_controllerBtnSound;
    public AudioSource r_controllerBtnSound;
    public AudioSource mainAudioSource;


    [Header("Debug")]
    public int wellcomeUIIndex;
    public int tutorialUIIndex;
    public int entranceUIIndex;
    public int curIndex;    
    public Image showUI;
    public float delay;
    public float temp = 0;
    public bool isBtn1Pressed;
    public bool isBtn2Pressed;
    public bool isPressed;
    public bool isGrab;
    public bool isPortal;

    [Header("XROrigin")] 
    public GameObject leftHand;
    public GameObject leftController;
    public GameObject rightHand;
    public GameObject rightController;
    public List<GameObject> L_Button = new List<GameObject>();
    public List<GameObject> R_Button = new List<GameObject>();
    private XROrigin xrOrigin;
    
    [Header("Tutorial Object")]
    public GameObject vrButton;
    public List<GameObject> grabableCube = new List<GameObject>();
    private List<XRGrabInteractable> grabbable = new List<XRGrabInteractable>();
    public Pistol tutorialPistol;
    public List<ScoreEffect> lolipops = new List<ScoreEffect>();
    public Image loadingImage;
    public GameObject keyItem;
    public XRGrabInteractable key;


    [Header("Key")]
    public InputActionProperty btnA;
    public InputActionProperty btnB;
    public InputActionProperty btnX;
    public InputActionProperty btnY;
    public InputActionProperty btnLGrip;
    public InputActionProperty btnRGrip;
    public InputActionProperty btnLTrigger;
    public InputActionProperty btnRTrigger;
    public InputActionProperty btnLThumbStick;
    public InputActionProperty btnRThumbStick;

    void SetComponentEnabled<T>(bool isEnabled) where T : MonoBehaviour
    {
        MonoBehaviour componentToDisable = xrOrigin.GetComponent<T>();
        componentToDisable.enabled = isEnabled;
    }
    private void Awake()
    {
        // 빌드 창 설정
        // Screen.SetResolution(960, 540, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 시작하자마자 조작키 비활성화
        if (xrOrigin == null) xrOrigin = FindObjectOfType<XROrigin>();
        SetComponentEnabled<ActionBasedContinuousMoveProvider>(false);
        SetComponentEnabled<ActionBasedContinuousTurnProvider>(false);
        SetComponentEnabled<TeleportationProvider>(false);
        // SetComponentEnabled<tutorial.ActivateTeleportationRay>(false);
        
        foreach (var VARIABLE in L_Button)
        {
            VARIABLE.SetActive(false);
        }
        foreach (var VARIABLE in R_Button)
        {
            VARIABLE.SetActive(false);
        }

        foreach (var VARIABLE in grabableCube)
        {
            grabbable.Add(VARIABLE.GetComponent<XRGrabInteractable>());
        }
        
        
        curIndex = 0;
        wellcomeUIIndex = m_wellcomeImgData.Length;
        tutorialUIIndex = m_tutorialImgData.Length;
        entranceUIIndex = m_entrancetImgData.Length;
        temp = 0;
        showUI.sprite = m_wellcomeImgData[curIndex];
        isBtn1Pressed = false;
        isBtn2Pressed = false;
        isPressed = false;
        isGrab = false;
        isPortal = false;

        StartCoroutine(FadeOutImage());
    }

    IEnumerator FadeOutScene()
    {
        Debug.Log("열쇠 잡으면 시작");
        timer = 0;
        float delay = 3f;
        float percent = 50f;
        Color targetColor = new Color(0, 0, 0, 1); // 목표 알파 값
        key.selectEntered.AddListener(OnActivateGravity);
        key.selectEntered.AddListener(OnGrab);

        while (true)
        {
            if (isGrab)
            {
                timer += Time.deltaTime / delay;
                loadingImage.color = Color.Lerp(loadingImage.color, targetColor, timer/percent);    // 색상의 알파 값을 서서히 변경
                if (timer >= delay)
                {
                    // lerpValue = timer;      // 타이머가 얼마나 진행되었는지 비율로 계산
                    
                    break;
                }
            }
            yield return null;
        }
        SceneManager.LoadScene(2);
        // yield return StartCoroutine(FadeOutImage());
    }

    IEnumerator FadeOutImage()
    {
        timer = 0;
        lerpValue = 0;
        float delay = 2f;
        float percent = 20f;
        Color targetColor = new Color(1, 1, 1, 1); // 목표 알파 값
        while (timer < delay)
        {
            timer += Time.deltaTime;
            lerpValue = Mathf.Clamp01(timer / percent);       // 타이머가 얼마나 진행되었는지 비율로 계산
            showUI.color = Color.Lerp(showUI.color, targetColor, lerpValue);    // 색상의 알파 값을 서서히 변경
            yield return null;
        }
        yield return StartCoroutine(WellcomeSentence_Play());
    }

    // 컨트롤러 -> 손
    void ChangeController()
    {
        leftHand.SetActive(true);
        leftController.SetActive(false);
        rightHand.SetActive(true);
        rightController.SetActive(false);
    }
    
    void SpawnCube()
    {
        foreach (var VARIABLE in grabableCube)
        {
            VARIABLE.SetActive(true);
        }
    }
    void SpawnTarget()
    {
        foreach (var item in lolipops)
        {
            item.gameObject.SetActive(true);
        }
    }
    public void PressBtn() => isPressed = true;
    void OnGrab(SelectEnterEventArgs arg) => isGrab = true;
    void OnActivateGravity(SelectEnterEventArgs arg) 
    {
        // key.transform.SetParent(null);
        Rigidbody rb = key.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
    }
    void OnRelease(SelectExitEventArgs arg) => isGrab = false;
    public void OnPortal() => isPortal = true;
    void EntranceSentence() => StartCoroutine(EntranceSentence_Play());

    void PlayVoice(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
    private bool CheckAllTarget()
    {
        int count = 0;
        foreach (var item in lolipops)
        {
            if (item.gameObject.activeSelf == true)
            {
                return false;
            }
            else
            {
                count++;
            }
        }
        return count == 5;
    }

    #region WelcomeFlow

    IEnumerator WellcomeSentence_Play()
    {
        int idx = 0;           // wellcomeUIIndex = 4
        mainAudioSource.Play();     // 메인 음악 재생
        
        // 3되면 탈출하는데... 문제는 화면이 안바뀜
        while (idx < wellcomeUIIndex - 1)
        {
            showUI.sprite = m_wellcomeImgData[idx];    // 0번 3초간,1,2,
            PlayVoice(idx);
            yield return new WaitUntil(() => !audioSource.isPlaying);
            yield return new WaitForSeconds(1f);
            idx++;
        }

        showUI.sprite = m_wellcomeImgData[idx];
        PlayVoice(idx);

        while (idx == wellcomeUIIndex - 1)
        {
            // yield return new WaitUntil(() => !audioSource.isPlaying);
            // yield return new WaitForSeconds(1f);
            // yield return StartCoroutine(tutorialSentence_Play());
            // break;
            if (btnA.action.IsPressed())
            {
                audioSource.Stop();
                yield return StartCoroutine(tutorialSentence_Play());
                Debug.Log("A누름");
                break;
            }
            if (btnB.action.IsPressed())
            {
                audioSource.Stop();
                yield return StartCoroutine(EntranceSentence_Play());
                Debug.Log("B누름");
                break;              
            }

            yield return null;      // 입력받을떄까지 대기
        }
        print("WellcomeSentence_Play 코루틴이 끝남");
    }

    #endregion

    #region TutorialFlow

    IEnumerator tutorialSentence_Play()
    {
        PlayVoice(curIndex + 4);
        
        Debug.Log("튜토리얼 시작");
        showUI.sprite = m_tutorialImgData[curIndex];
        
        L_Button[curIndex].SetActive(true);
        R_Button[curIndex].SetActive(true);            
        
        yield return new WaitForSeconds(1f);
        
        //엄지로 빛나는 버튼 A, X를 모두 눌러보세요.
        while (curIndex == 0)
        {
            Debug.Log("엄지로 빛나는 버튼 A, X를 모두 눌러보세요.");
            if (btnA.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                r_controllerBtnSound.Play();      // 블록같은 효과음
                r_clickEffects[curIndex].gameObject.SetActive(true);
                r_clickEffects[curIndex].Play();
                Debug.Log("A 누름");
                R_Button[curIndex].SetActive(false);
                isBtn1Pressed = true;
                temp = 0f;
            }
            if (btnX.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                l_controllerBtnSound.Play();
                l_clickEffects[curIndex].gameObject.SetActive(true);
                l_clickEffects[curIndex].Play();
                Debug.Log("X 누름");
                L_Button[curIndex].SetActive(false);
                isBtn2Pressed = true;
                temp = 0f;
            }
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    L_Button[curIndex].SetActive(true);
                    R_Button[curIndex].SetActive(true);
                    break;
                }
            }
            yield return null;
        }
        
        PlayVoice(curIndex + 4);
        // 엄지로 빛나는 버튼 B, Y를 모두 눌러보세요.
        while (curIndex == 1)
        {
            // 두 버튼 한번씩은 눌러야 함.
            Debug.Log("엄지로 빛나는 버튼 B, Y를 모두 눌러보세요.");
            if (btnB.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                r_controllerBtnSound.Play();      // 블록같은 효과음
                r_clickEffects[curIndex].gameObject.SetActive(true);
                r_clickEffects[curIndex].Play();
                Debug.Log("B 누름");
                R_Button[curIndex].SetActive(false);
                isBtn1Pressed = true;
                temp = 0f;
            }
            if (btnY.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                l_controllerBtnSound.Play();      // 블록같은 효과음
                l_clickEffects[curIndex].gameObject.SetActive(true);
                l_clickEffects[curIndex].Play();
                Debug.Log("Y 누름");
                L_Button[curIndex].SetActive(false);
                isBtn2Pressed = true;
                temp = 0f;
            }
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    L_Button[curIndex].SetActive(true);
                    R_Button[curIndex].SetActive(true);
                    break;
                }
            }
            yield return null;
        }

        PlayVoice(curIndex + 4);
        // 엄지로 아날로그 스틱을 움직여보세요.
        while (curIndex == 2)
        {
            Debug.Log("엄지로 아날로그 스틱을 움직여보세요.");
            
            if (btnRThumbStick.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                r_controllerBtnSound.Play();      // 블록같은 효과음
                r_clickEffects[curIndex].gameObject.SetActive(true);
                r_clickEffects[curIndex].Play();
                Debug.Log("btnRThumbStick 돌림");
                R_Button[curIndex].SetActive(false);
                isBtn2Pressed = true;
                temp = 0f;
            }
            if (btnLThumbStick.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                l_controllerBtnSound.Play();      // 블록같은 효과음
                l_clickEffects[curIndex].gameObject.SetActive(true);
                l_clickEffects[curIndex].Play();
                Debug.Log("btnLThumbStick 돌림");
                L_Button[curIndex].SetActive(false);
                isBtn1Pressed = true;
                temp = 0f;
            }
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    L_Button[curIndex].SetActive(true);
                    R_Button[curIndex].SetActive(true);
                    break;
                }
            }
            yield return null;
        }
        
        PlayVoice(curIndex + 4);
        // 이번엔 검지를 이용해 컨트롤러의 트리거를 쥐어보세요.
        while (curIndex == 3)
        {
            Debug.Log("이번엔 검지를 이용해 컨트롤러의 트리거를 쥐어보세요.");
            if (btnRTrigger.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                r_controllerBtnSound.Play();      // 블록같은 효과음
                r_clickEffects[curIndex].gameObject.SetActive(true);
                r_clickEffects[curIndex].Play();
                Debug.Log("RTrigger 누름");
                R_Button[curIndex].SetActive(false);
                isBtn1Pressed = true;
                temp = 0f;
            }
            if (btnLTrigger.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                l_controllerBtnSound.Play();      // 블록같은 효과음
                l_clickEffects[curIndex].gameObject.SetActive(true);
                l_clickEffects[curIndex].Play();
                Debug.Log("LTrigger 누름");
                L_Button[curIndex].SetActive(false);
                isBtn2Pressed = true;
                temp = 0f;
            }
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    L_Button[curIndex].SetActive(true);
                    R_Button[curIndex].SetActive(true);
                    break;
                }
            }
            yield return null;
        }
        
        PlayVoice(curIndex + 4);
        // 그립 버튼을 확인하고 중지로 쥐어보세요.
        while (curIndex == 4)
        {
            Debug.Log("그립 버튼을 확인하고 중지로 쥐어보세요.");
            if (btnRGrip.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                r_controllerBtnSound.Play();      // 블록같은 효과음
                r_clickEffects[curIndex].gameObject.SetActive(true);
                r_clickEffects[curIndex].Play();
                Debug.Log("RGrip 누름");
                R_Button[curIndex].SetActive(false);
                isBtn1Pressed = true;
                temp = 0f;
            }
            if (btnLGrip.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                l_controllerBtnSound.Play();      // 블록같은 효과음
                l_clickEffects[curIndex].gameObject.SetActive(true);
                l_clickEffects[curIndex].Play();
                Debug.Log("LGrip 누름");
                L_Button[curIndex].SetActive(false);
                isBtn2Pressed = true;
                temp = 0f;
            }
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    break;
                }
            }
            yield return null;
        }
        
        PlayVoice(curIndex + 4);
        // 이제 이 가상 손으로 무엇을 할 수 있는지 볼까요?
        while (curIndex == 5)
        {
            Debug.Log("이제 이 가상 손으로 무엇을 할 수 있는지 볼까요?");
            leftHandTransition.gameObject.SetActive(true);
            rightHandTransition.gameObject.SetActive(true);
            leftHandTransition.Play();
            rightHandTransition.Play();
            yield return new WaitForSeconds(3f);
            ChangeController();
            yield return new WaitUntil(() => !rightHandTransition.isPlaying);
            yield return new WaitForSeconds(1f);
            leftHandTransition.gameObject.SetActive(false);
            rightHandTransition.gameObject.SetActive(false);
            tutorialPistol.gameObject.SetActive(true);
            curIndex++;
            showUI.sprite = m_tutorialImgData[curIndex];
            temp = 0;
        }

        /* PlayVoice(curIndex + 4);
        // 주먹을 움켜쥐려면 그립 버튼과 트리거 버튼을 모두 쥐고 있으세요.
        while (curIndex == 6)
        {
            Debug.Log("주먹을 움켜쥐려면 그립 버튼과 트리거 버튼을 모두 쥐고 있으세요.");
            // 둘다 쥐기
            if (btnRTrigger.action.IsPressed() && btnRGrip.action.IsPressed() && !isBtn1Pressed)
            {
                Debug.Log("오른쪽 주먹쥠");
                isBtn1Pressed = true;
            }
            if (btnLTrigger.action.IsPressed() && btnLGrip.action.IsPressed() && !isBtn2Pressed)
            {
                Debug.Log("왼쪽 주먹쥠");
                isBtn2Pressed = true;
            }
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    break;
                }
            }
            yield return null;
        }

        PlayVoice(curIndex + 4);
        // 가리키려면 그립버튼을 쥔 상태에서 검지만 떼세요.
        while (curIndex == 7)
        {
            Debug.Log("가리키려면 그립버튼을 쥔 상태에서 검지만 떼세요.");
            
            if (!btnRTrigger.action.WasPressedThisFrame() && btnRGrip.action.WasPressedThisFrame() && !isBtn1Pressed)
            {
                Debug.Log("오른쪽 가리킴");
                isBtn1Pressed = true;
                temp = 0f;
            }
            if (!btnLTrigger.action.WasPressedThisFrame() && btnLGrip.action.WasPressedThisFrame() && !isBtn2Pressed)
            {
                Debug.Log("왼쪽 가리킴");
                isBtn2Pressed = true;
                temp = 0f;
            }
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isBtn1Pressed && isBtn2Pressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    isBtn1Pressed = false;
                    isBtn2Pressed = false;
                    temp = 0f;
                    vrButton.SetActive(true);
                    break;
                }
            }
            yield return null;
        }

        PlayVoice(curIndex + 4);
        // 이제 검지를 이용해 앞에 있는 버튼을 눌러보세요
        while (curIndex == 8)
        {
            Debug.Log("이제 검지를 이용해 앞에 있는 버튼을 눌러보세요");
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isPressed)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    vrButton.SetActive(false);
                    SpawnCube();
                    temp = 0f;
                    break;
                }
            }
            yield return null;
        }

        PlayVoice(curIndex + 4);
        // 물체를 집으려면 중지로 그립 버튼을 쥔 상태를 유지하면 됩니다.
        // 손을 뻗어서 블록 하나를 집어보세요.
        while (curIndex == 9)
        {
            for (int i = 0; i < grabbable.Count; i++)
            {
                grabbable[i].selectEntered.AddListener(OnGrab);        // 잡으면 isGrab = true로 바꾸는 함수 등록
            }
            
            // 여러 개 큐브중 적어도 하나를 잡으면 다음 인덱스로 넘어가기
            if (isGrab)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    temp = 0f;
                    break;
                }
            }
            yield return null;
        }

        PlayVoice(curIndex + 4);
        // 놓으려면 그립 버튼을 때세요.
        while (curIndex == 10)      // 15
        {
            for (int i = 0; i < grabbable.Count; i++)
            {
                grabbable[i].selectExited.AddListener(OnRelease);
            }
            // grabbable.selectExited.AddListener(OnRelease);        // 놓으면 isGrab = false로 바꾸는 함수 등록
            // 놓으면 다음 인덱스로 넘어가기
            if (isGrab == false)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    
                    tutorialPistol.gameObject.SetActive(true);
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    temp = 0f;
                    break;
                }
            }
            yield return null;
        }*/

        // 이제 눈 앞에 있는 총을 잡아보겠습니다. 
        PlayVoice(curIndex + 4);
        while (curIndex ==  6)
        {
            Debug.Log("이제 눈 앞에 있는 총을 잡아보겠습니다. ");
            if (tutorialPistol.isGrapped)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    SpawnTarget();
                    // 다음 UI띄우기
                    curIndex++;
                    showUI.sprite = m_tutorialImgData[curIndex];
                    temp = 0f;
                    break;
                }
            }
            yield return null;
        }

        // 총을 잡은 채, 검지로 트리거를 누르면 총알이 나갑니다. 
        PlayVoice(curIndex + 4);
        while (curIndex == 7)
        {
            Debug.Log("총을 잡은 채, 검지로 트리거를 누르면 총알이 나갑니다.");
            Debug.Log("한번 앞에 있는 롤리팝 과녁을 맞춰볼까요?");            

            if (CheckAllTarget())
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    yield return StartCoroutine(EntranceSentence_Play());
                    break;
                }
            }
            yield return null;
        }

        /*PlayVoice(curIndex + 4);
        // 거대한 원기둥에 레이저를 갖다대면 빠르게 이동이 가능합니다.
        while (curIndex == tutorialUIIndex - 1)
        {
            Debug.Log("투명 원기둥를 향해 검지로 트리거 버튼을 쥐었다 피면 원기둥으로 빠르게 이동이 가능합니다.");
            SetComponentEnabled<TeleportationProvider>(true);
            SetComponentEnabled<tutorial.ActivateTeleportationRay>(true);
            
            // 한번 누르면 카운트를 세고 다음 UI뜨기까지 딜레이를 줌
            if (isPortal)
            {
                temp += Time.deltaTime;
                if (temp >= delay)
                {
                    audioSource.Stop();
                    EntranceSentence();
                    break;
                }
            }
            yield return null;
        }*/
        // 마지막에 꼭 EntranceSentence(); 넣기
    }

    #endregion
    
    #region EntranceFlow

    IEnumerator EntranceSentence_Play()
    {
        tutorialPistol.gameObject.SetActive(false);
        keyItem.SetActive(true);
        ChangeController();
        isGrab = false;
        int idx = 0;
        PlayVoice(audioClips.Count - 1);        // 음성 재생
        while (idx < entranceUIIndex)
        {
            showUI.sprite = m_entrancetImgData[idx];
            idx++;
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }
        print("코루틴 끝");
        yield return StartCoroutine(FadeOutScene());
    }

    #endregion   

}
