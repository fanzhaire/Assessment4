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

        // ����PacStudent���ƶ�
        pacStudent.GetComponent<PacStudentController>().enabled = true;

        // ������Ϸ��ʱ��
        pacStudent.GetComponent<PacStudentController>().StartGame();

        // ���ص���ʱ�ı�
        roundStartCountdown.gameObject.SetActive(false);
    }

}
