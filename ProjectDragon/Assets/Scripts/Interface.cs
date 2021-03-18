//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interfae of PersonalSpecification
public interface PersonalSpecificational
{
    int HPChanged(int ATK);
    int ATKChanger(int attackDamage);
    float ATKSpeedChanger(float AttackSpeed);
    float MoveSpeedChanger(float MoveSpeed);

    float ATTACKSPEED { get; set; }
    int ATTACKDAMAGE { get; set; }
    float MoveSpeed { get; set; }
    int HP { get; set; }
}