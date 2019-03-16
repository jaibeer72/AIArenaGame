using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class TrollAIController : MonoBehaviour
{

    AIChirectorManager aiChirectorScript;
    GameObject player;
    AIStates trollState;
    NavMeshAgent trollAgent;
    // Start is called before the first frame update
    void Start()
    {
        trollState = AIStates.idel;
        aiChirectorScript = GetComponent<AIChirectorManager>();
        trollAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        trollAgent.SetDestination(player.transform.position);
        trollState = AIStates.walking;
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
            case AIStates.basicAttack:
                trollAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(trollAgent.desiredVelocity, false, true, trollState);
                if (trollAgent.remainingDistance > trollAgent.stoppingDistance)
                {
                    trollState = AIStates.walking;
                }
                break;
            case AIStates.idel:
                trollAgent.SetDestination(transform.position);
                aiChirectorScript.Move(Vector3.zero, false, true, trollState);
               
                break;
            case AIStates.stunned:
                //will not happen but resuing code is fine.
                trollAgent.SetDestination(transform.position);
                aiChirectorScript.Move(Vector3.zero, false, true, trollState);
                StartCoroutine(StunTime());
                //trollState = AIStates.walking; 
                break;
            case AIStates.walking:
                trollAgent.SetDestination(player.transform.position);
                aiChirectorScript.Move(trollAgent.desiredVelocity, false, false, trollState);
                if (trollAgent.remainingDistance < trollAgent.stoppingDistance)
                {
                    trollState = AIStates.basicAttack;
                }
                break;
            case AIStates.rushAttack:
                //will not happen but it is useless to create multyple enums
                Debug.Log("TrollState Error RushAttack");
                break;
            default:
                Debug.Log("TrollState Error default");
                break;
        }
    }
    public void IsAttacked()
    {
            trollState = AIStates.stunned;
    }
    IEnumerator StunTime()
    {
        trollState = AIStates.stunned;
        yield return new WaitForSeconds(1);
        //Debug.Log(tankState);
        trollState = AIStates.idel;
        StopCoroutine(StunTime());
    }
}
