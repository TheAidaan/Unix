using UnityEngine;

public class MeleeUnit : BaseUnit
{
    public override void Setup(Color TeamColor, Color32 unitColor, string  CharacterID)
    {
        maxHealth = 20;
        coolDown = 3f;
        damage = 2;

        base.Setup(TeamColor, unitColor, CharacterID);

        movement = new Vector3Int(1, 1, 1);

        GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Models/Melee");
        gameObject.AddComponent<BoxCollider>();
    }

    public override BaseUnit CheckForEnemy()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 15f, Vector3.down);

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
