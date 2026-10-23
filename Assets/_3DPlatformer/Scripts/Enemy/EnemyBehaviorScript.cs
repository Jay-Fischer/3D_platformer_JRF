using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Wander, Pursue, Return, Attack, Recovery }
    public EnemyState currentState = EnemyState.Wander;


    //bools and Condition Checking
    private bool recovering = false;
    private bool attacking = false;
    private bool isHurt = false;


    //Return Point Variables
    public Vector3 startLocation;
    public float returnDistance = 0.5f;


    //Navigation
    public NavMeshAgent nav;


    // Force and Attacking
    [SerializeField] Rigidbody rb;
    public float attackDistance = 2f;
    [SerializeField] public Transform targetPlayer;
    [SerializeField] public GameObject attackTrigger;
    public float attackForce = 5;

    [Header("Wander Settings")]
    public float wanderRadius = 3f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;
    private bool isWaiting = false;


    Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        startLocation = transform.position;
    }

    private void Update()
    {
    
        switch (currentState)
        {
            case EnemyState.Wander:
                if(!isHurt)
                {
                    nav.speed = 1;
                    Wander();
                }
                break;
            case EnemyState.Pursue:
                if (!isHurt)
                {
                    nav.speed = 2;
                    Pursue();
                }                
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Attack:
                if (!isHurt)
                {
                    if (!attacking)
                    {
                        StartCoroutine(Attack());
                    }
                }                
                break;
            case EnemyState.Recovery:
                if (!recovering) {
                    StartCoroutine(StartRecover());
                }
                break;
        }
    }

    public void SwitchToPursue(){
        anim.SetBool("Walking", false);
        currentState = EnemyState.Pursue;
    }

    public void SwitchToWander(){
        currentState = EnemyState.Wander;
        anim.SetBool("Running", false);
    }

    public void SwitchToReturn(){
        nav.SetDestination(startLocation);
        currentState = EnemyState.Return;
    }

    public void Wander()
    {
        if (!nav.pathPending && nav.remainingDistance <= returnDistance)
        {
            anim.SetBool("Walking", false);
            if (!isWaiting)
            {
                StartCoroutine(StartWander());
            }
        }
    }
    public void Pursue()
    {
        if (nav.enabled != false){
            anim.SetBool("Running", true);
            nav.SetDestination(targetPlayer.position);
            //Distance Calculation
            Vector3 positionOffset = targetPlayer.position - transform.position;
            float distanceToPlayer = positionOffset.magnitude;

            //Angle Calculation
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

            if (dotProduct > 0.9f && distanceToPlayer <= (attackDistance)){
                anim.SetBool("Running", false);
                currentState = EnemyState.Attack;
            }
        }
    }
    public void Return(){
    if (nav.enabled != false){
            Vector3 positionOffset = startLocation - transform.position;
            float distanceToPoint = positionOffset.magnitude;
            if (distanceToPoint <= (returnDistance * returnDistance))
                SwitchToWander();
        }        
    }
    IEnumerator Attack()
    {
        attacking = true;        
        nav.enabled = false;
        attackTrigger.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger("Attack");
        rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);
        currentState = EnemyState.Recovery;
        attacking = false;
    }
    IEnumerator StartRecover(){
        attackTrigger.SetActive(false);
        recovering = true;
        yield return new WaitForSeconds(1.25f);
        recovering = false;
        nav.enabled = true;
        currentState = EnemyState.Pursue;
    }

    IEnumerator StartWander()
    {
        
        isWaiting = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        anim.SetBool("Walking", true);

        Vector3 randomDestination = GetWanderPoint(startLocation, wanderRadius);
        nav.SetDestination(randomDestination);

        isWaiting = false;
    }

    Vector3 GetWanderPoint(Vector3 centerPoint, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += centerPoint;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            return hit.position;
        }

        return transform.position;
    }
    public bool ToggleHurt(bool hurt){
        return isHurt = hurt;
    }

}
