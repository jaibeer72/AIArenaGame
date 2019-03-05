using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    
}

public enum AttackType
{
    basic, 
    stunn, 
    magic
};


public enum TankAIStates
{
    idel,
    walking,
    basicAttack,
    rushAttack,
    stunned
};
