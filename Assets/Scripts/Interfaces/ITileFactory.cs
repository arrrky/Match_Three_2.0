﻿using UnityEngine;

public interface ITileFactory
{
    GameObject CreateTile(TileType tileType);
}
