using UnityEngine;
using TMPro;  
using System.Collections;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI roundStartCountdown;  // 使用TextMeshProUGUI替代原始的Text
    public GameObject pacStudent;     // 拖拽PacStudent对象

    public Text gameTimerText;  // 游戏计时器文本组件的引用
    private float elapsedTime = 0f; // 流逝的时间
    private bool isGameStarted = false;
    private void Start()
    {
        StartCoroutine(StartRoundCountdown());
        gameTimerText.text = "00:00:00";
    }
    void Update()
    {
        if (isGameStarted)
        {
            elapsedTime += Time.deltaTime;
            int minutes = (int)elapsedTime / 60;
            int seconds = (int)elapsedTime % 60;
            int milliseconds = (int)(elapsedTime * 100) % 100;

            gameTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
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

        StartGame();

        // 隐藏倒计时文本
        roundStartCountdown.gameObject.SetActive(false);

        // 启用PacStudent的移动
        pacStudent.GetComponent<PacStudentController>().enabled = true;
    }
}
