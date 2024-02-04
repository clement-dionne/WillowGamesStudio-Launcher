using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AccountManager : MonoBehaviour
{
    #region Unity public
    public bool IsConnectedAndOK = false;
    public InputField AccNameLogin;
    public InputField AccPswrdLogin;
    public Toggle KeepConnectedLogin;

    public InputField AccNameSignIn;
    public InputField AccPswrdSignIn;
    public InputField AccEmail;
    public Toggle KeepConnectedSignIn;

    public Text ErrorLog;

    public Button LoginButton;
    public Button SignInButton;
    #endregion

    #region Visual Studios Public
    public string SaveFolderName = "Accounts";
    public string PlayerName = "Player1";
    public string SaveFileExtention = "WgsAccount";
    public bool StartSaveSys = true;
    #endregion

    #region Visual Studios Private
    private string GameFolder;
    private string SaveFolder;
    private string SaveFile;
    #endregion

    void Start()
    {
        StartSaveSys = true;

        GameFolder = Directory.GetCurrentDirectory();
        if (!File.Exists(SaveFolder)) Directory.CreateDirectory(SaveFolderName);
        SaveFolder = Path.Combine(GameFolder, SaveFolderName);

        ErrorLog.text = "";

        IsConnectedAndOK = Convert.ToBoolean(PlayerPrefs.GetString("KeepConnected"));
        PlayerName = PlayerPrefs.GetString("AccountName");

        StartSaveSys = false;
    }

    private void Update()
    {
        if (AccPswrdLogin.text == null || AccPswrdLogin.text == "" || AccNameLogin.text == null || AccNameLogin.text == "") LoginButton.interactable = false;
        else LoginButton.interactable = true;

        if (AccNameSignIn.text == null || AccNameSignIn.text == "" || AccPswrdSignIn.text == null || AccPswrdSignIn.text == "") SignInButton.interactable = false;
        else SignInButton.interactable = true;
    }

    public void ConnectToAccount()
    {
        PlayerName = AccNameLogin.text;
        SaveFile = PlayerName + "." + SaveFileExtention;
        SaveFile = Path.Combine(SaveFolder, SaveFile);

        if (File.Exists(SaveFile))
        {
            if (LoadString("WillowsGSAccPassWord") == AccPswrdLogin.text)
            {
                IsConnectedAndOK = true;

                if (KeepConnectedLogin.isOn)
                {
                    KeepConnectedToAccount();
                }
            }
            else
            {
                ErrorLog.text = "Invalid User Name or Password";
            }
        }
        else
        {
            ErrorLog.text = "Invalid User Name or Password";
        }
    }

    public void CreateAccount()
    {
        PlayerName = AccNameSignIn.text;
        SaveFile = PlayerName + "." + SaveFileExtention;
        SaveFile = Path.Combine(SaveFolder, SaveFile);

        if (File.Exists(SaveFile))
        {
            ErrorLog.text = "User Name Already Used";
        }
        else
        {
            File.Create(SaveFile).Close();
            SaveData(PlayerName, "AccUserName");
            SaveData(AccPswrdSignIn.text, "WillowsGSAccPassWord");
            SaveData(AccEmail.text,"AccUserEmail");

            if (KeepConnectedSignIn.isOn)
            {
                KeepConnectedToAccount();
            }

            IsConnectedAndOK = true;
        }
    }

    public string ReadData()
    {
        return File.ReadAllText(SaveFile);
    }

    public void DeleteSaveFile()
    {
        File.Delete(SaveFile);
    }

    public void DeleteSaveByKey(string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        string NewData = "";
        bool KeyFound = false;

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                KeyFound = true;
            }
            else
            {
                if (Data != "")
                {
                    NewData += Data + "\n";
                }
            }
        }

        if (!KeyFound)
        {
            Debug.LogError("Key Not Found");
        }

        File.WriteAllText(SaveFile, NewData);
    }

    public void SaveData(string DataToSave, string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        string NewData = "";
        bool KeyFound = false;

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                NewData += DataKey + "=" + DataToSave + "\n";
                KeyFound = true;
            }
            else
            {
                if (Data != "")
                {
                    NewData += Data + "\n";
                }
            }
        }

        if (!KeyFound)
        {
            NewData += DataKey + "=" + DataToSave + "\n";
        }

        File.WriteAllText(SaveFile, NewData);
    }

    public string LoadString(string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                return Data.Split('=')[1];
            }
        }
        Debug.LogError("Key Not Found");
        return "null";
    }

    public float LoadFloat(string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                return Convert.ToSingle(Data.Split('=')[1]);
            }
        }
        Debug.LogError("Key Not Found");
        return 0f;
    }

    public int LoadInt(string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                return Convert.ToInt32(Data.Split('=')[1]);
            }
        }
        Debug.LogError("Key Not Found");
        return 0;
    }

    public bool LoadBool(string DataKey)
    {
        string[] DataSaved = ReadData().Split('\n');

        foreach (string Data in DataSaved)
        {
            if (Data.Split('=')[0] == DataKey)
            {
                return Convert.ToBoolean(Data.Split('=')[1]);
            }
        }
        Debug.LogError("Key Not Found");
        return false;
    }

    public void KeepConnectedToAccount()
    {
        PlayerPrefs.SetString("KeepConnected", "true");
        PlayerPrefs.SetString("AccountName", PlayerName);
    }

    public void DisconnectAccount()
    {
        IsConnectedAndOK = false;
        PlayerPrefs.SetString("KeepConnected", "false");
        PlayerPrefs.SetString("AccountName", "");
    }
}