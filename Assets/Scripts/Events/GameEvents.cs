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

    public event EventHandler<MyEventArgs> TileClicked;

    public void OnTileClicked()
    {
        MyEventArgs args = new MyEventArgs {clipName = "click"};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventArgs> WrongTileClicked;
    
    public void OnWrongTileClicked()
    {
        MyEventArgs args = new MyEventArgs {clipName = "wrong_move"};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventArgs> TilesSwapped;
    
    public void OnTilesSwapped()
    {
        MyEventArgs args = new MyEventArgs {clipName = "swap"};
        TileClicked?.Invoke(this, args);
    }

}
