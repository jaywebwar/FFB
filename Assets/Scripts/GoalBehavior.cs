using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    [SerializeField] GameObject gameBall;
    [SerializeField] GameManager gameManager;
    [SerializeField] bool isBlueTeam;

    void OnTriggerEnter(Collider other)
    {
        //Check if what has collided is the ball
        if(other.gameObject == gameBall.gameObject)
        {
            Debug.Log("Game ball has reached goal");
            gameBall.SetActive(false);
            gameManager.ScoreGoal(isBlueTeam);
        }
    }
}
