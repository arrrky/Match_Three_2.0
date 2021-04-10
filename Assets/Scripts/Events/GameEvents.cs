using System;
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
        MyEventArgs args = new MyEventArgs {soundType = SoundType.Click};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventArgs> WrongTileClicked;
    
    public void OnWrongTileClicked()
    {
        MyEventArgs args = new MyEventArgs {soundType = SoundType.WrongMove};
        TileClicked?.Invoke(this, args);
    }
    
    public event EventHandler<MyEventArgs> TilesSwapped;
    
    public void OnTilesSwapped()
    {
        MyEventArgs args = new MyEventArgs {soundType = SoundType.Swap};
        TileClicked?.Invoke(this, args);
    }
}