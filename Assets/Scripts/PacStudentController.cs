using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public float speed = 5.0f; 
    public Animator animator; 

    private Vector3 targetPosition;
    private bool isLerping;
    private string lastInput = "";
    private string currentInput = "";

    public ParticleSystem dustParticles;

    public AudioSource moveAudio;

    private bool hasMovedOnce = false;

    private Dictionary<string, Vector3> directions = new Dictionary<string, Vector3>
    {
        { "w", Vector3.up },
        { "a", Vector3.left },
        { "s", Vector3.down },
        { "d", Vector3.right }
    };

    private Dictionary<string, string> animationTriggers = new Dictionary<string, string>
    {
        { "w", "MoveUp" },
        { "a", "MoveLeft" },
        { "s", "MoveDown" },
        { "d", "MoveRight" }
    };

    void Start()
    {
        moveAudio.Stop();

        targetPosition = transform.position;
    }

    void Update()
    {
        
        GetInput();

        if (!hasMovedOnce) return;

        if (!isLerping)
        {
            TryMove(lastInput);

            if (!CanMoveTo(targetPosition))
            {
                TryMove(currentInput);
            }

            if (CanMoveTo(targetPosition))
            {
                StartLerping();
            }
        }
        else
        {
            ContinueLerping();
        }
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            lastInput = Input.inputString;
            TryMove(lastInput);

            if (CanMoveTo(targetPosition))
            {
                hasMovedOnce = true;
                StartLerping();
            }
        }
    }


    void TryMove(string directionKey)
    {
        if (directions.ContainsKey(directionKey))
        {
            targetPosition = transform.position + directions[directionKey];
            currentInput = directionKey;
        }
    }

    bool CanMoveTo(Vector3 targetPos)
    {
        // 这里需要你实现一个方法来检查目标位置是否可行走
        // 例如，检查LevelGenerator的levelMap，确保目标位置没有墙或者其他障碍物
        return true; // 假设总是可以移动
    }

    void StartLerping()
    {
        isLerping = true;
        PlayAnimation(currentInput);
        
        dustParticles.Play();

        if (!moveAudio.isPlaying)
        {
            moveAudio.Play();
        }

    }

    void ContinueLerping()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isLerping = false;
            StopAnimation();          
        }
    }

    void PlayAnimation(string directionKey)
    {
        StopAnimation();
        if (animationTriggers.ContainsKey(directionKey))
        {
            animator.SetTrigger(animationTriggers[directionKey]);
        }
    }

    void StopAnimation()
    {
        foreach (var trigger in animationTriggers.Values)
        {
            animator.ResetTrigger(trigger);
        }

        dustParticles.Stop();

        moveAudio.Stop();

    }
}
