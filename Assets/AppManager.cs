using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
public class AppManager : MonoBehaviour
{
    public StarGan_Controller StarganController;
    public ScenarioController ScenarioController;
    public AvatarManager AvatarManager;
    public UnityDecoupledBehavior.PageController PgController;
    public WebcamController WCcontroller;

    private void Awake()
    {
        StarganController.On_Receive_Results += OnReceiveResults;
    }

    public void OnReceiveResults(Astar.REST.StarGan.Output data)
    {
        //process json calls functions accordingly
        Debug.Log(data);

        if (data != null)
        {
            AvatarManager.UpdateAIPotrait(data.output_img);
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
            WCcontroller.StopCamera();
            AvatarManager.UpdateNewPotrait();
            PgController.transitPage(PgController.current + 1);
            
        }
        else
        {
            //Camera not ready / error;
        }
    }
    async public void SendImage()
    {
        
        //pop up loading page
        var connectionResult = await StarganController.useStarGan(WCcontroller.imagestring, AvatarManager.currentSelectedId);
        //unpop loading page
        //wait results
        if (connectionResult)
        {
            //handle UI Logic when process success and connected
            //next page
            //WCcontroller.StopCamera();
            Debug.Log("data processed");
        }
        else
        {
            //handle UI Logic
            //handle UI Logic when process fail / not connected
            //disable confirm ss button.
            //stay on page
            Debug.Log("data not processed");
        }


    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
