using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
   private TileFactory tileFactory;
   private Array tileTypeEnumValues;

   [Inject]
   private void Construct(TileFactory tileFactory)
   {
      this.tileFactory = tileFactory;
   }
   
   private void Awake()
   {
      tileTypeEnumValues = Enum.GetValues(typeof(TileType));
   }

   private void SpawnTile(TileType tileType)
   {
      GameObject tile = tileFactory.GetTileGameObject(tileType);
      Instantiate(tile, gameObject.transform);
   }

   private void SpawnRandomTile()
   {
      GameObject tile = tileFactory.GetTileGameObject(GetRandomTypeOfTile());
      Instantiate(tile, gameObject.transform);
   }

   public TileType GetRandomTypeOfTile()
   {
      TileType randomTileType = (TileType)tileTypeEnumValues.GetValue(Random.Range(1, tileTypeEnumValues.Length));
      return randomTileType;
   }
}
