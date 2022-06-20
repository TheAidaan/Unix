using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, string characterID)
    {
        maxHealth = 18;
        coolDown = 3f;
        damage = 2;

        base.Setup(TeamColor, unitColor, characterID);


        movement = new Vector3Int(0, 7, 0);
        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Models/Ranged");
        gameObject.AddComponent<BoxCollider>();
    }

    
    public override BaseUnit CheckForEnemy()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 25f , Vector3.down);
        foreach (RaycastHit Hit in hit)
        {
            if (Hit.transform.gameObject.layer != transform.gameObject.layer)
            {
                BaseUnit Target = Hit.transform.gameObject.GetComponent<BaseUnit>();
                if (Target != null)
                {
                    target = Target;

                    if (!GameManager.aiEvaluationInProgress)
                    {
                        targetPos = Target.transform.position;
                        TransitionToState(attackState);
                        break;
                    }

                    return target;                    
                }
            }
        }

        return null;
    }
}
