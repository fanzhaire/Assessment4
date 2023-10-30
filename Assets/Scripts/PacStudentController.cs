using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5.0f;
    public Animator animator;
    public ParticleSystem dustParticles;
    public AudioSource moveAudio;
    public Transform leftTeleporterPosition;
    public Transform rightTeleporterPosition;
    public AudioClip wallCollisionSound;
    public ParticleSystem wallCollisionParticles;
    private bool isTeleporting = false;

    public Text scoreText;
    private int score = 0;
    public AudioClip diamondSound;

    public AudioSource scaredAudioSource;


    public Text countdownText;
    private float scaredTime = 10f;
    private bool isScared = false;
    private bool isRecovering = false;

    public TextMeshProUGUI gameOverText;// 在Unity编辑器中添加Game Over文本引用
    private bool isGameOver = false;
    public string startSceneName = "StartScene"; // 请设置为您的开始场景的名称
    public int totalPellets; // 设置关卡中的总球数
    private int eatenPellets = 0;

    public Text gameTimerText;  // 游戏计时器文本组件的引用
    private float elapsedTime = 0f; // 流逝的时间
    private bool isGameStarted = false; // 用于判断游戏是否已经开始

    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rb;

    private Dictionary<KeyCode, Vector2> directions = new Dictionary<KeyCode, Vector2>
    {
        { KeyCode.W, Vector2.up },
        { KeyCode.A, Vector2.left },
        { KeyCode.S, Vector2.down },
        { KeyCode.D, Vector2.right }
    };

    private Dictionary<KeyCode, string> animationTriggers = new Dictionary<KeyCode, string>
    {
        { KeyCode.W, "MoveUp" },
        { KeyCode.A, "MoveLeft" },
        { KeyCode.S, "MoveDown" },
        { KeyCode.D, "MoveRight" }
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAudio.Stop();
        gameOverText.gameObject.SetActive(false); // 默认隐藏Game Over文本
        gameTimerText.text = "00:00:00";

    }

    void Update()
    {
        if (isGameStarted && !isGameOver)
        {
            elapsedTime += Time.deltaTime;
            int minutes = (int)elapsedTime / 60;
            int seconds = (int)elapsedTime % 60;
            int milliseconds = (int)(elapsedTime * 100) % 100;

            gameTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        if (!isGameOver)
        {
            GetInput();
            PlayAnimationBasedOnDirection();
            CheckGameOver();
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
    }
    void CheckGameOver()
    {
        if (eatenPellets >= totalPellets)
        {
            StartCoroutine(ShowGameOver());
        }
    }
    IEnumerator ShowGameOver()
    {
        isGameOver = true;
        isGameStarted = false;
        // 停止玩家和游戏计时器
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;

        // 如果您有其他音频源或动画，也请停止它们
        moveAudio.Stop();
        scaredAudioSource.Stop();
        countdownText.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);

        int currentScore = score;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.SetFloat("BestTime", elapsedTime);
        }
        else if (currentScore == highScore && elapsedTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", elapsedTime);
        }



        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneName);
    }

    void GetInput()
    {
        foreach (var keyDirectionPair in directions)
        {
            if (Input.GetKeyDown(keyDirectionPair.Key))
            {
                moveDirection = keyDirectionPair.Value;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;

        if (moveDirection != Vector2.zero && !moveAudio.isPlaying)
        {
            moveAudio.Play();
        }

        else if (moveDirection == Vector2.zero)
        {
            moveAudio.Stop();
        }
    }

    void PlayAnimationBasedOnDirection()
    {
        foreach (var pair in animationTriggers)
        {
            if (directions[pair.Key] == moveDirection)
            {
                animator.SetTrigger(pair.Value);
            }
            else
            {
                animator.ResetTrigger(pair.Value);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioSource.PlayClipAtPoint(wallCollisionSound, transform.position);
            wallCollisionParticles.Play();
            moveDirection = Vector2.zero;
        }
        else if (collision.gameObject.CompareTag("LeftTeleporter") && !isTeleporting)
        {
            StartCoroutine(TeleportCooldown(rightTeleporterPosition));
        }
        else if (collision.gameObject.CompareTag("RightTeleporter") && !isTeleporting)
        {
            StartCoroutine(TeleportCooldown(leftTeleporterPosition));
        }
    }

    IEnumerator TeleportCooldown(Transform teleportToPosition)
    {
        isTeleporting = true;
        Vector3 offset = moveDirection * 0.2f;
        transform.position = teleportToPosition.position + offset;
        yield return new WaitForSeconds(0.1f);
        isTeleporting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Diamond"))
        {
            eatenPellets++;
            score += 10;
            UpdateScoreUI();

            moveAudio.Stop();

            moveAudio.PlayOneShot(diamondSound);

            StartCoroutine(PlayMoveAudioAfterDelay(diamondSound.length));

            Destroy(other.gameObject);
        }
        if (other.CompareTag("bounscherry"))
        {
            score += 100;
            UpdateScoreUI();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("powerpellet"))
        {
            score += 100;
            UpdateScoreUI();


            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            scaredAudioSource.Play();

            StartCoroutine(ScaredCountdown());

            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
            foreach (GameObject ghost in ghosts)
            {
                Animator ghostAnimator = ghost.GetComponent<Animator>();
                if (ghostAnimator != null)
                {
                    ghostAnimator.SetTrigger("scared");
                }
            }

            Destroy(other.gameObject);
        }

    }


    IEnumerator ScaredCountdown()
    {
        float timer = scaredTime;

        countdownText.gameObject.SetActive(true);

        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();

            if (timer <= 3 && !isRecovering)
            {
                isRecovering = true;
                GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
                foreach (GameObject ghost in ghosts)
                {
                    Animator ghostAnimator = ghost.GetComponent<Animator>();
                    if (ghostAnimator != null)
                    {
                        ghostAnimator.SetTrigger("recover");
                    }
                }
            }

            yield return new WaitForSeconds(1);
            timer -= 1;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        scaredAudioSource.Stop();

        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("GhostTag");
        foreach (GameObject ghost in allGhosts)
        {
            Animator ghostAnimator = ghost.GetComponent<Animator>();
            if (ghostAnimator != null)
            {
                ghostAnimator.SetTrigger("back");
            }
        }
        isScared = false;
        isRecovering = false;
    }


    IEnumerator PlayMoveAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay + 1f);
        if (moveDirection != Vector2.zero)
        {
            moveAudio.Play();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }


}