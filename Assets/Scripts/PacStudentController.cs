using System.Collections;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Vector2[] localPathPoints = {
        new Vector2(0.32f, -0.32f), 
        new Vector2(3.84f, -0.32f),
        new Vector2(3.84f, -1.60f),
        new Vector2(2.88f, -1.60f),
        new Vector2(2.88f, -2.56f),
        new Vector2(3.84f, -2.56f),
        new Vector2(3.84f, -3.52f),
        new Vector2(2.88f, -3.52f),
        new Vector2(2.88f, -4.48f),
        new Vector2(1.92f, -4.48f),
        new Vector2(1.92f, -2.56f),
        new Vector2(0.32f, -2.56f),
        new Vector2(0.32f, -0.32f),     
     };


    private int currentPointIndex = 0;
    public float speed = 1f;
    private Animator animator;
    public GameObject manualLevelLayout; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePacStudent();
    }

    void MovePacStudent()
    {
        Vector2 globalTargetPoint = localPathPoints[currentPointIndex] + (Vector2)manualLevelLayout.transform.position;

        if (Vector2.Distance(transform.position, globalTargetPoint) < 0.01f)
        {
            currentPointIndex = (currentPointIndex + 1) % localPathPoints.Length;
        }

        Vector2 direction = (globalTargetPoint - (Vector2)transform.position);
        direction.y = -direction.y;  
        direction = direction.normalized;
        transform.position = Vector2.MoveTowards(transform.position, globalTargetPoint, speed * Time.deltaTime);

        UpdateAnimation(direction);
    }

    void UpdateAnimation(Vector2 direction)
    {
        Vector2 globalTargetPoint = localPathPoints[currentPointIndex] + (Vector2)manualLevelLayout.transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement
            if (globalTargetPoint.x > transform.position.x)
            {
                animator.Play("WalkRight");
            }
            else
            {
                animator.Play("WalkLeft");
            }
        }
        else
        {
            // Vertical movement
            if (globalTargetPoint.y < transform.position.y)
            {
                animator.Play("WalkDown");
            }
            else
            {
                animator.Play("WalkUp");
            }
        }
    }



}