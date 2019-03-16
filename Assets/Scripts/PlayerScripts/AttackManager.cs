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


public enum AIStates
{
    basicAttack,
    idel,
    stunned,    
    walking,
    rushAttack
    
};
