using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Stats enemyStats;

    [Tooltip("The transform that will lock onto the player once the enemy has spotted them.")]
    public Transform sight;
    
    [Tooltip("Blue explosion particles")]
    public GameObject enemyExplosionParticles;

    public bool slipping = false;
   
    public float facing;

    public GameObject player;
    public struct Stats
    {
        [Tooltip("How fast the enemy runs after the player (only when idle is false).")]
        public float chaseSpeed;

        [Tooltip("Whether the enemy is idle or not. Once the player is within distance, idle will turn false and the enemy will chase the player.")]
        public bool idle;

        [Tooltip("How close the enemy needs to be to explode")]
        public float explodeDust;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStats.idle == false)
        {
            //Chase the player
            sight.position = new Vector3(player.transform.position.x, transform.position.y,
                player.transform.position.z);
            transform.LookAt(sight);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
                Time.deltaTime * enemyStats.chaseSpeed);

            //Explode if we get within the enemyStats.explodeDist
            if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.explodeDust)
            {
                StartCoroutine("Explode");
                enemyStats.idle = true;
            }
        }
        // stops enemy from following player up the inaccessible slopes
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
        }
    }

    private void OnCollisionEnter(Collision other)
   {
    if (other.gameObject.layer == 9)
    {
        slipping = true;
    }
    else
    {
        slipping = false;
    }
   }
   
   private void OnTriggerEnter(Collider other)
   {
    //start chasing if the player gets close enough
    if (other.gameObject.tag == "Player")
    {
        player = other.gameObject;
        enemyStats.idle = false;
    }
   }

   private void OnTriggerExit(Collider other)
   {
    //stop chasing if the player gets far enough away
    if (other.gameObject.tag == "Player")
    {
        enemyStats.idle = true;      
    }
   }

   private IEnumerator Explode()
   {
    GameObject particles = Instantiate(enemyExplosionParticles, transform.position, new Quaternion());
    yield return new WaitForSeconds(0.2f);
    Destroy(transform.parent.gameObject);
   }
}
