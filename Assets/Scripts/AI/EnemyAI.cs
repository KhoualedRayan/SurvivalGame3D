using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("REFERENCES")]
    
    [SerializeField]
    private Transform player;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [Header("STATS")]

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float chaseSpeed;

    [SerializeField]
    private float detectionRadius;
    
    [SerializeField]
    private float attackRadius;
    
    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private float damageDealt;

    [SerializeField]
    private float rotationSpeed;

    [Header("WANDERING PARAMETERS")]

    [SerializeField]
    private float wanderingWaitTimeMin;

    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField]
    private float wanderingDistanceMin;

    [SerializeField]
    private float wanderingDistanceMax;

    /* Variables privées */
    private bool awaitingDestination;

    private bool hasDestination;

    private bool isAttacking;

    // Update is called once per frame
    void Update()
    {
        //Si le joueur est dans la range d'aggro de l'ennemi, alors il le poursuit
        if(Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            agent.speed = chaseSpeed;

            //On tourne l'ennemi vers le joueur
            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation,rot, rotationSpeed * Time.deltaTime);

            //Si l'ennemi n'attaque pas
            if(!isAttacking)
            {
                if(Vector3.Distance(player.transform.position, transform.position) < attackRadius)
                {
                    //Attaque le joueur s'il est en range d'attaque
                    StartCoroutine(AttackPlayer()); 
                }
                else
                {
                    //Sinon on le poursuit
                    agent.SetDestination(player.position);
                }
            }
        }
        else 
        {
            agent.speed = walkSpeed;

            //Génère une nouvelle destination
            if(agent.remainingDistance < 0.75f && !hasDestination)
            {
                
                StartCoroutine(GetNewDestination());
            }
        }
        animator.SetFloat("Speed",agent.velocity.magnitude);
    }
    private IEnumerator GetNewDestination()
    {
        hasDestination = true;
        //Attend un temps random entre le min et le max de repos avant une nouvelle destination
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin,wanderingDistanceMax) * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        hasDestination = false;
    }

    private IEnumerator AttackPlayer()
    {
        //On stop les mouvements de l'ennemi et on le met en attaque
        isAttacking = true;
        agent.isStopped = true;

        //On lance l'animation d'attaque
        animator.SetTrigger("Attack");

        //On enlève de la vie au joueur
        playerStats.TakeDamage(damageDealt);

        //On attend que l'animation se joue avant de continuer le script
        yield return new WaitForSeconds(attackDelay);

        //On restaure les mouvements de l'ennemi et il n'est plus en attaque
        agent.isStopped = false;
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        //Sphère de la range d'aggro
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        //Sphère de la range d'attaque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
