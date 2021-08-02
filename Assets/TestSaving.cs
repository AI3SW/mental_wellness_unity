using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestSaving : MonoBehaviour
{
    [SerializeField] Image ImgToSave;

    NativeShare shareMaster;
    // Start is called before the first frame update
    void Start()
    {
        shareMaster = new NativeShare();

        //NativeGallery.SaveImageToGallery(ImgToSave.sprite.texture, "MonsterWellness", "myName");

    }

    public void share()
    {
        shareMaster.SetText("hello");
        shareMaster.Share();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
