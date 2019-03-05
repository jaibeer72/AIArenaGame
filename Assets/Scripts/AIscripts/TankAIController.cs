using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TankAIController : MonoBehaviour
{

    AIChirectorManager aiChirectorScript;
    GameObject player;
    TankAIStates tankState;
    public float range;
    NavMeshAgent tankAgent; 

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

        // we can use reamaining distance for the distance calculation. 
        if (tankAgent.remainingDistance>tankAgent.stoppingDistance && !PlayerInputChirector.isAttacking)
        {
            tankState = TankAIStates.walking;
            tankAgent.SetDestination(player.transform.position); 
            aiChirectorScript.Move(tankAgent.desiredVelocity*0.5f, false, false, tankState); 
        }

        // Checks if tank is russing to the player if not then use the basic attack. 
        if(tankAgent.remainingDistance<tankAgent.stoppingDistance && tankState!= TankAIStates.rushAttack)
        {
            //Make state idel
            // basic attack
            aiChirectorScript.Move(tankAgent.desiredVelocity, false, false, tankState);
            tankState = TankAIStates.basicAttack;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < range && PlayerInputChirector.isAttacking)
        {
            tankState = TankAIStates.rushAttack;
            //Set destination Away from the player 
            //when AI reaches 
            //set destination to player position 
            //and run to him 
            
        }
    }

    Transform TargetManager(Transform playerPos, bool isGoingTowards)
    {
        //if the AI is going towards player
            // Then return a randome point away from the player
        // if is going towards the player 
            // return towards the player. 
        return this.transform;
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
