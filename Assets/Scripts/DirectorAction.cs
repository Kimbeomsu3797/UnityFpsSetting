using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //PlayebleDirector�� �����ϱ� ���� ���ӽ����̽�
using Cinemachine;           //ChimachineBrain�� �����ϱ� ���� ���ӽ����̽�

//���׸ӽ��� �����ϱ� ���� �ڵ�
public class DirectorAction : MonoBehaviour
{
    PlayableDirector pd; // ���� ������Ʈ

    public Camera targetCam;
    // Start is called before the first frame update
    void Start()
    {
        //Director ������Ʈ�� ������ �ִ� PlayableDirector������Ʈ�� ������
        pd = GetComponent<PlayableDirector>();
        //Ÿ�Ӷ����� �����Ѵ�.
        pd.Play();//�÷��̰� �ȴٸ� �ڵ����� ����ī�޶� �ȴ�.
        //����ī�޶�� �±װ� ������ �ʾƵ� �ý��ۻ󿡼��� ����ī�޶�� �����Ǿ� �ν��̵ȴ�.
    }

    // Update is called once per frame
    void Update()
    {
       //���� �������� �ð��� ��ü �ð��� ũ�ų� ������(����ð��� �� �Ǹ�) 
       //������ ���� ����ī�޶��϶� ������ �Ǵ� ���� �����ϱ����Ͽ� ī�޶� ��Ȱ��ȭ
       if(pd.time >= pd.duration)//time�� ����ð� duration�� ��ü�ð�
        {
            //���࿡ ����ī�޶� Ÿ��ī�޶�(���׸ӽſ� Ȱ���ϴ� ī�޶�)���
            //��� ���ؼ� ���׸ӽ� �극���� ��Ȱ��ȭ�ض�
            if(Camera.main == targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            //���׸ӽſ� ����� ī�޶� ��Ȱ��ȭ �ض�
            targetCam.gameObject.SetActive(false);
            //Director �ڽ��� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}
