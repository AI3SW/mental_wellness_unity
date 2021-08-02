using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
public class AvatarHolder : MonoBehaviour
{
    private int styleID;
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private RectTransform _rect;
    [SerializeField]
    private RawImage _img;

    [SerializeField]
    private Image _frame;

    [SerializeField]
    private Button _btn;

    [SerializeField]
    private Color Selected;
    [SerializeField]
    private Color Unselected_locked;
    [SerializeField]
    private Color Unselected_unlocked;
    public void updateAvatarData(int id ,string newText, Sprite newImg)
    {
        styleID = id;
        _name.text = newText;
        _img.texture = newImg.texture;
        //Debug.Log(styleID);
    }
    public void updateAvatarImgToSaveData(Texture2D newImg,bool unlocked)
    {
        if(unlocked)
            _img.texture = newImg;
        //Debug.Log(styleID);
    }
    public void btnEvent(UnityAction<int> Uaction)
    {
        _btn.onClick.AddListener(delegate { Uaction(styleID); });
    }

    public void btnEvent(UnityAction Uaction)
    {
        _btn.onClick.AddListener(delegate { Uaction(); });
    }

    public void setSelectedColor()
    {
        _frame.color = Selected;
        _rect.localScale = Vector3.one;
        Debug.Log(_name.text + "activated");
    }
    public void setUnselectedLockedColor()
    {
        _frame.color = Unselected_locked;
        _rect.localScale = Vector3.one * 0.9f;
        Debug.Log(_name.text + "deactivated");
    }
    public void setUnselectedUnlockedColor()
    {
        _frame.color = Unselected_unlocked;
        _rect.localScale = Vector3.one * 0.9f;
        Debug.Log(_name.text + "deactivated");
    }

    public void setBtnInteractable(bool val)
    {
        _btn.interactable = val;
    }
}
