using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileSpawner : MonoBehaviour
{
   [SerializeField] private TileFactory tileFactory;

   private Array tileTypeEnumValues;

   private void Awake()
   {
      tileTypeEnumValues = Enum.GetValues(typeof(TileType));
   }

   // TODO - убрать, нужно для тестов
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.S))
      {
         SpawnRandomTile();
      }
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

   private TileType GetRandomTypeOfTile()
   {
      TileType randomTileType = (TileType)tileTypeEnumValues.GetValue(Random.Range(0, tileTypeEnumValues.Length));
      return randomTileType;
   }
}
