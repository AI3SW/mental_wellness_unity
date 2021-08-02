using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
public class AppManager : MonoBehaviour
{
    public StarGan_Controller StarganController;
    public ScenarioController ScenarioController;
    public AvatarManager AvatarManager;
    public UnityDecoupledBehavior.PageController PgController;
    public WebcamController WCcontroller;

    public string directoryPath = "/Data/";
    public string filename = "Avatars";

    public ErrorPage ePage;
    public GameObject LoadingPage;
    private void Awake()
    {
        StarganController.On_Receive_Results += OnReceiveResults;
        AvatarManager.LoadData(directoryPath+ filename);
    }

    public void OnReceiveResults(Astar.REST.FaceTech.Output data)
    {
        //process json calls functions accordingly
        Debug.Log(data);

        if (data != null)
        {
            AvatarManager.UpdateAIPotrait(data.output_img);
            ScenarioController.ScenarioStart();
            Debug.Log("data processed");
        }
        else
        {
            Debug.Log("data not processed");
        }
    }

    public void TakeImage()
    {
        if (WCcontroller.IsScreenshotReady())
        {
            WCcontroller.getWebcamTextureString(WebcamController.ImageFormat.JPEG);
            //WCcontroller.StopCamera();
            AvatarManager.UpdateTakenPotrait();
            PgController.transitPage(PgController.current + 1);
            
        }
        else
        {
            //Camera not ready / error;
        }
    }

    [SerializeField]
    Button Sendpicture; 
    async public void SendImage()
    {
        Sendpicture.interactable = false;
        //pop up loading page
        LoadingPage.gameObject.SetActive(true);
        var connectionResult = await StarganController.useStarGan(WCcontroller.imagestring, AvatarManager.currentSelectedId);
        await Task.Delay(500);
        //unpop loading page
        LoadingPage.gameObject.SetActive(false);
        //wait results
        if (connectionResult)
        {
            Sendpicture.interactable = true;
            PgController.transitPage(PgController.current+1);
            Debug.Log("data processed");
        }
        else
        {
            Sendpicture.interactable = false;
            ePage.Activate("Unable To Connect, Please Check ur Connectoin");
            Debug.Log("data not processed");
        }
    }

    public GameObject ShareNextButton;
    public void UpdateUnlockStatusAndShowProgressionPage()
    {
        Debug.Log("update");
        if(OnGoingSession)
        {
            ShareNextButton.gameObject.SetActive(true);
            AvatarManager.UnlockAvatarResult();
            AvatarManager.SaveData(directoryPath, filename);
            //disable the button
            EndSession();
        } else
        {
            ShareNextButton.gameObject.SetActive(false);
        }
    }

  public  bool OnGoingSession;
    public void StartSession()
    {
        OnGoingSession = true;
        StarganController.StartSession();
    }

    public void EndSession()
    {
        OnGoingSession = false;
        StarganController.EndSession();
    }
    public void SaveImageToGallery()
    {
        string appName = "Share The Avatar with your friends";
        string photoName = "Fantasy " + AvatarManager.getAvatarName();
        
        try
        {
            ShareController.SaveImageToGallery(AvatarManager.getAvatarImage(), appName, photoName);
            ePage.Activate("Image is saved to Gallery");
        }
        catch (Exception e)
        {
            ePage.Activate(e.Message);
        }
    }
    public void ShareAvatar()
    {
        string title = "Share The Avatar with your friends";
        string photoName = "Fantasy " + AvatarManager.getAvatarName(); 
        string description = "See How ur friends was living in their fantasy world";

        try { 
            ShareController.SharePhoto(AvatarManager.getAvatarImage(), title, photoName, description);
        }
        catch (Exception e)
        {
            ePage.Activate(e.Message);
        }
    }
    public void ShareApp()
    {
        string title = "Download and try to expierience and create ur own fantasy creatures";
        string url = "Fantasy Wellness";
        string description = "Expierience your own fantasy world with their scenarios and learn about coping with different situations";
        
        try
        {
            ShareController.ShareAppLink(title, url, description);
        }
        catch (Exception e)
        {
            ePage.Activate(e.Message);
        }
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
