// using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("UI")]
    public TextMeshProUGUI tLeftTime;
    public TextMeshProUGUI tLeftMonster;
    public TextMeshProUGUI tScore;
    public TextMeshProUGUI tCombo;
    public TextMeshProUGUI tLeftCase;
    public Image stageInfo;
    public Image specialSkillIcon;

    [Header("Result UI")]
    public GameObject resultPenel;
    public GameObject playPenel;
    public GameObject feverUI;
    public TextMeshProUGUI[] tStageScores;
    public TextMeshProUGUI tTotalScore;
    public TextMeshProUGUI tComboPlayScreen;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bonusSound;
    [SerializeField] private AudioClip sumResult;

    [SerializeField] private Animator animResultUI;
    [SerializeField] private Animator animPlayUI;
    [SerializeField] private Animator animFeverUI;
    [SerializeField] private float loadingTime;
    [SerializeField] private Sprite[] stageImages;
    private int initTotalScore;
    private Player player;
    private AudioSource audioSource;
    private bool doOnce;
    private bool doOnce_2;


    [Header("Init variable")]
    public int[] maxTime;



    // Start is called before the first frame update
    void Start()
    {
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftTime.text = GameManager.Instance.LeftTime.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        
        animResultUI = resultPenel.GetComponent<Animator>();
        animPlayUI = playPenel.GetComponent<Animator>();
        animFeverUI = feverUI.GetComponent<Animator>();

        player = FindAnyObjectByType<Player>();
        
        GameManager.Instance.Init();
        
        initTotalScore = GameManager.Instance.totalScore;
        audioSource = GetComponent<AudioSource>();
        // 마우스 커서를 보이게 하고 잠금을 해제합니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        doOnce = false;
        doOnce_2 = true;
        InitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        tLeftTime.text = GameManager.Instance.LeftTime.ToString();
        tLeftMonster.text = GameManager.Instance.leftMonster.ToString();
        tScore.text = GameManager.Instance.score.ToString();
        tCombo.text = GameManager.Instance.combo.ToString();
        tLeftCase.text = GameManager.Instance.leftCase.ToString();
        //FeverUI();
    }

    void FeverUI()
    {
        if (GameManager.Instance.leftCase > 0 && doOnce == false)
        {
            doOnce = true;
            animFeverUI.SetBool("Fever", doOnce);
            player.ActivateFeverModeEffect(true);
        }

        if(GameManager.Instance.leftCase <= 0)
        {
            doOnce = false;
            animFeverUI.SetBool("Fever", doOnce);
            player.ActivateFeverModeEffect(false);
        }
    }

    void SetComponentEnabled<T>(bool isEnabled) where T : MonoBehaviour
    {
        MonoBehaviour componentToDisable = player.GetComponent<T>();
        componentToDisable.enabled = isEnabled;
    }

    public void PlayStart()
    {
        // 컴포넌트 활성화 
        SetComponentEnabled<Player>(true);
        SetComponentEnabled<PlayerMovement>(true);
        SetComponentEnabled<SpecialSkill>(true);
    }

    public void PlayEnd()
    {
        // 플레이어에 있는 모든 AudioSource 컴포넌트를 가져와 mute시킨다.
        AudioSource[] moveSource = player.GetComponents<AudioSource>();
        foreach (var item in moveSource) item.mute = true;

        // 컴포넌트 비활성화 
        // player.audioSource.Stop();
        player.fireGun.gameObject.SetActive(false);     // 총 애니메이션을 비활성화해야하는데 그냥 총 비활성화...
        SetComponentEnabled<Player>(false);
        SetComponentEnabled<PlayerMovement>(false);
        SetComponentEnabled<SpecialSkill>(false);
    }


    public void ShowResultUI()
    {
        InitScore();
        print("UI");
        animResultUI.SetBool("End", true);

        StartCoroutine(ShowResult());
    }

    public void ShowBonusUI()
    {
        tComboPlayScreen.text = GameManager.Instance.combo.ToString();
        animPlayUI.SetBool("KillAllMonster", true);
        audioSource.clip = bonusSound;
        audioSource.Play();
    }

    // 결과 초기 점수 갱신
    public void InitScore()
    {
        for (int i = 0; i < tStageScores.Length; i++)
        {
            tStageScores[i].text = GameManager.Instance.stageScore[i].ToString();
        }
        // 초기 결과 점수 
        tTotalScore.text = initTotalScore.ToString();
    }

    public void InitInfo()
    {
        int curStage = GameManager.Instance.currentStage;
        stageInfo.sprite = stageImages[curStage];
    }

    IEnumerator ShowResult()
    {
        int curStage = GameManager.Instance.currentStage;
        int currentStageScore = GameManager.Instance.stageScore[curStage];
        int totalScore = GameManager.Instance.totalScore;
        yield return StartCoroutine(AnimateStageScore(0, currentStageScore));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(AnimateTotalScore(initTotalScore, totalScore));
    }

    IEnumerator AnimateStageScore(int a, int b)
    {
        int curStage = GameManager.Instance.currentStage;
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / loadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;

            tempScore = (int)Mathf.Lerp(a, b, temp);

            tStageScores[curStage].text = tempScore.ToString();
            if (tempScore >= b)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator AnimateTotalScore(int a, int b)
    {
        audioSource.clip = sumResult;
        audioSource.Play();
        float temp = 0;
        int tempScore = 0;
        float LoadingDuration = 1f / loadingTime;
        while (true)
        {
            temp += Time.deltaTime * LoadingDuration;

            tempScore = (int)Mathf.Lerp(a, b, temp);

            tTotalScore.text = tempScore.ToString();
            if (tempScore >= b)
            {
                
                break;
            }
            yield return null;
        }
    }



    // 게임 시작
    public void StartUI()
    {
        if (doOnce_2)
        {
            doOnce_2 = false;
            // 3,2,1 실행. 이거 코루틴
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameManager.Instance.PlayMainBGM();
            GameManager.Instance.SetTimer();
            GameManager.Instance.SetObjective();
            // GameManager.Instance.SetEnemies(true);  // 모든 적들 활성화하는 코드

            print("시작 UI");
        }        
    }
    // 보스전 직전 씬에서 Start버튼 이벤트에 등록될 함수
    public void StartBossStage()
    {
        // 3,2,1 실행. 이거 코루틴
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameManager.Instance.PlayBossBGM();
        GameManager.Instance.SetTimer();
        GameManager.Instance.SetObjective();

        print("보스 UI시작");
    }

}
