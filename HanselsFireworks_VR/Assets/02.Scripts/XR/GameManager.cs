using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VR
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                if (null == instance)
                {
                    return null;
                }
                return instance;
            }
        }

        private void Awake()
        {
            var obj = FindObjectsOfType<VR.GameManager>();
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            /*if(obj.Length == 1)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }*/
        }

        [Header("Info")]
        public int score;
        public int currentStage;
        public int leftMonster;
        public float hp;
        public float takenTime;
        public float elapseTime;        
        public float spawnDuration;
        public float nextSpawnHeight;


        [Header("Countdown")]
        public float delay;
        public float percent;
        public Image playerUI;
        public Image countUI;
        public Sprite[] sprites;

        [Header("Audio")]
        public List<AudioClip> audioClips;
        public AudioSource audioSource;

        [Header("Manager")]
        [SerializeField] private Marshmallow marshmallow;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private AudioSource mainBGM;


        [Header("UI")]
        public GameObject warningScreen;
        public GameObject gameoverScreen;
        public Player player;
        public Status status;

        // Start is called before the first frame update
        void Start()
        {
            marshmallow = FindObjectOfType<Marshmallow>();
            spawnManager = FindObjectOfType<SpawnManager>();
            status = FindObjectOfType<Status>();
            player = FindObjectOfType<Player>();
            // limitedTime = 5;
            elapseTime = 0;
            nextSpawnHeight = 0.3f;
            spawnDuration = 2;
            Init();
            StartCoroutine(FadeOutImage());            
        }

        private void Init()
        {
            hp = 100;
            score = 0;
        }

        void PlaySound(int index)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }

        public void StartStage()
        {
            marshmallow.StartStage();
            currentStage = 1;            
        }
        public void ControlStoppedTime()
        {
            // Debug.Log("ControlStoppedTime");
            if (marshmallow.growSpeed <= 0.2f)
            {
                elapseTime += Time.deltaTime;
            }
            else
            {
                if (elapseTime <= 0) return;
                elapseTime -= Time.deltaTime;
            }
        }
        
        IEnumerator FadeOutImage()
        {
            float timer = 0;
            float lerpValue = 0;
            Color targetColor = new Color(0, 0, 0, 0); // 목표 알파 값
            while (timer < delay)
            {
                timer += Time.deltaTime;
                lerpValue = Mathf.Clamp01(timer / percent);       // 타이머가 얼마나 진행되었는지 비율로 계산
                playerUI.color = Color.Lerp(playerUI.color, targetColor, lerpValue);    // 색상의 알파 값을 서서히 변경
                yield return null;
            }
            yield return StartCoroutine(CountDown());
        }


        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(1f);
            countUI.color = new Color(1, 1, 1, 1);
            countUI.sprite = sprites[3];
            PlaySound(0);
            yield return new WaitForSeconds(1f);
            countUI.sprite = sprites[2];
            PlaySound(0);
            yield return new WaitForSeconds(1f);
            countUI.sprite = sprites[1];
            PlaySound(0);
            yield return new WaitForSeconds(1f);
            countUI.sprite = sprites[0];
            PlaySound(3);
            yield return new WaitForSeconds(1f);
            countUI.color = new Color(1, 1, 1, 0);
            mainBGM.Play();
            StartStage();
            yield return StartCoroutine(CheckObjective());
            // yield return StartCoroutine(CheckTime());
        }

        IEnumerator CheckTime()
        {            
            takenTime += Time.deltaTime;
            yield return null;            
        }

        IEnumerator CheckObjective()
        {
            Debug.Log("호출 시작");
            while (true)
            {
                takenTime += Time.deltaTime;
                if (status.CurrentHP <= 0) 
                { 
                    Debug.Log("Lose");
                    GameOver();
                    // 마시멜로 멈추고 GameOver UI 띄우기
                    yield break;
                }

                yield return null;
            }
        }

        public void Restart()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(1);
            Destroy(gameObject);
        }

        public void GameOver()
        {
            gameoverScreen.SetActive(true);
            warningScreen.SetActive(false);
            player.TakeOffGun();
            player.ActivateGrabRay();
            marshmallow.StopGrowing();
            marshmallow.enabled = false;
        }

    }
}
