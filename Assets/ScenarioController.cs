using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScenarioController : MonoBehaviour
{
    int currentScenario;
    int currentAnswer;

    [System.Serializable]
    public class Endings {
        public string answer;
        public string endingline;
        public Sprite sprite;
    }

    [System.Serializable]
    public class Scenario
    {
        public string name;
        public Sprite ScenarioSprite;
        public string Question;
        public List<Endings> ending;
    }

    [SerializeField]
    private List<Scenario> ScenariosToLoad;
    [SerializeField]
    private Image EndingImageUIHolder;
    [SerializeField]
    private Image ExtraThoughtsImageUIHolder;

    [SerializeField]
    private List<TextMeshProUGUI> answersUI;
    [SerializeField]
    private TextMeshProUGUI ScenarioQuestion;
    [SerializeField]
    private TextMeshProUGUI ScenarioEnding;
    // Start is called before the first frame update

    public int getRandomScenario()
    {
        currentScenario = Random.Range(0, ScenariosToLoad.Count);
        return currentScenario;
    }
    public Sprite getScenarioSprite()
    {
        return ScenariosToLoad[currentScenario].ScenarioSprite;
    }
    public Sprite getEndingSprite()
    {
        return ScenariosToLoad[currentScenario].ending[currentAnswer].sprite;
    }

    public string getEndingLine()
    {
        return ScenariosToLoad[currentScenario].ending[currentAnswer].endingline;
    }
    public void ScenarioStart()
    {
        getRandomScenario();
        //StartingImageUIHolder.sprite = getScenarioSprite();
        for (int i = 0; i < answersUI.Count;++i)
        {
            answersUI[i].text = ScenariosToLoad[currentScenario].ending[i].answer;
        }
        ScenarioQuestion.text = ScenariosToLoad[currentScenario].Question;
    }

    public void UpdateEnding()
    {
        EndingImageUIHolder.sprite = getEndingSprite();
        ExtraThoughtsImageUIHolder.sprite = getEndingSprite();
        ScenarioEnding.text = getEndingLine();
    }
    public void goToEnding(int endingNum)
    {
        currentAnswer = endingNum-1;
        UpdateEnding();
    }
}
