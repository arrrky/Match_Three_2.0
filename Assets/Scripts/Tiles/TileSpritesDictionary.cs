﻿using System;
using UnityEditor;
using UnityEngine;

[Serializable] public class TileSpritesDictionary : SerializableDictionary<TileType, Sprite> {}

[CustomPropertyDrawer(typeof(TileSpritesDictionary))]
public class MyTileSpritesDictionaryDrawer: DictionaryDrawer<TileType, Sprite> { }
