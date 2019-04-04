using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TankAIController : MonoBehaviour
{

    AIChirectorManager aiChirectorScript;
    GameObject player;
    public AIStates tankState;
    public float awayRadius;
    NavMeshAgent tankAgent;
    bool isGoingTowardsPlayer = false;
    public Transform secondaryTarget;
    bool isAwayTargetSet = false;
    LayerMask mask; 

    // Start is called before the first frame update
    void Start()
    {
        aiChirectorScript = GetComponent<AIChirectorManager>();
        tankAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        tankAgent.SetDestination(player.transform.position);
        tankState = AIStates.walking;

    }

    // Update is called once per frame
    void Update()
    {
        TankAISateAIManagement();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawSphere(secondaryTarget.position, 5f); 
    }

    void TankAISateAIManagement()
    {
        switch (tankState)
        {
            case AIStates.basicAttack:
                tankAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(tankAgent.desiredVelocity, false, true, tankState);
                StartCoroutine(RushAttackTime());
                if (tankAgent.remainingDistance > tankAgent.stoppingDistance)
                {
                    tankState = AIStates.walking;
                }
                break;

            case AIStates.idel:
                tankAgent.isStopped = true;
                aiChirectorScript.Move(Vector3.zero, false, true, tankState);
                
                break;

            case AIStates.stunned:
                
                tankAgent.isStopped = true;
                aiChirectorScript.Move(Vector3.zero, false, true, tankState);
                break;

            case AIStates.walking:
                tankAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(tankAgent.desiredVelocity * 0.5f, false, false, tankState);
                if (tankAgent.remainingDistance < tankAgent.stoppingDistance)
                {
                    tankState = AIStates.basicAttack;
                }

                break;

            case AIStates.rushAttack:
                StopCoroutine(RushAttackTime());
                if (isAwayTargetSet && !isGoingTowardsPlayer && tankAgent.remainingDistance > tankAgent.stoppingDistance)
                {
                    tankAgent.SetDestination(secondaryTarget.position);
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);
                }
                else if (isAwayTargetSet && tankAgent.remainingDistance < tankAgent.stoppingDistance && !isGoingTowardsPlayer)
                {
                    tankAgent.SetDestination(player.transform.position);
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);
                    isGoingTowardsPlayer = true;
                    isAwayTargetSet = false;
                }
                else if(isGoingTowardsPlayer && !isAwayTargetSet/*&& tankAgent.remainingDistance > tankAgent.stoppingDistance*/)
                {
                    tankAgent.SetDestination(player.transform.position);
                    GetComponent<AIChirectorManager>().hitStrength = 50f;
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);

                }
                else if(isGoingTowardsPlayer && tankAgent.remainingDistance < tankAgent.stoppingDistance)
                {
                    //tankAgent.SetDestination(player.transform.position + transform.forward * 1);
                    //StartCoroutine(RushAttackTime());
                    GetComponent<AIChirectorManager>().hitStrength = 10f;
                    tankState = AIStates.basicAttack;
                    isGoingTowardsPlayer = false;


                }
                break;

            case AIStates.dead:
                tankAgent.isStopped = true;
                aiChirectorScript.Move(Vector3.zero, false, false, tankState); 
                break; 
            default:
                Debug.Log("TankState Error");
                break;
        }

    }

    IEnumerator RushAttackTime()
    {
        Debug.Log("called");
        yield return new WaitForSeconds(10);  
        tankState = AIStates.rushAttack;
        TargetManager(player.transform, false); 
        Debug.Log(tankState);
        //tankAgent.isStopped = false;
    }

    void TargetManager(Transform playerPos, bool isGoingTowards)
    {
        if (!isGoingTowards && !isAwayTargetSet)
        {
            isGoingTowardsPlayer = false;
            float randAngle = Random.Range(0.0f, 366.0f);
            secondaryTarget.position = new Vector3(playerPos.position.x + awayRadius * Mathf.Cos(randAngle), playerPos.position.y, playerPos.position.z + awayRadius * Mathf.Sin(randAngle));
            isAwayTargetSet = true;
            tankState = AIStates.rushAttack;
            NavMeshHit navMeshHit;
            NavMesh.SamplePosition(secondaryTarget.position, out navMeshHit, awayRadius, mask);
            secondaryTarget.position = navMeshHit.position; 
            //tankAgent.speed = 2;
        }
        else
        {
        }
        //if the AI is going towards player
        // Then return a randome point away from the player
        // if is going towards the player 
        // return towards the player. 
    }

    public void isAttacked(AttackType playerAttackType)
    {
        if (playerAttackType == AttackType.magic)
        {
            tankState = AIStates.stunned;
            StopCoroutine(RushAttackTime()); 
            StartCoroutine(StunTime());
            tankAgent.isStopped=true; 
        }
    }

    public IEnumerator StunTime()
    {
        tankState = AIStates.stunned;
        Debug.Log(tankState);
        yield return new WaitForSeconds(10);
        tankState = AIStates.stunned;
        Debug.Log(tankState);
        tankState = AIStates.idel;
        
        yield return new WaitForSeconds(4);
        tankState = AIStates.walking;
        tankAgent.isStopped = false;
        StartCoroutine(RushAttackTime()); 
        StopCoroutine(StunTime());
    }
}


//Psudocode 
// Spwan
// Idel taunt 
// if(player > range && player is !attacking) 
// walk to player and look at him 
// if(player<range)
// simple AIstate
//if(player < range and player is attacking) 
// go back away from range 
// target player 
// Rush AIstate. 
// DamageManager ( attacktype , damageAmount) 
//if(health>0 && Tank AI is !Stunned) 
// dont take Damage
//if (Health  >0 && AI is Stunned) 
// Take Damage 
//if (Health > 0 && attackType == magic) 
// Tank.State == stunned
// IEnumerator(Stay stunned for 3 seconds) 
// and go back to idel 
// if(Health < 0)
// Death animation 
