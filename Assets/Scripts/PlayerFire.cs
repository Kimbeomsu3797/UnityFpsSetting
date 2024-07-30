using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    enum WeaponMode
    {
        Normal,
        Sniper,
    }
    WeaponMode wMode;

    //ī�޶� Ȯ�� Ȯ�ο� ����
    bool ZoomMode = false;

    public GameObject firePosition;
    public GameObject bombFactory;
    public float throwPower = 15f;
    ParticleSystem ps;
    public GameObject bulletEffect;
    public int weaponPower = 5;
    Animator anim;
    public Text gameState;
    public Text weaponMode;
    public GameObject[] eff_Flash;
    // Start is called before the first frame update
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        anim = GetComponentInChildren<Animator>();
        wMode = WeaponMode.Normal;
        weaponMode.text = "Normal Weapon";
    }

    // Update is called once per frame
    void Update()
    {
        //���� ���°� ������ �����϶��� ���۰����ϰ�
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:
                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;
                    Rigidbody rb = bomb.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
                    
                    break;
                case WeaponMode.Sniper:
                    //��(��Ŭ��)�� ȭ�� Ȯ�� + �ܸ�� ���·� ����
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }
                    //�׷��� ������ ī�޶� ���� ���·� �ǵ����� �� ��� ���¸� ����
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }
                    
                    break;
        }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
            if(Physics.Raycast(ray,out hitInfo))
            {
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;
                    bulletEffect.transform.forward = hitInfo.point;

                    ps.Play();
                }
                
            }
            StartCoroutine(ShootEffectOn(0.05f));
        }
       // 1�� �Է½� �븻���� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            //ī�޶��� ȭ���� �ٽ� ������� �����ش�.
            Camera.main.fieldOfView = 60f;
            weaponMode.text = "Normal Weapon";
           
        }
        //���� Ű������ ���� 2�� �Է��� ������, ���� ��带 �������� ���� �����Ѵ�.
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;
            weaponMode.text = "Sniper Weapon";
        }
        
    }
    IEnumerator ShootEffectOn(float i)
    {
        int randomEffect = Random.Range(0, eff_Flash.Length);
        eff_Flash[randomEffect].SetActive(true);
        yield return new WaitForSeconds(i);
        eff_Flash[randomEffect].SetActive(false);
    }
}
