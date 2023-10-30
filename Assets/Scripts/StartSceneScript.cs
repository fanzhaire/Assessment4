using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartSceneScript : MonoBehaviour
{
    public Text HighScoreText; // ��Unity�༭�����Ϸ���Ӧ��Text����
    public Text BestTimeText; // ��Unity�༭�����Ϸ���Ӧ��Text����

    void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        HighScoreText.text = "High Score: " + highScore;
        BestTimeText.text = "Best Time: " + FormatTime(bestTime);
    }

    string FormatTime(float time)
    {

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 100) % 100); // ��ðٺ�����

        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }
}
