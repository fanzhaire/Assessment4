using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartSceneScript : MonoBehaviour
{
    public Text HighScoreText; // 在Unity编辑器中拖放相应的Text对象
    public Text BestTimeText; // 在Unity编辑器中拖放相应的Text对象

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
        int milliseconds = (int)((time * 100) % 100); // 获得百毫秒数

        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }
}
