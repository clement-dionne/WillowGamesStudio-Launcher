using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net;
using System;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine.Diagnostics;

public class Updater : MonoBehaviour
{
    public string RootPath;
    public string LauncherInfoFolder;
    public string LauncherVersionText;
    public string OnlineLauncherVersionAddress = "";
    public string OnlineLauncherSetupAddress = "";

    public GameObject CheckForUpdateButton;
    public GameObject LoadingBar;

    public UIManager UImanager;

    void Start()
    {
        CheckForUpdateButton.SetActive(true);
        LoadingBar.SetActive(false);
        RootPath = Directory.GetCurrentDirectory();

        if (!File.Exists(Path.Combine(RootPath, "LauncherInfo")))
        {
            Directory.CreateDirectory(Path.Combine(RootPath, "LauncherInfo"));
        }
        LauncherInfoFolder = Path.Combine(RootPath, "LauncherInfo");

        if (!File.Exists(Path.Combine(LauncherInfoFolder, "Version.txt")))
        {
            File.Create(Path.Combine(LauncherInfoFolder, "Version.txt")).Close();

            File.WriteAllText(Path.Combine(LauncherInfoFolder, "Version.txt"), "0.0.0");
        }
        LauncherVersionText = Path.Combine(LauncherInfoFolder, "Version.txt");
    }
    public void UpdateButton()
    {
        if (!CheckLauncherVersion())
        {
            try { UpdateLauncher(); } catch (Exception error) { UImanager.Error("Launcher Update Error : " + error); }
            CheckForUpdateButton.SetActive(false);
            LoadingBar.SetActive(true);
        }
    }

    public void UpdateLauncher()
    {
        WebClient Client = new WebClient();

        Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Download);

        void Download(object sender, DownloadProgressChangedEventArgs Event)
        {
            double Coming = double.Parse(Event.BytesReceived.ToString());
            double Total = double.Parse(Event.TotalBytesToReceive.ToString());
            double Ratio = (Coming / Total) * 100;
            print(Ratio);
            LoadingBar.GetComponent<Slider>().value = (int)Ratio;
        }

        Client.DownloadFileCompleted += new AsyncCompletedEventHandler(InstallUpdate);
        Client.DownloadFileAsync(new Uri(OnlineLauncherSetupAddress), "WillowsGamesStudiosLauncherSetUp.exe");
    }

    public void InstallUpdate(object sender, AsyncCompletedEventArgs Event)
    {
        try 
        {
        WebClient C = new WebClient();
        string OnlineLauncherVersion = C.DownloadString(OnlineLauncherVersionAddress);
        File.WriteAllText(LauncherVersionText, OnlineLauncherVersion);

        Process.Start("WillowsGamesStudiosLauncherSetUp.exe");
        Application.Quit();
        Utils.ForceCrash(0);
        } 
        catch (Exception error) { UImanager.Error("Launcher Update Error : " + error); }
    }

    public bool CheckLauncherVersion()
    {
        string[] LocalVersion = File.ReadAllText(LauncherVersionText).Split('.');
        WebClient C = new WebClient();
        string[] OnlineVersion = C.DownloadString(OnlineLauncherVersionAddress).Split('.');

        if (LocalVersion.Length != OnlineVersion.Length) return false;

        for (int index = 0; index < LocalVersion.Length; index ++)
        {
            if (LocalVersion[index] != OnlineVersion[index])
            {
                return false;
            }
        }
        return true;
    }
}
