using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;

    int timerMinute = 5;
    int timerSecond = 0;

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
            timerText.text = timerMinute.ToString()+":"+timerSecond.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
