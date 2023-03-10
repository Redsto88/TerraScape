using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePreview : MonoBehaviour
{
    private Image _image;

    [SerializeField] private List<Sprite> imgList;
    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        _image.sprite = imgList[0];
    }

    public void ChangeImage(float textureNumber)
    {
        _image.sprite = imgList[(int)textureNumber];
    }
}
