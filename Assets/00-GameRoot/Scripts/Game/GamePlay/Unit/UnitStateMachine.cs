using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBaseState : BaseSate<BaseUnit>
{
    public override void EnterState(BaseUnit script) { }

    public override void Update(BaseUnit script) { }

}

public class UnitAttackState : UnitBaseState
{
    float timePassed=0;
    public override void EnterState(BaseUnit script)
    {
        char characterCode = script.characterID[1];
        if (characterCode == 'R'|| characterCode == 'M')
            script.transform.LookAt(script.target.transform);
        timePassed = 0;
    }

    public override void Update(BaseUnit script)
    {
        timePassed += Time.deltaTime;
        if (timePassed > script.coolDown)
        {
            script.Attack();
            timePassed = 0;
        }
        
    }
}
public class UnitHoverState : UnitBaseState
{
    public override void EnterState(BaseUnit script)
    {
        script.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public override void Update(BaseUnit script)
    {
        script.Drag(); //drag the unit
    }
}
public class UnitIdleState : UnitBaseState
{
    public override void EnterState(BaseUnit script)
    {
        script.transform.eulerAngles = script.rotation;
        script.transform.position = script.currentTile.transform.position; // go back to where you came from
        script.HideHighlightedTiles();
        script.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    public override void Update(BaseUnit script)
    {
        script.IdleUpdate();
    }
}
