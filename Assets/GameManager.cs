using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public struct Scenario {
        string question;
        string answer_1;
        string answer_2;
        string answer_3;
        string answer_4;
    }
    public List<Scenario> ScenarioBank;

    // Start is called before the first frame update
    void Awake()
    {
        ScenarioBank = new List<Scenario>();
        //LoadAllScenario;
        //Play Scenario;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
