using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

    private const float TimeBetweenRandomSwaps = 0.01f;

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
        IPlayingFieldGenerator playingFieldGenerator = new PlayingFieldGenerator(rowsCount,columnsCount);
        TileType[,] randomPlayingField = playingFieldGenerator.GetRandomPlayingField();
        
        Vector3 pointToInstantiateTile =
            new Vector3(topLeftPointOfPlayingField.x, topLeftPointOfPlayingField.y - spriteShift.y, 0);

        for (int rowNumber = 0; rowNumber < rowsCount; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < columnsCount; columnNumber++)
            {
                TileType randomTileType = randomPlayingField[rowNumber, columnNumber];
                GameObject tile = tileFactory.GetTileGameObject(randomTileType);

                playingField[rowNumber, columnNumber] =
                    Instantiate(tile, pointToInstantiateTile, quaternion.identity, gameObject.transform)
                        .GetComponent<Tile>();
                
                playingField[rowNumber, columnNumber].tileIndex = new Vector2Int(rowNumber, columnNumber);
                playingField[rowNumber, columnNumber].tileType = randomTileType;
                
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

        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(SwapRandomTilesRoutine());

            foreach (var tile in playingField)
            {
                Destroy(tile.gameObject);
            }
            
            FillPlayingField();
        }
    }

    private IEnumerator SwapRandomTilesRoutine()
    {
        float randomSwappingTime = 2f;

        do
        {
            SwapRandomTiles();
            randomSwappingTime -= TimeBetweenRandomSwaps;
            yield return new WaitForSeconds(TimeBetweenRandomSwaps);
        } while (randomSwappingTime > 0);
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