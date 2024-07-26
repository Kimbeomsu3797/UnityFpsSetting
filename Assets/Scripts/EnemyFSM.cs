using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die,
    }
    
    EnemyState m_State;

    public float findDistance = 8f;

    public float attackDistance = 2f;

    public float moveSpeed = 5f;

    CharacterController cc;

    Transform player;

    float currentTime = 0;

    float attackDelay = 2f;
    
    Vector3 originPos;

    public float moveDistance = 20f;

    public int attackPower = 3;

    public int hp = 15;
    public int maxHp = 15;
    public Slider EnemyHpslider;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        maxHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            /*case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;*/
        }
        EnemyHpslider.value = (float)hp / (float)maxHp;
        
    }

    public void Idle()
    {

        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Idle -> Move");
        }
    }
    void Move()
    {
        //cc.Move(pos * moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(player.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("상태 전환 : Move -> Return");
        }
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");
            currentTime = attackDelay;
        }
        
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환 : Attack -> Move");
            currentTime = 0;
        }
    }
    void Return()
    {
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

        }
        else
        {
            transform.position = originPos;

            hp = maxHp;
            m_State = EnemyState.Idle;
            print("상태전환 : Return -> Idle");
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f);

        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);
    }
    //데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
       //만일, 이미 피격 상태이거나 사망 상태 또는 복귀 상태라면 아무런 처리도 하지 않고 함수를 종료한다.
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        hp -= hitPower;
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환 : Any state -> Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    }
}
