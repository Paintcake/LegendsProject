using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorEnemy : MonoBehaviour
{
    public Stats enemyStats;

    [Tooltip("The transform to which the enemy will pace back and forth to.")]
    public Transform[] patrolPoints;
    
    public int currentPatrolPoint = 0;

    public float facing;
    
    public Rigidbody rb;

    private GameObject player;

    public FollowPlayer followPlayer;

    /// <summary>
    /// Contains tunable parameters to tweak the enemy's movement and behavior.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        [Header("Enemy Settings")]
        [Tooltip("How fast the enemy walks (only when idle is true).")]
        public float walkSpeed;

        [Tooltip("How fast the enemy turns in circles as they're walking (only when idle is true).")]
        public float rotateSpeed;
        
        [Tooltip("Whether the enemy is idle or not. Once the player is within distance, idle will turn false and the enemy will chase the player.")]
        public bool idle;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // changes the enemy's behavior: pacing in circles or chasing the player
        if (enemyStats.idle == true)
        {
            //Patrol Logic
                Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
                transform.position = Vector3.MoveTowards(transform.position, moveToPoint, enemyStats.walkSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint > patrolPoints.Length - 1)
                    {
                        currentPatrolPoint = 0;
                    }
                }
        }
    }
}