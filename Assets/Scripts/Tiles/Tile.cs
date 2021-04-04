using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class Tile : MonoBehaviour
{
   private PlayingFieldManager playingFieldManager;
  
   public Light tileBacklight;
   public static Tile SelectedTile;
   public TileType tileType;

   [Inject]
   private void Construct(PlayingFieldManager playingFieldManager)
   {
      this.playingFieldManager = playingFieldManager;
   }
   
   private void Awake()
   {
      Init();
   }

   private void Init()
   {
      tileBacklight = GetComponent<Light>();
   }
   
   
   private void OnMouseDown()
   {
      SelectTile();
   }

   private void SelectTile()
   {
      if (SelectedTile != this && SelectedTile != null)
      {
         //TODO - проверка на допустимость свапа
         playingFieldManager.SwapTiles(this, SelectedTile);
         
         this.tileBacklight.enabled = false;
         SelectedTile.tileBacklight.enabled = false;
         
         SelectedTile = null;
         return;
      }
      
      SelectedTile = SelectedTile == this ? null : this;
      tileBacklight.enabled = SelectedTile == this;
   }
}
