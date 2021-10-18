using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Saran Krishnaraja
// Got code from https://www.youtube.com/watch?v=jUdx_Nj4Xk0

public class Guard : MonoBehaviour {

    public bool isOn = false;

    public float speed = 5f;
    public float waitTime = .3f;
    public float turnSpeed = 90f;
    public float timeToSpotPlayer = .5f;

    public Light spotLight;
    public float viewDistance;
    public LayerMask viewMask;

    float viewAngle;
    float playerVisibleTimer;

    public Transform pathHolder;

    [SerializeField]
    Player playerCaught;
    //Vector3[] waypoints;

    [SerializeField]
    int guardNumber;

    [SerializeField]
    Warden warden;

    [SerializeField]
    Animator mAnimator;

    private void Start()
    {
        viewAngle = spotLight.spotAngle;

        // Get the waypoints positions for where the guard will patrol on
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i <waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        CheckIsOn();
        StartCoroutine(FollowPath(waypoints));
    }

    private void FixedUpdate()
    {
        if (CheckIsOn())
        {
            mAnimator.SetBool("IsWalking", true);
            mAnimator.SetFloat("Speed", speed);
            if (CanSeePlayer())
            {
                playerCaught.PlayerGotCaught();
            }
        }
        else
        {
            mAnimator.SetBool("IsWalking", false);
        }
    }

    bool CheckIsOn()
    {
        return isOn;
    }

    bool CanSeePlayer()
    {
        List<NetworkPlayer> playersInGame = GameManager.GetAllPlayers();    // Get all the players

        foreach (NetworkPlayer player in playersInGame) // Check if the guard is infront of every player in the game
        {
            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

                if (angleBetweenGuardAndPlayer < viewAngle)
                {
                    if (!Physics.Linecast(transform.position, player.transform.position, viewMask))
                    {
                        mAnimator.SetBool("CaughtPlayer", true);

                        playerCaught = player.GetComponent<Player>();
                        return true;
                    }
                    else
                    {
                        mAnimator.SetBool("CaughtPlayer", false);
                    }
                }
            }
        }
        return false;
    }

    // Guard follow the path
    IEnumerator FollowPath(Vector3[] _waypoints)
    {
        transform.position = _waypoints[0];

        int targetWaypointIndex = 0;
        Vector3 targetWaypoint = _waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            while (isOn)
            {
                //Debug.Log("Guard moving");

                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
                if (transform.position == targetWaypoint)
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % _waypoints.Length;
                    targetWaypoint = _waypoints[targetWaypointIndex];
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(TurnToFace(targetWaypoint));
                }
                yield return null;
            }
            yield return null;
        }
    }

    // Guard rotates to next path
    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }

    }

    // Draws the sphere and line for where the Guard will run on
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        // Draw view distance of guard
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

    }


}
