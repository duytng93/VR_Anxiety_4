using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChildMovementController : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> mySpots = new List<Transform>();
    public void GoToRandomSpot()
    {
        agent.destination = mySpots[Random.Range(0, mySpots.Count - 1)].position;
    }
}
