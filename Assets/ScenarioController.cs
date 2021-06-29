using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScenarioController : MonoBehaviour
{
    int currentScenario;
    int currentAnswer;

    [System.Serializable]
    public class Scenario
    {
        public string name;
        public List<Sprite> sprites;
        //public Sprite sprites;
    }

    public List<Scenario> EndingToLoad;
    public Image ImageUIHolder;
    public UnityDecoupledBehavior.PageController PgController;
    // Start is called before the first frame update

    public void goToEnding(int endingNum)
    {
        PgController.transitPage(4);
    }
}
