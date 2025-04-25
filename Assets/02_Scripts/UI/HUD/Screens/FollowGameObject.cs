using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject _objectToFollow;

    private void OnEnable()
    {
        followMethod(this.gameObject, _objectToFollow);
    }

    void followMethod(GameObject follower, GameObject followed)
    {

        follower.transform.position = followed.transform.position;
    }
}
