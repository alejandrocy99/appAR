using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;
    public static GameManager instance;
    //nos aseguramos que solo hay una instacia de gamemanager
    private void Awake(){
        if(instance!=null && instance !=  this){
            Destroy(gameObject);
        }else
        {
            instance = this;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    public void MainMenu(){
        OnMainMenu?.Invoke();
        Debug.Log("main menu activated");
    }

    public void ItemMenu(){
        OnItemsMenu?.Invoke();
        Debug.Log("items menu activated");
    }

    public void ARPosition(){
        OnARPosition?.Invoke();
        Debug.Log("AR Position activated");
    }

    public void CloseApp(){
        Application.Quit();
    }
}
