using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver,
    }
    
    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }
    public static GameManager gm;
    
    public GameState gState;

    public GameObject gameLabel;
    Text gameText;
    PlayerMove player;

    public GameObject gameOption;
    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Ready...";
        gameText.color = new Color32(255, 185, 0, 255);
        StartCoroutine(ReadyToStart());
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }
    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);

        gameText.text = "Go!";

        yield return new WaitForSeconds(0.5f);

        gameLabel.SetActive(false);

        gState = GameState.Run;
    }
    // Update is called once per frame
    void Update()
    {
        if(player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            gState = GameState.GameOver;
            //상태 텍스트의 자식 오브젝트의 트랜스폼 컴포넌트를 가져온다.
            Transform buttons = gameText.transform.GetChild(0);

            buttons.gameObject.SetActive(true);
        }
        
    }
    //퍼즈 (일시정지) 함수
    public void OpenOptionWindow()
    {
        gameOption.SetActive(true); // 패널 활성화
        Time.timeScale = 0f; // 게임속도 0배속
        gState = GameState.Pause; // 게임 상태 퍼즈로 변경
    }

    //계속하기 옵션
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false); // 패널 비활성화
        Time.timeScale = 1f; // 게임속도 1배속
        gState = GameState.Run; // 게임 상태 시작으로 변경
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
