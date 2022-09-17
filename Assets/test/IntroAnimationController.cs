using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimationController : MonoBehaviour
{

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Activate()
    {
        anim.SetBool("ActiveSimulation", true);
    } 
    
    public IEnumerator Deactivate()
    {
        anim.SetBool("ActiveSimulation", false);
        yield return new WaitForSeconds(1.7f);
        gameObject.SetActive(false);
    }
  
}
