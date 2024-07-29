using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    float h;
    float z;
    float playerSpeed = 5f;
    
    CharacterController cc;
    float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 10f;
    public bool isJumping = false;
    public int hp = 20;
    int maxHp = 20;
    public Slider hpSlider;
    public GameObject hitEffect;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        h = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, z);
        dir = dir.normalized;
        dir = Camera.main.transform.TransformDirection(dir);
        transform.position += dir * playerSpeed * Time.deltaTime;
        anim.SetFloat("MoveMotion", dir.magnitude);
        //transform.Translate(new Vector3(z*playerSpeed*Time.deltaTime, 0, h * playerSpeed * Time.deltaTime));
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }
        if (Input.GetButtonDown("Jump")&& !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * playerSpeed * Time.deltaTime);
        hpSlider.value = (float)hp / (float)maxHp;
    }

    public void DamageAction(int damage)
    {
        //만약 에너미가 공격하는 판정에 충돌하였다면 내 체력을 깎아줘
        //체력이 0 이하라면 DIE모션으로 넘어가줘

        hp -= damage;
        if (hp > 0)
        {
            StartCoroutine(PlayHitEffect());
        }
    }
    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        hitEffect.SetActive(false);
    }
}
