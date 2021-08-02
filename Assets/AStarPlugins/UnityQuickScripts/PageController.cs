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
        public List<PageElement> pageList;
        public int current
        {
            get;
            private set;
        }
        bool inTransition;
        public PageElement firstPage;

        public UnityEvent OnAppQuitPrompt;
        public UnityEvent OnGameQuitPrompt;
        
        [System.Serializable]
        public enum pageName
        {
            Game_Screensaver = 0,
            Game_PhotoTaking,
            Game_AvatarSelection,
            Game_Scenario,
            Game_Ending,
            Share_Selection,
            Share_Progress,
            Content_Main,
            Content_SelectedTopic,
            Options
        }

        // Start is called before the first frame update
        void Awake()
        {
            pageNumberStack = new Stack<int>();
            foreach (PageElement page in pageList)
            {
                page.canvaPage = page.GetComponent<CanvasGroup>();
                page.deactivatePage();
            }
            firstPage = pageList[current];
            firstPage.activatePage();
        }

        // Update is called once per frame
        void Update()
        {
            //For android back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                goPrevPage();
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
        public void transitPage(PageElement nextPage)
        {
            PageElement currPage = pageList[current];
            if (nextPage == currPage ) return;
            else
            {
                inTransition = true;

                
                currPage.fadePage(0, 0.5f, () =>
                {
                    currPage.gameObject.SetActive(false);
                    inTransition = false;
                    currPage.TransitOut?.Invoke();
                });
                currPage.deactivatePageInteraction();

                nextPage.gameObject.SetActive(true);
                nextPage.fadePage(1, 0.5f, () =>
                {
                    nextPage.activatePageInteraction();
                    inTransition = false;
                    nextPage.TransitIn?.Invoke();
                });

                current = pageList.FindIndex((PageElement x) => { return nextPage == x;  });

            }
        }

        public void goNextBufferedPage()
        {
            if(nextPage >= 0)
            {
                goNextPage(nextPage);
                nextPage = -1;
            }
            else
            {
                Debug.LogError("page error");
            }


        }
        public void goNextPage(int nextPageId)
        {
            Debug.Log("pressed");
            if (inTransition) return;
            transitPage(nextPageId);
            pageNumberStack.Push(nextPageId);
        }
        public void goPrevPage()
        {
            int prevNum = 0;
            //Debug.Log("Prev Page");
            Debug.Log(pageNumberStack.Count);
            if (inTransition) return;
            if (isInGame()) {
                Debug.Log("prompt game quit Page");
                nextPage = (int)pageName.Game_Screensaver;
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

        int nextPage;
        public void checkBeforeGoNextPage(int pageNum)
        {
            nextPage = pageNum;
            if (inTransition) return;
            if (isInGame())
            {
                Debug.Log("prompt game quit Page");
                OnGameQuitPrompt?.Invoke();
                return;
            } else
            {
                goNextPage(pageNum);
            }
        }

        public bool isInGame()
        {
            PageElement currentPage = pageList[current];

            //hardcode
            return currentPage.gameObject.name.StartsWith("Game") && firstPage != currentPage;
        }
    }

}