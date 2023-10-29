//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PlayerMovement : MonoBehaviour
//{
//    public float speed = 5.0f;

//    private Vector3 moveDirection;
//    private bool canMove = false;
    
//    void Update()
//    {
        

//        if (canMove && GetInput())
//        {
//            Move();
//        }
        
        
//    }

//    bool GetInput()
//    {
//        //
//        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
//        {
//            SetDirection(Input.inputString);

//            return true;
//        }

//        return false;
//    }

//    void Move()
//    {
//        // ¸üÐÂÎ»ÖÃ
//        transform.position += moveDirection * speed * Time.deltaTime;
//        // PLAY FX
//        PlayFootFX();
//    }

//    void SetDirection(string dir)
//    {
//        switch (dir)
//        {
//            case "W":
//                moveDirection = Vector3.up; 
//                break;

//        }
//    }

//    void PlayAnimation()
//    {

//    }

//    void PlayFootFX()
//    {
//        //SFX

//        //VFX
//    }

//    private void OnCollisionEnter2D(Collision collision)
//    {
//        canMove = false;
//    }
    
//    private void OnCollisionExit2D(Collision2D collision)
//    {
        
//    }
//}
