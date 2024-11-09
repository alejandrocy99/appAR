using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    private string itemName;
    private string itemDescription;
    private Sprite itemImage;
    private GameObject item3DModel;
    private ARInteractionsManager interactionsManager;
    public string ItemName
    {
        set
        {
            itemName = value;

        }
    }
    
    public string ItemDescription{set => itemDescription = value;}
    public Sprite ItemImage{set => itemImage = value;}
    public GameObject Item3DModel{set => item3DModel = value;}

    // Start is called before the first frame update
   void Start()
{
    if (itemName != null)
    {
        transform.GetChild(0).GetComponent<Text>().text = itemName;
    }
    else
    {
        Debug.LogWarning("itemName no está configurado.");
    }

    if (itemImage != null)
    {
        transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;
    }
    else
    {
        Debug.LogWarning("itemImage no está configurado.");
    }

    if (itemDescription != null)
    {
        transform.GetChild(2).GetComponent<Text>().text = itemDescription;
    }
    else
    {
        Debug.LogWarning("itemDescription no está configurado.");
    }

    var button = GetComponent<Button>();
    if (button != null)
    {
        button.onClick.AddListener(GameManager.instance.ARPosition);
        button.onClick.AddListener(Create3DModel);
    }
    else
    {
        Debug.LogWarning("Button no está presente en el GameObject.");
    }

    interactionsManager = FindAnyObjectByType<ARInteractionsManager>();
}

  private void Create3DModel()
{
    if (item3DModel != null)
    {
       interactionsManager.Item3DModel=  Instantiate(item3DModel);
    }
    else
    {
        Debug.LogWarning("item3DModel no está configurado.");
    }
}

}
