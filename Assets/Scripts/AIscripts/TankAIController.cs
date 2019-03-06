using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TankAIController : MonoBehaviour
{

    AIChirectorManager aiChirectorScript;
    GameObject player;
    TankAIStates tankState;
    public float awayRadius;
    NavMeshAgent tankAgent;
    bool isGoingTowardsPlayer = false;
    public Transform secondaryTarget;
    bool isAwayTargetSet = false;

    // Start is called before the first frame update
    void Start()
    {
        tankState = TankAIStates.idel;
        aiChirectorScript = GetComponent<AIChirectorManager>();
        tankAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        tankAgent.SetDestination(player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        TankAISateAIManagement();
    }

    void TankAISateAIManagement()
    {

        if (tankState != TankAIStates.rushAttack && tankState != TankAIStates.stunned && tankState != TankAIStates.idel)
        {
            // we can use reamaining distance for the distance calculation. 
            if (tankAgent.remainingDistance > tankAgent.stoppingDistance)
            {
                tankState = TankAIStates.walking;
                tankAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(tankAgent.desiredVelocity * 0.5f, false, false, tankState);
            }

            // Checks if tank is russing to the player if not then use the basic attack. 
            if (tankAgent.remainingDistance < tankAgent.stoppingDistance && tankState != TankAIStates.rushAttack)
            {
                //Make state idel
                // basic attack
                StartCoroutine(RushAttackTime());
                tankState = TankAIStates.basicAttack;
                tankAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(tankAgent.desiredVelocity, false, true, tankState);

            }
        }
        else if (tankState == TankAIStates.rushAttack && tankState != TankAIStates.stunned)
        {
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
                tankState = TankAIStates.basicAttack;
                isGoingTowardsPlayer = false;


            }
        }
        else if ( tankState == TankAIStates.stunned || tankState == TankAIStates.idel)
        {
            StopCoroutine(RushAttackTime());
            StartCoroutine(StunTime());
            tankAgent.SetDestination(transform.position);
            aiChirectorScript.Move(Vector3.zero, false, true, tankState);
        }
        //if (Vector3.Distance(transform.position, player.transform.position) < range && PlayerInputChirector.isAttacking)
        //{

            //    //Set destination Away from the player 
            //    //when AI reaches 
            //    //set destination to player position 
            //    //and run to him 

            //}
    }

    IEnumerator RushAttackTime()
    {
        yield return new WaitForSeconds(4);
        //Debug.Log("called");
        tankState = TankAIStates.rushAttack;
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
            tankState = TankAIStates.rushAttack;
            tankAgent.speed = 2;
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
            tankState = TankAIStates.stunned; 
        }
    }

    IEnumerator StunTime()
    {
        tankState = TankAIStates.stunned;
        
        yield return new WaitForSeconds(3);
        Debug.Log(tankState);
        tankState = TankAIStates.idel;
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
