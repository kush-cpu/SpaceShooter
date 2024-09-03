using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSceneForCamera : MonoBehaviour
{
    public Transform toFollow;
    private float initialOffset;
    // Start is called before the first frame update
    void Start()
    {
        initialOffset = toFollow.position.x - transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(toFollow != null)
        {
            Vector3 positionToFollow = toFollow.position;
            Vector3 finalPosition = new Vector3(positionToFollow.x - initialOffset, positionToFollow.y, positionToFollow.z);
            transform.position = finalPosition;
        }
    }
}
