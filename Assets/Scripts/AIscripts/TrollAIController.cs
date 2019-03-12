using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TrollAIController : MonoBehaviour
{

    AIChirectorManager aiChirectorScript;
    GameObject player;
    TankAIStates trollState;
    NavMeshAgent trollAgent;
    // Start is called before the first frame update
    void Start()
    {
        trollState = TankAIStates.idel;
        aiChirectorScript = GetComponent<AIChirectorManager>();
        trollAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        trollAgent.SetDestination(player.transform.position);
        trollState = TankAIStates.walking;
    }

    // Update is called once per frame
    void Update()
    {
        TrollAIStateAIManagement();
    }

    void TrollAIStateAIManagement()
    {
        switch (trollState)
        {
            case TankAIStates.basicAttack:
                trollAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(trollAgent.desiredVelocity, false, true, trollState);
                if (trollAgent.remainingDistance > trollAgent.stoppingDistance)
                {
                    trollState = TankAIStates.walking;
                }
                break;
            case TankAIStates.idel:
                trollAgent.SetDestination(transform.position);
                aiChirectorScript.Move(Vector3.zero, false, true, trollState);
                break;
            case TankAIStates.stunned:
                //will not happen but resuing code is fine.
                Debug.Log("TrollState Error Stunned");
                break;
            case TankAIStates.walking:
                trollAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(trollAgent.desiredVelocity, false, false, trollState);
                if (trollAgent.remainingDistance < trollAgent.stoppingDistance)
                {
                    trollState = TankAIStates.basicAttack;
                }
                break;
            case TankAIStates.rushAttack:
                //will not happen but it is useless to create multyple enums
                Debug.Log("TrollState Error RushAttack");
                break;
            default:
                Debug.Log("TrollState Error default");
                break;
        }
    }
}
