using UnityEngine;
using Zenject;

public class Tile : MonoBehaviour
{
   private PlayingFieldManager playingFieldManager;

   private TileIndex tileIndex;
   private TileType tileType;

   private Light tileBacklight;
   private static Tile selectedTile;

   #region PROPERTIES
   public TileIndex TileIndex
   {
      get => tileIndex;
      set => tileIndex = value;
   }

   public TileType TileType
   {
      get => tileType;
      set => tileType = value;
   }

   #endregion

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
      if (selectedTile != this && selectedTile != null)
      {
         //TODO - проверка на допустимость свапа
         if (!IsSwapCorrect())
         {
            return;
         }
         
         SwapTilesIndexes(this, selectedTile);
         playingFieldManager.SwapTiles(this, selectedTile);
         
         this.tileBacklight.enabled = false;
         selectedTile.tileBacklight.enabled = false;
         
         selectedTile = null;
         return;
      }
      
      selectedTile = selectedTile == this ? null : this;
      tileBacklight.enabled = selectedTile == this;
   }

   private void SwapTilesIndexes(Tile tile0, Tile tile1)
   {
      TileIndex tempIndex = tile0.TileIndex;
      tile0.TileIndex = tile1.TileIndex;
      tile1.TileIndex = tempIndex;
   }

   // Разрешаем свап, только если плитки в одном ряду или в одном столбце
   private bool IsSwapCorrect()
   {
      return (this.TileIndex.Row == selectedTile.TileIndex.Row) || (this.TileIndex.Column == selectedTile.TileIndex.Column);
   }
}
