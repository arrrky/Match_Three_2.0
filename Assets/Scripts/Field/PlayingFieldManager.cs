using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlayingFieldManager : MonoBehaviour
{
    // Пример спрайта плитки, чтобы один раз получить ее размеры и не трогать
    [SerializeField] private Sprite tileSpriteExample;
    
    private TileFactory tileFactory;
    private TileSpawner tileSpawner;
    
    private int rowsCount = 10;
    private int columnsCount = 10;

    private Vector3 topLeftPointOfPlayingField;
    private Vector3 screenBounds;
    private Vector2 spriteShift;
    private float distanceBetweenTiles;

    private Tile[,] playingField;

    [Inject]
    private void Construct(TileFactory tileFactory, TileSpawner tileSpawner)
    {
        this.tileFactory = tileFactory;
        this.tileSpawner = tileSpawner;
    }

    private void Start()
    {
        Init();
        FillPlayingField();
    }

    private void Init()
    {
        playingField = new Tile[rowsCount, columnsCount];
        screenBounds = MiscTools.GetScreenBounds();
        topLeftPointOfPlayingField = new Vector3(-screenBounds.x + screenBounds.x / 2, screenBounds.y, 0);
        spriteShift = MiscTools.GetSpriteShift(tileSpriteExample);
        distanceBetweenTiles = spriteShift.x * 2;
    }

    private void FillPlayingField()
    {
        Vector3 pointToInstantiateTile = new Vector3(topLeftPointOfPlayingField.x + spriteShift.x, topLeftPointOfPlayingField.y - spriteShift.y,0);

        for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                GameObject tile = tileFactory.GetTileGameObject(tileSpawner.GetRandomTypeOfTile());
                Instantiate(tile, pointToInstantiateTile, quaternion.identity, gameObject.transform);
                pointToInstantiateTile.x += distanceBetweenTiles;
            }

            pointToInstantiateTile.x = topLeftPointOfPlayingField.x + spriteShift.x;
            pointToInstantiateTile.y -= distanceBetweenTiles;
        }
    }
}