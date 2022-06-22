using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehavior : MonoBehaviour
{
    [SerializeField] GameObject gameBall;

    void OnTriggerEnter(Collider other)
    {
        //Check if what has collided is the ball
        if(other.gameObject == gameBall.gameObject)
        {
            Debug.Log("Game ball has reached goal");
        }
    }
}
