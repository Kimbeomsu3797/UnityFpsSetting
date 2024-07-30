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
            //���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ������ ������Ʈ�� �����´�.
            Transform buttons = gameText.transform.GetChild(0);

            buttons.gameObject.SetActive(true);
        }
        
    }
    //���� (�Ͻ�����) �Լ�
    public void OpenOptionWindow()
    {
        gameOption.SetActive(true); // �г� Ȱ��ȭ
        Time.timeScale = 0f; // ���Ӽӵ� 0���
        gState = GameState.Pause; // ���� ���� ����� ����
    }

    //����ϱ� �ɼ�
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false); // �г� ��Ȱ��ȭ
        Time.timeScale = 1f; // ���Ӽӵ� 1���
        gState = GameState.Run; // ���� ���� �������� ����
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
