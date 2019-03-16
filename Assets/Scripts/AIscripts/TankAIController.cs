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

    // Start is called before the first frame update
    void Start()
    {
        tankState = AIStates.idel;
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
                tankAgent.SetDestination(transform.position);
                aiChirectorScript.Move(Vector3.zero, false, true, tankState);
                break;

            case AIStates.stunned:
                StopCoroutine(RushAttackTime());
                StartCoroutine(StunTime());
                tankAgent.SetDestination(transform.position);
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
                if (isAwayTargetSet && !isGoingTowardsPlayer)
                {
                    tankAgent.SetDestination(secondaryTarget.position);
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);
                }
                if (isAwayTargetSet && tankAgent.remainingDistance < tankAgent.stoppingDistance && !isGoingTowardsPlayer)
                {
                    tankAgent.SetDestination(player.transform.position + transform.forward * 3);
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);
                    isGoingTowardsPlayer = true;
                    isAwayTargetSet = false;
                }
                if (isGoingTowardsPlayer && tankAgent.remainingDistance > tankAgent.stoppingDistance)
                {
                    tankAgent.SetDestination(player.transform.position);
                    GetComponent<AIChirectorManager>().hitStrength = 50f;
                    aiChirectorScript.Move(tankAgent.desiredVelocity * 2, false, false, tankState);

                }
                if (isGoingTowardsPlayer && tankAgent.remainingDistance < tankAgent.stoppingDistance)
                {
                    //tankAgent.SetDestination(player.transform.position + transform.forward * 1);
                    //StartCoroutine(RushAttackTime());
                    GetComponent<AIChirectorManager>().hitStrength = 10f;
                    tankState = AIStates.basicAttack;
                    isGoingTowardsPlayer = false;


                }
                break;
            default:
                Debug.Log("TankState Error");
                break;
        }

    }

    IEnumerator RushAttackTime()
    {
        Debug.Log("called");
        yield return new WaitForSeconds(4);  
        tankState = AIStates.rushAttack;
        TargetManager(player.transform, isGoingTowardsPlayer);
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
        }
    }

    IEnumerator StunTime()
    {
        tankState = AIStates.stunned;

        yield return new WaitForSeconds(5);
        Debug.Log(tankState);
        tankState = AIStates.idel;
        yield return new WaitForSeconds(1);
        tankState = AIStates.walking;
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
