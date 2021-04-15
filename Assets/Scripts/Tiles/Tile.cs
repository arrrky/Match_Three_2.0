using UnityEngine;
using Zenject;

public class Tile : MonoBehaviour
{
   private PlayingFieldManager _playingFieldManager;

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
      this._playingFieldManager = playingFieldManager;
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
            GameEvents.Instance.OnWrongTileClicked();
            return;
         }
         
         GameEvents.Instance.OnTilesSwapped();
         
         _playingFieldManager.SwapTiles(this, selectedTile);
         
         this.tileBacklight.enabled = false;
         selectedTile.tileBacklight.enabled = false;
         
         selectedTile = null;
         return;
      }
      
      GameEvents.Instance.OnTileClicked();

      selectedTile = selectedTile == this ? null : this;
      tileBacklight.enabled = selectedTile == this;
   }

   // Разрешаем свап, только если плитки в одном ряду или в одном столбце
   private bool IsSwapCorrect()
   {
      return (this.TileIndex.Row == selectedTile.TileIndex.Row) || (this.TileIndex.Column == selectedTile.TileIndex.Column);
   }
}
