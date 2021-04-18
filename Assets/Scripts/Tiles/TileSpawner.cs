﻿using UnityEngine;
using Zenject;

public class TileSpawner : MonoBehaviour
{
   private TileFactory tileFactory;

   [Inject]
   private void Construct(TileFactory tileFactory)
   {
      this.tileFactory = tileFactory;
   }

   private void SpawnTile(TileType tileType)
   {
      GameObject tile = tileFactory.CreateTile(tileType);
      Instantiate(tile, gameObject.transform);
   }

   private void SpawnRandomTile()
   {
      GameObject tile = tileFactory.CreateTile(MiscTools.GetRandomTypeOfTile());
      Instantiate(tile, gameObject.transform);
   }
}
