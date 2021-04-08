using UnityEngine;
using Zenject;

public class TileSpawner : MonoBehaviour
{
   private TileFactory tileFactory;

   [Inject]
   private void Construct(TileFactory tileFactory)
   {
      this.tileFactory = tileFactory;
   }
   
   private void Awake()
   {
   }

   private void SpawnTile(TileType tileType)
   {
      GameObject tile = tileFactory.GetTileGameObject(tileType);
      Instantiate(tile, gameObject.transform);
   }

   private void SpawnRandomTile()
   {
      GameObject tile = tileFactory.GetTileGameObject(MiscTools.GetRandomTypeOfTile());
      Instantiate(tile, gameObject.transform);
   }
}
