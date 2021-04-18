using UnityEngine;
using Zenject;

public class Tile : MonoBehaviour
{
   private PlayingFieldManager playingFieldManager;

   private Light tileBacklight;
   private SpriteRenderer spriteRenderer;
   private static Tile selectedTile;

   #region PROPERTIES

   public TileIndex TileIndex { get; set; }
   public TileType TileType { get; set; }
   public bool IsActive { get; set; } = true;

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
      spriteRenderer = GetComponent<SpriteRenderer>();
   }

   private void OnMouseDown()
   {
      Debug.Log($"Tile index: {TileIndex.Row}-{TileIndex.Column}");
      Debug.Log($"Tile type: {TileType.ToString()}");
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
         
         playingFieldManager.SwapTiles(this, selectedTile);
         
         this.tileBacklight.enabled = false;
         selectedTile.tileBacklight.enabled = false;
         
         selectedTile = null;
         return;
      }
      
      GameEvents.Instance.OnTileClicked();

      selectedTile = selectedTile == this ? null : this;
      tileBacklight.enabled = selectedTile == this;
   }

   
   // Включаем / выключаем спрайт рендерер, чтобы объект оставался на сцене, но был невидим 
   public void SetActive(bool state)
   {
      IsActive = state;
      spriteRenderer.enabled = state;
   }

   // Разрешаем свап, только если плитки в одном ряду или в одном столбце
   private bool IsSwapCorrect()
   {
      return (this.TileIndex.Row == selectedTile.TileIndex.Row) || (this.TileIndex.Column == selectedTile.TileIndex.Column);
   }
}
