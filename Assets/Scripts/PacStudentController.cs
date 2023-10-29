using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text scoreText;  // 分数的UI文本
    private int score = 0;  // 当前分数
    public AudioClip diamondSound;

    public AudioSource scaredAudioSource;
    

    public Text countdownText; // 这是一个UnityEngine.UI.Text，需要指向Canvas上的文本组件
    private float scaredTime = 10f;
    private bool isScared = false;
    private bool isRecovering = false;

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
    }

    void Update()
    {
        GetInput();
        PlayAnimationBasedOnDirection();
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
            score += 10;  // 增加分数
            UpdateScoreUI();  // 更新UI文本

            // 暂停移动声音
            moveAudio.Stop();

            // 播放钻石声音
            moveAudio.PlayOneShot(diamondSound);

            // 立即开始播放移动声音
            StartCoroutine(PlayMoveAudioAfterDelay(diamondSound.length));

            Destroy(other.gameObject);
        }
        if (other.CompareTag("bounscherry"))
        {
            score += 100;  // 增加分数
            UpdateScoreUI();  // 更新UI文本
            Destroy(other.gameObject);  // 销毁钻石
        }
        if (other.CompareTag("powerpellet"))
        {
            score += 100;  // 增加分数
            UpdateScoreUI();  // 更新UI文本

            // 停止所有音乐
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            // 播放 scaredSound
            
            scaredAudioSource.Play();

            // 启动倒计时
            StartCoroutine(ScaredCountdown());

            // 触发所有Ghost的 scared 动画
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
            foreach (GameObject ghost in ghosts)
            {
                Animator ghostAnimator = ghost.GetComponent<Animator>();
                if (ghostAnimator != null)
                {
                    ghostAnimator.SetTrigger("scared");
                }
            }

            Destroy(other.gameObject);  // 销毁 powerpellet
        }

    }


    IEnumerator ScaredCountdown()
    {
        float timer = scaredTime;

        countdownText.gameObject.SetActive(true);  // 激活countdownText

        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString(); // 更新Canvas上的倒数计时

            if (timer <= 3 && !isRecovering)
            {
                isRecovering = true; // 避免多次调用
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

        countdownText.text = ""; // 清除Canvas上的倒数计时
        countdownText.gameObject.SetActive(false);  // 禁用countdownText
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
        isRecovering = false; // 重置
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
