using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    #region SINGLETON
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    #endregion

    public event EventHandler<MyEventsArgs> TileClicked;

    public void OnTileClicked()
    {
        MyEventsArgs args = new MyEventsArgs {clipName = "click"};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventsArgs> WrongTileClicked;
    
    public void OnWrongTileClicked()
    {
        MyEventsArgs args = new MyEventsArgs {clipName = "wrong_move"};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventsArgs> TilesSwapped;
    
    public void OnTilesSwapped()
    {
        MyEventsArgs args = new MyEventsArgs {clipName = "swap"};
        TileClicked?.Invoke(this, args);
    }

}
