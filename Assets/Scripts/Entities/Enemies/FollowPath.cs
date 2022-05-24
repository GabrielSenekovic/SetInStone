using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

[RequireComponent(typeof(PathFollower))]
public class FollowPath : MonoBehaviour
{
    public GameObject pathPrefab;
    [System.NonSerialized] public PathFollower pathFollow;
    [System.NonSerialized] public PathCreator pathCreator;
    public float turnAroundSpeed = 10;
    void Start()
    {
        if(!pathPrefab){return;}
        pathFollow = GetComponent<PathFollower>();
        GameObject path = GameObject.Instantiate(pathPrefab, transform.position, Quaternion.identity);
        pathFollow.pathCreator = path.GetComponent<PathCreator>();
        pathCreator = pathFollow.pathCreator;
    }

    // Update is called once per frame
    void Update()
    {
        if(!pathCreator){return;}
        Quaternion rotQ = pathCreator.path.GetRotationAtDistance(pathFollow.distanceTravelled, pathFollow.endOfPathInstruction);
        Vector3 rotE = rotQ.eulerAngles;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotE.y, 0), Time.deltaTime * turnAroundSpeed);
    }
}
