using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField id;

    public TMP_InputField password;

    public TextMeshProUGUI notify;
    // Start is called before the first frame update
    void Start()
    {
        //�˻� �ؽ�Ʈ â�� ����.
        notify.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //���̵�� �н����� ���� �Լ�
    public void SaveUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� �����Ѵ�.
        if(!CheckInput(id.text, password.text))
        {
            return;
        }

        //���� �ý��ۿ� ����Ǿ� �ִ� ���̵� �������� �ʴ´ٸ�...
        if (!PlayerPrefs.HasKey(id.text))
        {
            //������� ���̵�� Ű�� �н����带 ������ �����Ͽ� �����Ѵ�.
            //�˻� �ؽ�Ʈ�� ���̵������ �Ϸ�Ǿ����ϴ�. ���
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�Ǿ����ϴ�";
        }
        //�׷��� ������ �̹� �����Ѵٴ� �޼����� ����Ѵ�.
        else
        {
            //�˻� �ؽ�Ʈ�� �̹� �����ϴ� ���̵��Դϴ�. ���
            notify.text = "�̹� �����ϴ� ���̵� �Դϴ�.";
        }
    }
    bool CheckInput(string id, string pwd)
    {
        //����, ���̵�� �н����� �Է¶��� �ϳ��� ��������� ���� ���� �Է��� �䱸�Ѵ�.
        if (id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� �н����带 �Է����ּ���.";
            //�˻� �ؽ�Ʈ�� "���̵� �Ǵ� �н����带 �Է����ּ���." ���
            return false;
        }
        //�Է��� ������� ������ true�� ��ȯ�Ѵ�.
        else
        {
            return true;
        }
    }
    //�α��� �Լ�
    public void CheckUseData()
    {
        //���� �Է� �˻翡 ������ ������ �Լ��� �����Ѵ�.
        if(!CheckInput(id.text, password.text))
        {
            return;
        }

        //����ڰ� �Է��� ���̵� Ű�� ����ؼ� �ý��ۿ� ����� ���� �ҷ��´�.
        string pass = PlayerPrefs.GetString(id.text);
        //����, ����ڰ� �Է��� �н������ �ý��ۿ��� �ҷ��� ���� ���ؼ� �����ϴٸ�
        if (password.text == pass)
        {
            //���� ��(1�� �� : firstScene(���� ��쿣 mainscene)�� �ε��Ѵ�.
            SceneManager.LoadScene(1);
        }
        //�׷��� �ʰ� �� �������� ���� �ٸ��ٸ�, ���� ���� ����ġ �޼����� �����.
        else
        {
            //�˻� �ؽ�Ʈ�� "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�." ���
            notify.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
        }
    }
}

