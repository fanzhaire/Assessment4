using UnityEngine;
using TMPro;  
using System.Collections;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI roundStartCountdown;  // ʹ��TextMeshProUGUI���ԭʼ��Text
    public GameObject pacStudent;     // ��קPacStudent����

    public Text gameTimerText;  // ��Ϸ��ʱ���ı����������
    private float elapsedTime = 0f; // ���ŵ�ʱ��
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
        // ����PacStudent���ƶ�
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

        // ���ص���ʱ�ı�
        roundStartCountdown.gameObject.SetActive(false);

        // ����PacStudent���ƶ�
        pacStudent.GetComponent<PacStudentController>().enabled = true;
    }
}
