using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
        Vector3 pointToInstantiateTile =
            new Vector3(topLeftPointOfPlayingField.x, topLeftPointOfPlayingField.y - spriteShift.y, 0);

        for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                GameObject tile = tileFactory.GetTileGameObject(tileSpawner.GetRandomTypeOfTile());

                playingField[rowNumber, columnNumber] =
                    Instantiate(tile, pointToInstantiateTile, quaternion.identity, gameObject.transform)
                        .GetComponent<Tile>();
                pointToInstantiateTile.x += distanceBetweenTiles;
            }

            pointToInstantiateTile.x = topLeftPointOfPlayingField.x;
            pointToInstantiateTile.y -= distanceBetweenTiles;
        }
    }

    public void SwapTiles(Tile tile0, Tile tile1)
    {
        Vector3 tempTransformPosition = tile0.gameObject.transform.position;
        tile0.gameObject.transform.position = tile1.gameObject.transform.position;
        tile1.gameObject.transform.position = tempTransformPosition;
    }

    // TODO - удалить, для тестов
    private void Update()
    {
        //SwapRandomTiles(); можно использовать как заставку -  красиво

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwapRandomTiles();
        }
    }

    private void SwapRandomTiles()
    {
        Vector2Int randomIndex0 = GetRandomIndex();
        Vector2Int randomIndex1 = GetRandomIndex();

        Vector3 tempTransformPosition = playingField[randomIndex0.x, randomIndex0.y].gameObject.transform.position;
        playingField[randomIndex0.x, randomIndex0.y].gameObject.transform.position =
            playingField[randomIndex1.x, randomIndex1.y].gameObject.transform.position;
        playingField[randomIndex1.x, randomIndex1.y].gameObject.transform.position = tempTransformPosition;
    }

    private Vector2Int GetRandomIndex()
    {
        return new Vector2Int(Random.Range(0, rowsCount), Random.Range(0, columnsCount));
    }
}