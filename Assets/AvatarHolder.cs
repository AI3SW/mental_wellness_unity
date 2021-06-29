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
    private Image _img;

    [SerializeField]
    private Image _frame;

    [SerializeField]
    private Button _btn;

    [SerializeField]
    private Color Selected;
    [SerializeField]
    private Color Unselected;
    public void updateAvatarData(int id ,string newText, Sprite newImg)
    {
        styleID = id;
        _name.text = newText;
        _img.sprite = newImg;
        //Debug.Log(styleID);
    }

    public void btnEvent(UnityAction<int> Uaction)
    {
        _btn.onClick.AddListener(delegate { Uaction(styleID); });
    }

    public void setSelectedColor()
    {
        _frame.color = Selected;
        Debug.Log(_name.text + "activated");
    }
    public void setUnselectedColor()
    {
        _frame.color = Unselected;
        Debug.Log(_name.text + "deactivated");
    }
}
