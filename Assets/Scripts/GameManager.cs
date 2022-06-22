using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI blueScoreText;
    [SerializeField] TextMeshProUGUI redScoreText;

    int timerMinute = 5;
    int timerSecond = 0;
    int blueScore = 0;
    int redScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunGameTimer());
    }

    IEnumerator RunGameTimer()
    {
        while(timerMinute > 0 || timerSecond > 0)
        {
            yield return new WaitForSeconds(1);
            if (timerSecond == 0)
            {
                timerMinute--;
                timerSecond = 59;
            }
            else
            {
                timerSecond--;
            }
            if(timerSecond < 10)
            {
                timerText.text = timerMinute.ToString() + ":0" + timerSecond.ToString();
            }
            else
            {
                timerText.text = timerMinute.ToString() + ":" + timerSecond.ToString();
            }
        }
    }

    internal void ScoreGoal(bool isBlueTeam)
    {
        if(isBlueTeam)
        {
            redScore++;
            redScoreText.text = redScore.ToString();
        }
        else
        {
            blueScore++;
            blueScoreText.text = blueScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
