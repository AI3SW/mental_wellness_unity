using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AvatarManager : MonoBehaviour
{
    [System.Serializable]
    public class Avatar
    {
        public int id;
        public string name;
        public Sprite img;
    }
    [SerializeField]
    private List<Avatar> AvatarList;
    [SerializeField]
    private List<AvatarHolder> AvatarHolders;
    [SerializeField]
    private RectTransform holderParent;
    [SerializeField]
    private AvatarHolder Prefab;
    [SerializeField]
    private RawImage OriginalPotraitHolder;
    [SerializeField]
    private RawImage AIPotraitHolder;
    Texture2D snap;
    [SerializeField]
    private WebcamController WCcontroller;
    [HideInInspector]
    public int currentSelectedId;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Convert.ToBase64String(AvatarList[0].img.texture.EncodeToPNG()) );
        holderParent.sizeDelta = new Vector2(Mathf.Abs(holderParent.rect.y)* AvatarList.Count , holderParent.sizeDelta.y);
        foreach (Avatar item in AvatarList)
        {
            AvatarHolder temp = Instantiate<AvatarHolder>(Prefab, holderParent);
            
            temp.transform.SetParent(holderParent);
            temp.updateAvatarData(item.id,item.name,item.img);
            temp.btnEvent(doAllButton);
            AvatarHolders.Add(temp);
        }
    }

    public void doAllButton(int id)
    {
        Debug.Log(id);
        currentSelectedId = id;
        UnselectAllHolders();
        AvatarHolders[id-1].setSelectedColor();
    }

    void UnselectAllHolders()
    {
        foreach (AvatarHolder item in AvatarHolders)
        {
            item.setUnselectedColor();
        }
    }

    public void UpdateNewPotrait()
    {
        Texture2D newPhoto = new Texture2D(1, 1);
        newPhoto.LoadImage(Convert.FromBase64String(WCcontroller.imagestring));
        newPhoto.Apply();
        
        OriginalPotraitHolder.texture = newPhoto;
    }
    public void UpdateAIPotrait(string imgstr)
    {
        Texture2D newPhoto = new Texture2D(1, 1);
        newPhoto.LoadImage(Convert.FromBase64String(imgstr));
        newPhoto.Apply();

        AIPotraitHolder.texture = newPhoto;
    }
}
