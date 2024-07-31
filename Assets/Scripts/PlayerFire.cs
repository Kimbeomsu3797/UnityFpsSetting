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

    //카메라 확대 확인용 변수
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
    //public GameObject[] WeaponImage;
    //public GameObject[] SubWeaponImage;
    public GameObject weapon01;
    public GameObject weapon02;
    public GameObject crosshair01;
    public GameObject crosshair02;
    public GameObject weapon01_R;
    public GameObject weapon02_R;
    public GameObject sniper_Zoom;
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
        //게임 상태가 게임중 상태일때만 조작가능하게
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
                    //줌(우클릭)시 화면 확대 + 줌모드 상태로 변경
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                        sniper_Zoom.SetActive(true);
                        crosshair02.SetActive(false);
                    }
                    //그렇지 않으면 카메라를 원래 상태로 되돌리고 줌 모드 상태를 해제
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                        sniper_Zoom.SetActive(false);
                        crosshair02.SetActive(true);
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
       // 1번 입력시 노말모드로 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            //카메라의 화면을 다시 원래대로 돌려준다.
            Camera.main.fieldOfView = 60f;
            weaponMode.text = "Normal Weapon";
            //WeaponImage[1].SetActive(false);
            //SubWeaponImage[1].SetActive(false);
            //WeaponImage[0].SetActive(true);
            //SubWeaponImage[0].SetActive(true);
            weapon01.SetActive(true);
            weapon02.SetActive(false);
            crosshair01.SetActive(true);
            crosshair02.SetActive(false);
            weapon01_R.SetActive(true);
            weapon02_R.SetActive(false);
            sniper_Zoom.SetActive(false);
        }
        //만일 키보드의 숫자 2번 입력을 받으면, 무기 모드를 스나이퍼 모드로 변경한다.
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;
            weaponMode.text = "Sniper Weapon";
            weaponMode.text = "Normal Weapon";
            //WeaponImage[0].SetActive(false);
            //SubWeaponImage[0].SetActive(false);
            //WeaponImage[1].SetActive(true);
            //SubWeaponImage[1].SetActive(true);
            weapon01.SetActive(false);
            weapon02.SetActive(true);
            crosshair01.SetActive(false);
            crosshair02.SetActive(true);
            weapon01_R.SetActive(false);
            weapon02_R.SetActive(true);
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
