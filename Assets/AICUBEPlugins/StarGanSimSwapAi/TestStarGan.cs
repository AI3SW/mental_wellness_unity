using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AICUBE.REST;

public class TestStarGan : MonoBehaviour , StarGanInterface
{
    public event Action<AICUBE.REST.FaceTech.Output> On_Receive_Results;

    [SerializeField]
    private string ip;
    [SerializeField]
    private string port;
    [SerializeField]
    private bool secured;
    [SerializeField]
    private bool debugOn = false;
    [SerializeField]
    private bool returnVal = false;
    private void Awake()
    {
    }

    public string GetName()
    {
        return "StarGanTest";
    }

    public void StartSession()
    {
    }
    public void EndSession()
    {
    }

    async public Task<bool> SendStarGan(string imgstr, int style_id)
    {

        AICUBE.REST.FaceTech.Output result = new AICUBE.REST.FaceTech.Output();
        result.output_img = imgstr;
        On_Receive_Results?.Invoke(result);
        return returnVal;
    }

}
