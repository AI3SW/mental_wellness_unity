using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
public class AvatarManager : MonoBehaviour
{
    [System.Serializable]
    public class AvatarJSON
    {
        public int id;
        public string name;
        public string imgstr;
        public bool unlocked;
    }

    [System.Serializable]
    public class AvatarlistJSON
    {
        public List<AvatarJSON> avatarsList;
    }

    public AvatarlistJSON saveData;

    [System.Serializable]
    public class Avatar
    {
        public int id;
        public string name;
        public Sprite img;
        public Texture2D result;
        public bool unlocked = false;
        public string unlockedImage;
    }
    [System.Serializable]
    public class AvatarClassifications
    {
        public List<int> members;
        public string typeName;
    }
    [SerializeField]
    private List<AvatarClassifications> AvatarClasses;
    [SerializeField]
    private List<Avatar> AvatarList;
    [SerializeField]
    private Dictionary<int, AvatarHolder> AvatarHolders_Create;
    [SerializeField]
    private Dictionary<int, AvatarHolder> AvatarHolders_Share;
    [SerializeField]
    private RectTransform Create_holderParent;
    [SerializeField]
    private RectTransform Share_holderParent;

    [SerializeField]
    private AvatarHolder Prefab;
    [SerializeField]
    private RawImage OriginalPotraitHolder;
    [SerializeField]
    private RawImage AIPotraitHolder;
    [SerializeField]
    private RawImage SharePotraitHolder;
    [SerializeField]
    private WebcamController WCcontroller;
    [SerializeField]
    private Button sendImage_btn;
    [HideInInspector]
    public int currentSelectedId;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log(Convert.ToBase64String(AvatarList[0].img.texture.EncodeToPNG()) );
        Create_holderParent.sizeDelta = new Vector2(Mathf.Abs(Create_holderParent.rect.y)* AvatarList.Count , Create_holderParent.sizeDelta.y);
        Share_holderParent.sizeDelta = new Vector2(Mathf.Abs(Share_holderParent.rect.y) * AvatarList.Count, Share_holderParent.sizeDelta.y);

    }

    void UICreation(Dictionary<int, AvatarHolder> avatarholders, RectTransform parent,bool holderInteractableWhenLock)
    {
        
        foreach (Avatar item in AvatarList)
        {
            AvatarHolder tempCreate = Instantiate<AvatarHolder>(Prefab, parent);
            tempCreate.transform.SetParent(parent);
            tempCreate.updateAvatarData(item.id, item.name, item.img);
            tempCreate.btnEvent(() => {
                doAllButton(item.id, avatarholders, holderInteractableWhenLock);
            });
            avatarholders.Add(item.id, tempCreate);
        }
    }

    void addAllButtonEvent(Dictionary<int, AvatarHolder> avatarholders, UnityAction Uaction)
    {
        for (int i = 0; i < avatarholders.Count; ++i)
        {
                avatarholders[AvatarList[i].id].btnEvent(Uaction);
        }
    }

    public void loadingCreateUI()
    {
        doAllButton(AvatarList[0].id, AvatarHolders_Create, true);
    }
    public void loadingShareUI()
    {
        int id = getFirstUnlocked();
        if (id == -1) return;
        doAllButton(id, AvatarHolders_Share, false);
    }

    public int getFirstUnlocked()
    {
        for (int i = 0; i < AvatarHolders_Share.Count; ++i)
        {
            if (AvatarList[i].unlocked)
                return AvatarList[i].id;
        }
        return -1;
    }
    void UIShareUpdate()
    {

        foreach (Avatar item in AvatarList)
        {
            //AvatarHolders_Share[item.id].updateAvatarImgToSaveData(item.result, item.unlocked);
            AvatarHolders_Share[item.id].btnEvent(() => {
                Avatar temp = findAvatarWithId(currentSelectedId, AvatarList);
                if (temp.unlocked)
                    SharePotraitHolder.texture = temp.result;
            });
        }

    }

    void doAllButton(int id, Dictionary<int, AvatarHolder> avatars,bool btnInteractable)
    {
        Debug.Log(id);
        currentSelectedId = id;
        UnselectAllHolders(avatars, btnInteractable);
        avatars[currentSelectedId].setSelectedColor(); //blue
    }

    void UnselectAllHolders(Dictionary<int, AvatarHolder> avatars,bool btnInteractable)
    {
        for (int i = 0; i < avatars.Count; ++i)
        {

            if(AvatarList[i].unlocked)
            {
                Debug.Log(AvatarList[i].name + "is unlocked");
                avatars[AvatarList[i].id].setUnselectedUnlockedColor(); //green
                avatars[AvatarList[i].id].setBtnInteractable(true);
            } else
            {
                Debug.Log(AvatarList[i].name + "is locked and" + btnInteractable);
                avatars[AvatarList[i].id].setUnselectedLockedColor(); //red
                avatars[AvatarList[i].id].setBtnInteractable(btnInteractable);
            }
        }
    }

    public void UpdateTakenPotrait()
    {

        Texture2D newPhoto = new Texture2D(1, 1);
        newPhoto.LoadImage(Convert.FromBase64String(WCcontroller.imagestring));
        newPhoto.Apply();
        float orient = WCcontroller.webcamRotation;
        if (orient != 0) orient += 180;
        //Debug.Log(orient);
        OriginalPotraitHolder.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        OriginalPotraitHolder.texture = newPhoto;
    }

    public static Avatar findAvatarWithId(int id, List<Avatar> list)
    {
        foreach (Avatar obj in list)
        {
            if (obj.id == id) return obj;
        }
        return null;
    }
    public static AvatarJSON findAvatarWithId(int id, AvatarlistJSON list)
    {
        foreach (AvatarJSON obj in list.avatarsList)
        {
            if (obj.id == id) return obj;
        }
        return null;
    }
    public void UpdateAIPotrait(string imgstr)
    {
        
        AvatarJSON tempJSON = findAvatarWithId(currentSelectedId, saveData);
        Avatar tempAvatar = findAvatarWithId(currentSelectedId, AvatarList);
        //Debug.Log(findAvatarWithId(currentSelectedId, saveData).imgstr);
        tempJSON.imgstr = imgstr;
        //Debug.Log(tempJSON.imgstr);
        //Debug.Log(findAvatarWithId(currentSelectedId, saveData).imgstr);

        //saveData.avatarsList[currentSelectedId].imgstr = imgstr;
        tempAvatar.unlockedImage = imgstr;

        tempAvatar.result = new Texture2D(1, 1);
        tempAvatar.result.LoadImage(Convert.FromBase64String(imgstr));
        tempAvatar.result.Apply();
        
        AIPotraitHolder.texture = tempAvatar.result;


    }

    public void UnlockAvatarResult()
    {
        findAvatarWithId(currentSelectedId, saveData).unlocked = true;
        Avatar avatarObj = findAvatarWithId(currentSelectedId, AvatarList);
        avatarObj.unlocked = true;
        SharePotraitHolder.texture = avatarObj.result;
    }
    /// <summary>
    /// Check for null, as null will be returned if avatar is locked
    /// </summary>
    /// <returns></returns>
    public Texture2D getAvatarImage()
    {
        Avatar tempAvatar = findAvatarWithId(currentSelectedId, AvatarList);
        if (tempAvatar != null && tempAvatar.unlocked)
        {
            return tempAvatar.result;
        }
        else
        {
            return null;
        }
    }
    public string getAvatarName()
    {
        Avatar tempAvatar = findAvatarWithId(currentSelectedId, AvatarList);
        return tempAvatar.name;
    }

    public void LoadData(string filepath)
    {
        string loadedstring = Astar.Utils.IOUtils.loadStringTextfromFile(filepath);

        Debug.Log(loadedstring);
        if (string.IsNullOrEmpty(loadedstring))
        {
            CopyAvatarToSave();
        } else
        {
            saveData = JsonUtility.FromJson<AvatarlistJSON>(loadedstring);
        }


        for (int i = 0; i < saveData.avatarsList.Count; ++i)
        {
            AvatarList[i].unlocked = saveData.avatarsList[i].unlocked;
            AvatarList[i].unlockedImage = saveData.avatarsList[i].imgstr;
            if(AvatarList[i].unlocked)
            {
                AvatarList[i].result = new Texture2D(1, 1);
                AvatarList[i].result.LoadImage(Convert.FromBase64String(AvatarList[i].unlockedImage));
                AvatarList[i].result.Apply();
            }
        }
        AvatarHolders_Create = new Dictionary<int, AvatarHolder>();
        AvatarHolders_Share = new Dictionary<int, AvatarHolder>();
        //
        UICreation(AvatarHolders_Create, Create_holderParent, true);
        //addAllButtonEvent(AvatarHolders_Create, () => { sendImage_btn.interactable = true; });
        //
        UICreation(AvatarHolders_Share, Share_holderParent,false);
        UIShareUpdate();
    }

    public void OnSelectClass(int index)
    {
        List<int> temp = getClassId(index);
        Debug.Log("deactivating");
        foreach(var avatarHolder in AvatarHolders_Create)
        {
            avatarHolder.Value.gameObject.SetActive(false);
        }

        Debug.Log("activating");

        Create_holderParent.sizeDelta = new Vector2(temp.Count * Create_holderParent.rect.height, 0);
        for (int i = 0; i < temp.Count; ++i)
        {

            AvatarHolders_Create[temp[i]].gameObject.SetActive(true);
            //Debug.Log(AvatarList[i].name);
        }
    }

    public ref List<int> getClassId(int i)
    {
        return ref AvatarClasses[i].members;
    }
    public void SaveData(string directorypath,string filename)
    {
        CopyAvatarToSave();
        string jsonString = JsonUtility.ToJson(saveData);
        Astar.Utils.IOUtils.saveDataToFile(directorypath,filename, jsonString);
    }
    public void CopyAvatarToSave()
    {
        saveData = new AvatarlistJSON();
        saveData.avatarsList = new List<AvatarJSON>();
        foreach (Avatar item in AvatarList)
        {
            AvatarJSON tempItem = new AvatarJSON();
            tempItem.imgstr = item.unlockedImage;
            tempItem.name = item.name;
            tempItem.unlocked = item.unlocked;
            tempItem.id = item.id;
            saveData.avatarsList.Add(tempItem);
        }

    }
    public string getSavedImage(int index)
    {
        if(saveData.avatarsList[index].unlocked)
        {
            return saveData.avatarsList[index].imgstr;
        }
        return null;
        
    }
}
