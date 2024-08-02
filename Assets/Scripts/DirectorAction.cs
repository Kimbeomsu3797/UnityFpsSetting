using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //PlayebleDirector을 제어하기 위한 네임스페이스
using Cinemachine;           //ChimachineBrain을 제어하기 위한 네임스페이스

//씨네머신을 제어하기 위한 코드
public class DirectorAction : MonoBehaviour
{
    PlayableDirector pd; // 감독 오브젝트

    public Camera targetCam;
    // Start is called before the first frame update
    void Start()
    {
        //Director 오브젝트가 가지고 있는 PlayableDirector컴포넌트를 가져옴
        pd = GetComponent<PlayableDirector>();
        //타임라인을 실행한다.
        pd.Play();//플레이가 된다면 자동으로 메인카메라가 된다.
        //메인카메라로 태그가 변하지 않아도 시스템상에서는 메인카메라로 변동되어 인식이된다.
    }

    // Update is called once per frame
    void Update()
    {
       //현재 진행중인 시간이 전체 시간과 크거나 같으면(재생시간이 다 되면) 
       //여전히 내가 메인카메라일때 루프가 되는 것을 방지하기위하여 카메라를 비활성화
       if(pd.time >= pd.duration)//time이 현재시간 duration이 전체시간
        {
            //만약에 메인카메라가 타켓카메라(씨네머신에 활용하는 카메라)라면
            //제어를 위해서 씨네머신 브레인을 비활성화해라
            if(Camera.main == targetCam)
            {
                targetCam.GetComponent<CinemachineBrain>().enabled = false;
            }
            //씨네머신에 사용한 카메라도 비활성화 해라
            targetCam.gameObject.SetActive(false);
            //Director 자신을 비활성화
            gameObject.SetActive(false);
        }
    }
}
