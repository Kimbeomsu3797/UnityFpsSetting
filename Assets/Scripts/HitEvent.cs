using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    public EnemyFSM efsm;
    // Start is called before the first frame update
    void Start()
    {
        efsm = GameObject.Find("Enemy").GetComponent<EnemyFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerHit()
    {
        efsm.AttackAction();
    }
}
