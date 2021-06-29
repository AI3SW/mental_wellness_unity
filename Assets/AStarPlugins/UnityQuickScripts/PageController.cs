using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace UnityDecoupledBehavior
{
    public class PageController : MonoBehaviour
    {
        Stack<int> pageNumberStack;
        public List<CanvasGroup> pageList;
        public int current
        {
            get;
            private set;
        }
        bool inTransition;
        public CanvasGroup firstPage;

        public UnityEvent OnAppQuitPrompt;
        public UnityEvent OnGameQuitPrompt;
        
        [System.Serializable]
        public enum pageName
        {
            Game_Screensaver = 0,
            Game_Instruction,
            Game_Game,
            Game_Result,
            Game_Prize,
            WellWishes_Send,
            WellWishes_Result,
        }

        // Start is called before the first frame update
        void Awake()
        {
            pageNumberStack = new Stack<int>();
            foreach (CanvasGroup page in pageList)
            {
                deactivatePage(page);
            }
            firstPage = pageList[current];
            activatePage(firstPage);
        }

        // Update is called once per frame
        void Update()
        {
            //For android back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                prevPage();
            }
            /*
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                transitPage((int)pageName.Game_Instruction);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                transitPage((int)pageName.Game_Game);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                transitPage((int)pageName.Game_Result);
            }*/
        }

        public void transitPage(int nextPageId)
        {
            transitPage(pageList[nextPageId]);
        }
        public void transitPage(CanvasGroup nextPage)
        {
            CanvasGroup currPage = pageList[current];
            if (nextPage == currPage ) return;
            else
            {
                inTransition = true;
                

                currPage.DOFade(0, 0.5f).OnComplete(() =>
                {
                    currPage.gameObject.SetActive(false);
                    inTransition = false;
                });
                currPage.blocksRaycasts = false;
                currPage.interactable = false;

                nextPage.gameObject.SetActive(true);
                nextPage.DOFade(1, 0.5f).OnComplete(() =>
                {
                    nextPage.blocksRaycasts = true;
                    nextPage.interactable = true;
                    inTransition = false;
                });

                current = pageList.FindIndex((CanvasGroup x) => { return nextPage == x;  });

            }
        }
        void deactivatePage(CanvasGroup page)
        {
            page.alpha = 0;
            page.blocksRaycasts = false;
            page.interactable = false;
            page.gameObject.SetActive(false);
        }
        public void activatePage(CanvasGroup page)
        {
            page.alpha = 1;
            page.blocksRaycasts = true;
            page.interactable = true;
            page.gameObject.SetActive(true);
        }

        public void nextPage(int nextPageId)
        {
            Debug.Log("pressed");
            if (inTransition) return;
            transitPage(nextPageId);
            pageNumberStack.Push(nextPageId);
        }
        public void prevPage()
        {
            int prevNum = 0;
            //Debug.Log("Prev Page");
            Debug.Log(pageNumberStack.Count);
            if (inTransition) return;
            if (isInGame()) {
                Debug.Log("prompt game quit Page");
                OnGameQuitPrompt?.Invoke();
                return;
            }
            if (pageNumberStack.Count > 0)
            {
                Debug.Log("Prev Page");
                prevNum = pageNumberStack.Pop();
                transitPage(prevNum);
            } else
            {
                if(pageList[current] != firstPage)
                {
                    Debug.Log("home Page");
                    transitPage(firstPage);
                } else
                {
                    Debug.Log("prompt quit app Page");
                    OnAppQuitPrompt?.Invoke();
                }
                
            }
        }

        public bool isInGame()
        {
            CanvasGroup currentPage = pageList[current];
            return currentPage.gameObject.name.StartsWith("Game") && firstPage != currentPage;
        }
    }

}