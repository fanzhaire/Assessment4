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

    public Text scoreText;  // ������UI�ı�
    private int score = 0;  // ��ǰ����
    public AudioClip diamondSound;

    public AudioSource scaredAudioSource;
    

    public Text countdownText; // ����һ��UnityEngine.UI.Text����Ҫָ��Canvas�ϵ��ı����
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
            score += 10;  // ���ӷ���
            UpdateScoreUI();  // ����UI�ı�

            // ��ͣ�ƶ�����
            moveAudio.Stop();

            // ������ʯ����
            moveAudio.PlayOneShot(diamondSound);

            // ������ʼ�����ƶ�����
            StartCoroutine(PlayMoveAudioAfterDelay(diamondSound.length));

            Destroy(other.gameObject);
        }
        if (other.CompareTag("bounscherry"))
        {
            score += 100;  // ���ӷ���
            UpdateScoreUI();  // ����UI�ı�
            Destroy(other.gameObject);  // ������ʯ
        }
        if (other.CompareTag("powerpellet"))
        {
            score += 100;  // ���ӷ���
            UpdateScoreUI();  // ����UI�ı�

            // ֹͣ��������
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            // ���� scaredSound
            
            scaredAudioSource.Play();

            // ��������ʱ
            StartCoroutine(ScaredCountdown());

            // ��������Ghost�� scared ����
            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
            foreach (GameObject ghost in ghosts)
            {
                Animator ghostAnimator = ghost.GetComponent<Animator>();
                if (ghostAnimator != null)
                {
                    ghostAnimator.SetTrigger("scared");
                }
            }

            Destroy(other.gameObject);  // ���� powerpellet
        }

    }


    IEnumerator ScaredCountdown()
    {
        float timer = scaredTime;

        countdownText.gameObject.SetActive(true);  // ����countdownText

        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString(); // ����Canvas�ϵĵ�����ʱ

            if (timer <= 3 && !isRecovering)
            {
                isRecovering = true; // �����ε���
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

        countdownText.text = ""; // ���Canvas�ϵĵ�����ʱ
        countdownText.gameObject.SetActive(false);  // ����countdownText
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
        isRecovering = false; // ����
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
