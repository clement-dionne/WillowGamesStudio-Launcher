using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GamePanel
{
    public string GameName;
    public GameObject Panel;
    public Sprite GameImage;
    public List<Sprite> ScreenShot = new List<Sprite>();
    public string InfoAndUpdate;
    public Text GameTitleText;
    public Text InfoAndUpdateContent;
    public string GameVersion;
    public Text GameVersionContent;
    public GameObject GameIconContent;
}

[System.Serializable]
public class AccountPanel{

    public GameObject Panel;
    public string UserName;
    public Sprite UserImage;
    public string[] Succes;
    public Text UserNameTitle;
    public Text LauncherVersion;
    public GameObject UserImageContent;
    public Text SuccesContent;

    public Button OpenAccountFolder;
}

public class UIManager : MonoBehaviour
{
    #region Unity Public
    public GameObject TopPanel;
    public GameObject ConnectPanel;
    public GameObject GamesPanelContent;
    public GameObject GameSelectorContent;
    public GameObject ButtonGameSelect;

    public GamePanel[] AllGamePanel;

    public AccountManager Accounts;
    public AccountPanel accountPanel;
    public Text ErrorLog;

    public GameObject errorPanel;
    #endregion


    void Start()
    {
        errorPanel.SetActive(false);
        TopPanel.SetActive(true);
        ConnectPanel.SetActive(true);
        GamesPanelContent.SetActive(false);

        for (int index = 0; index<AllGamePanel.Length; index ++)
        {
            AllGamePanel[index].Panel.SetActive(false);
        }

        for (int index = 0; index < AllGamePanel.Length; index++)
        {
            GameObject button = Instantiate(ButtonGameSelect, GameSelectorContent.transform);
            button.GetComponentInChildren<Text>().text = AllGamePanel[index].GameName;
            GamePanel a = AllGamePanel[index];
            button.GetComponent<Button>().onClick.AddListener(() => OpenGamePanel(a));
        }

        accountPanel.UserName = Accounts.PlayerName;
        accountPanel.UserNameTitle.text = PlayerPrefs.GetString("AccountName");
        GoToMainPanel();
    }

    void Update()
    {
        if (Accounts.IsConnectedAndOK)
        {
            GamesPanelContent.SetActive(true);
            ConnectPanel.SetActive(false);
        }
        else
        {
            TopPanel.SetActive(true);
            ConnectPanel.SetActive(true);
            GamesPanelContent.SetActive(false);
            accountPanel.Panel.SetActive(false);

            for (int index = 0; index < AllGamePanel.Length; index++)
            {
                AllGamePanel[index].Panel.SetActive(false);
            }
        }
    }

    public void OpenGamePanel(GamePanel Gpanel)
    {
        for (int index = 0; index < AllGamePanel.Length; index++)
        {
            AllGamePanel[index].Panel.SetActive(false);
        }
        Gpanel.Panel.SetActive(true);

        Gpanel.GameTitleText.text = Gpanel.GameName;
        Gpanel.GameIconContent.GetComponent<Image>().sprite = Gpanel.GameImage;
        Gpanel.InfoAndUpdateContent.text = Gpanel.InfoAndUpdate;
        Gpanel.GameVersionContent.text = Gpanel.GameVersion;

        accountPanel.Panel.SetActive(false);
    }

    public void GoToMainPanel()
    {
        for (int index = 0; index < AllGamePanel.Length; index++)
        {
            AllGamePanel[index].Panel.SetActive(false);
        }
        accountPanel.UserNameTitle.text = Accounts.PlayerName;
        accountPanel.UserImageContent.GetComponent<Image>().sprite = accountPanel.UserImage;
        accountPanel.SuccesContent.text = "";

        foreach (string s in accountPanel.Succes)
        {
            accountPanel.SuccesContent.text += s + "\n";
        }

        accountPanel.Panel.SetActive(true);
    }

    public void Error(string errorMess)
    {
        ErrorLog.text = errorMess;
        errorPanel.SetActive(true);
    }

    public void CloseError()
    {
        errorPanel.SetActive(false);
    }
}