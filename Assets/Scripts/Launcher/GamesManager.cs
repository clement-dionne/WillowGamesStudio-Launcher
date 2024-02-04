using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Game 
{
    public string GameName;
    public string ApplicationExeName;
    
    [System.NonSerialized]
    public string GameExePath;
    [System.NonSerialized]
    public string GameMainFolder;
    [System.NonSerialized]
    public string GameFiles;
    [System.NonSerialized]
    public string VersionText;
    [System.NonSerialized]
    public string ScrrenShotFolder;
    [System.NonSerialized]
    public string GameSaves;
    [System.NonSerialized]
    public string GameInfo;

    public Button OpenGameFolderButton;
    public Button StartGameButton;
}
public class GamesManager : MonoBehaviour
{
    public UIManager UImanager;
    public AccountManager Account;
    public Game[] Games;

    private string RootPath;
    private string GamesFolder;
    
    void Start()
    {
        RootPath = Directory.GetCurrentDirectory();
        if (!File.Exists(Path.Combine(RootPath, "Games")))
        {
            Directory.CreateDirectory(Path.Combine(RootPath, "Games"));
        }
        GamesFolder = Path.Combine(RootPath, "Games");

        foreach (Game game in Games)
        {
            if (!File.Exists(Path.Combine(GamesFolder, game.GameName)))
            {
                Directory.CreateDirectory(Path.Combine(GamesFolder, game.GameName));
            }
            game.GameMainFolder = Path.Combine(GamesFolder, game.GameName);

            if (!File.Exists(Path.Combine(game.GameMainFolder, "GameInfo")))
            {
                Directory.CreateDirectory(Path.Combine(game.GameMainFolder, "GameInfo"));
            }
            game.GameInfo = Path.Combine(game.GameMainFolder, "GameInfo");

            if (!File.Exists(Path.Combine(game.GameMainFolder, "Version.txt")))
            {
                File.Create(Path.Combine(game.GameMainFolder, "Version.txt")).Close();
                File.WriteAllText(Path.Combine(game.GameMainFolder, "Version.txt"), "0.0.0");
            }
            game.VersionText = Path.Combine(game.GameMainFolder, "Version.txt");

            if (!File.Exists(Path.Combine(game.GameMainFolder, "Files")))
            {
                Directory.CreateDirectory(Path.Combine(game.GameMainFolder, "Files"));
            }
            game.GameFiles = Path.Combine(game.GameMainFolder, "Files");

            if (!File.Exists(Path.Combine(game.GameFiles, "Saves")))
            {
                Directory.CreateDirectory(Path.Combine(game.GameFiles, "Saves"));
            }
            game.GameSaves = Path.Combine(game.GameFiles, "Saves");

            try { game.GameExePath = Path.Combine(game.GameFiles, game.ApplicationExeName + ".exe"); } catch { UnityEngine.Debug.LogError(game.GameName + " No exe File !"); }

            if (!File.Exists(Path.Combine(game.GameFiles, "Images")))
            {
                Directory.CreateDirectory(Path.Combine(game.GameFiles, "Images"));
            }
            game.ScrrenShotFolder = Path.Combine(game.GameFiles, "Images");
        }
        Start2();
    }

    void Start2()
    {
        UImanager.accountPanel.OpenAccountFolder.onClick.AddListener(() => Process.Start(Path.Combine(RootPath, "Accounts")));
        foreach (GamePanel gamepanel in UImanager.AllGamePanel)
        {
            int index = System.Array.IndexOf(UImanager.AllGamePanel, gamepanel);

            if (!File.Exists(Path.Combine(Games[index].GameInfo, "GameInfo.txt")))
            {
                File.Create(Path.Combine(Games[index].GameInfo, "GameInfo.txt")).Close();
            }
            gamepanel.InfoAndUpdate = File.ReadAllText(Path.Combine(Games[index].GameInfo, "GameInfo.txt"));

            gamepanel.GameVersion = "V : " + File.ReadAllText(Games[index].VersionText);

            string a = Games[index].GameExePath;
            string b = Games[index].GameMainFolder;
            Games[index].StartGameButton.onClick.AddListener(() => StartApp(a));
            Games[index].OpenGameFolderButton.onClick.AddListener(() => OpenFolder(b));
        }
    }

    public void StartApp(string AppPath)
    {
        try
        {
            Process.Start(AppPath);
            Application.Quit();
        }
        catch (Exception error)
        {
            UImanager.Error("Unable to launch application : " + error);
        }
    }

    public void OpenFolder(string FolderPath)
    {
        try
        {
            Process.Start(FolderPath);
        }
        catch (Exception error)
        {
            UImanager.Error("Unable to open folder : " + error);
        }
    }
}
