using UnityEngine;
using TMPro;  
using System.Collections;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI roundStartCountdown;  
    public GameObject pacStudent;    
    private void Start()
    {
        StartCoroutine(StartRoundCountdown());
    }
    void Update()
    {

    }

    IEnumerator StartRoundCountdown()
    {
        // 禁用PacStudent的移动
        pacStudent.GetComponent<PacStudentController>().enabled = false;

        roundStartCountdown.text = "3";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "2";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "1";
        yield return new WaitForSeconds(1);

        roundStartCountdown.text = "GO!";
        yield return new WaitForSeconds(1);

        // 启用PacStudent的移动
        pacStudent.GetComponent<PacStudentController>().enabled = true;

        // 启动游戏计时器
        pacStudent.GetComponent<PacStudentController>().StartGame();

        // 隐藏倒计时文本
        roundStartCountdown.gameObject.SetActive(false);
    }

}
