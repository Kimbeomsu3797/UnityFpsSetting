using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

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
    Quaternion originRot;

    public float moveDistance = 20f;

    public int attackPower = 3;

    public int hp = 15;
    public int maxHp = 15;
    public Slider EnemyHpslider;

    Animator anim;

    NavMeshAgent smith;
    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;
        player = GameObject.Find("Player").transform;
        cc = GetComponent<CharacterController>();
        originPos = transform.position;
        maxHp = hp;
        anim = transform.GetComponentInChildren<Animator>();
        originRot = transform.rotation;
        smith = GetComponent<NavMeshAgent>();
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
            print("���� ��ȯ : Idle -> Move");

            anim.SetTrigger("IdleToMove");
        }
    }
    void Move()
    {
        //cc.Move(pos * moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(player.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("���� ��ȯ : Move -> Return");
        }
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //Vector3 dir = (player.position - transform.position).normalized;

            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;
            //�׺���̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            smith.isStopped = true;
            smith.ResetPath();
            
            //������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
            smith.stoppingDistance = attackDistance;
            //�׺���̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�.
            smith.destination = player.position;
        }
        else
        {
            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");
            currentTime = attackDelay;
            anim.SetTrigger("MoveToAttackDelay");
        }
        
    }
    void Attack()
    {
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;
                anim.SetTrigger("StartAttack");
            }
        }
        else
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Attack -> Move");
            currentTime = 0;
            anim.SetTrigger("AttackToMove");
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }
    void Return()
    {
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);
            //transform.forward = dir;

            //������̼��� �������� �ʱ� ����� ��ġ�� �����Ѵ�.
            smith.destination = originPos;
            //������̼����� �����ϴ� �ּ� �Ÿ��� 0���� �����Ѵ�.
            smith.stoppingDistance = 0;
        }
        else
        {
            //������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
            smith.isStopped = true;
            smith.ResetPath();
            
            transform.position = originPos;
            transform.rotation = originRot;
            hp = maxHp;
            m_State = EnemyState.Idle;
            print("������ȯ : Return -> Idle");
            anim.SetTrigger("MoveToIdle");
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(1.0f);

        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
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
        print("�Ҹ�!");
        Destroy(gameObject);
    }
    //������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
       //����, �̹� �ǰ� �����̰ų� ��� ���� �Ǵ� ���� ���¶�� �ƹ��� ó���� ���� �ʰ� �Լ��� �����Ѵ�.
        if(m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        hp -= hitPower;
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ : Any state -> Damaged");
            anim.SetTrigger("Damaged");
            Damaged();
            
        }
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any state -> Die");
            anim.SetTrigger("Die");
            Die();
        }
    }
}
