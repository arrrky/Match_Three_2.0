using System.Collections;
using System.Collections.Generic;
using ModestTree;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public partial class PlayingFieldManager : MonoBehaviour
{
    // Пример спрайта плитки, чтобы один раз получить ее размеры и не трогать
    [SerializeField] private Sprite tileSpriteExample;

    private TileFactory tileFactory;
    private TileSpawner tileSpawner;

    private const int RowsCount = 10;
    private const int ColumnsCount = 10;
    private int matchesCount = 3;

    private Vector3 topLeftPointOfPlayingField;
    private Vector3 screenBounds;
    private Vector2 spriteShift;
    private float distanceBetweenTiles;

    private Tile[,] playingField;

    [SerializeField] private float swapSpeed = 5f;
    private bool isSwapping;
    private bool isReverseSwapping;

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
        playingField = new Tile[RowsCount, ColumnsCount];
        screenBounds = MiscTools.GetScreenBounds();
        topLeftPointOfPlayingField = new Vector3(-screenBounds.x + screenBounds.x / 2, screenBounds.y, 0);
        spriteShift = MiscTools.GetSpriteShift(tileSpriteExample);
        distanceBetweenTiles = spriteShift.x * 2;
    }

    private void FillPlayingField()
    {
        IPlayingFieldGenerator playingFieldGenerator = new PlayingFieldGenerator(RowsCount, ColumnsCount);
        TileType[,] randomPlayingField = playingFieldGenerator.GetRandomPlayingField();

        Vector3 pointToInstantiateTile =
            new Vector3(topLeftPointOfPlayingField.x, topLeftPointOfPlayingField.y - spriteShift.y, 0);

        for (int rowNumber = 0; rowNumber < RowsCount; rowNumber++)
        {
            for (int columnNumber = 0; columnNumber < ColumnsCount; columnNumber++)
            {
                TileType randomTileType = randomPlayingField[rowNumber, columnNumber];
                GameObject tile = tileFactory.CreateTile(randomTileType);

                playingField[rowNumber, columnNumber] =
                    Instantiate(tile, pointToInstantiateTile, quaternion.identity, gameObject.transform)
                        .GetComponent<Tile>();

                playingField[rowNumber, columnNumber].TileIndex = new TileIndex(rowNumber, columnNumber);
                playingField[rowNumber, columnNumber].TileType = randomTileType;

                pointToInstantiateTile.x += distanceBetweenTiles;
            }

            pointToInstantiateTile.x = topLeftPointOfPlayingField.x;
            pointToInstantiateTile.y -= distanceBetweenTiles;
        }
    }


    public void SwapTiles(Tile tile0, Tile tile1)
    {
        SwapTilesIndexes(tile0, tile1);
        SwapTilesInArray(tile0, tile1);

        HashSet<Tile> matchedTiles = GetMatchedTiles();

        if (matchedTiles.IsEmpty())
        {
            ReverseSwap(tile0, tile1); //TODO - придумать, как сделать анимацию обратного свапа
            GameEvents.Instance.OnWrongTileClicked();
        }
        else
        {
            SwapTilesNoAnimation(tile0, tile1);
            HideMatchedTiles(matchedTiles);
            DropTiles();
        }
    }

    private void ReverseSwap(Tile tile0, Tile tile1)
    {
        SwapTilesIndexes(tile0, tile1);
        SwapTilesInArray(tile0, tile1);
    }

    private void SwapTilesIndexes(Tile tile0, Tile tile1)
    {
        TileIndex tempIndex = tile0.TileIndex;
        tile0.TileIndex = tile1.TileIndex;
        tile1.TileIndex = tempIndex;
    }

    private void SwapTilesInArray(Tile tile0, Tile tile1)
    {
        playingField[tile0.TileIndex.Row, tile0.TileIndex.Column] = tile0;
        playingField[tile1.TileIndex.Row, tile1.TileIndex.Column] = tile1;
    }

    private void SwapTilesAnimation(Tile tile0, Tile tile1)
    {
        TileMover tile0Mover = tile0.GetComponent<TileMover>();
        TileMover tile1Mover = tile1.GetComponent<TileMover>();

        Vector3 tile0Position = tile0.gameObject.transform.position;
        Vector3 tile1Position = tile1.gameObject.transform.position;

        // Выступает в роли множителя скорости
        float distanceBetweenSwappingTiles =
            Vector3.Distance(tile0Position, tile1Position);

        tile0Mover.Init(tile1Position, distanceBetweenSwappingTiles);
        tile1Mover.Init(tile0Position, distanceBetweenSwappingTiles);
    }

    private void SwapTilesNoAnimation(Tile tile0, Tile tile1)
    {
        var tile0GO = tile0.gameObject;
        var tile1GO = tile1.gameObject;

        Vector3 tempPosition = tile0GO.transform.position;
        tile0GO.transform.position = tile1GO.transform.position;
        tile1GO.transform.position = tempPosition;
    }

    #region Old swap animation (based on coroutine)

    /*private IEnumerator SwapTilesAnimation(Tile tile0, Tile tile1)
       {
           IsSwapping = true;
   
           Vector3 tile0Position = tile0.gameObject.transform.position;
           Vector3 tile1Position = tile1.gameObject.transform.position;
   
           float currentTimeOfSwapping = 0f;
           float distanceBetweenTiles =
               Vector3.Distance(tile0Position, tile1Position); //чтобы скорость свапа зависела от расстояния между плитками
   
           while (tile0.gameObject.transform.position != tile1Position)
           {
               currentTimeOfSwapping += Time.deltaTime;
   
               tile0.gameObject.transform.position =
                   Vector3.MoveTowards(tile0Position, tile1Position,
                       (currentTimeOfSwapping * swapSpeed * distanceBetweenTiles));
   
               tile1.gameObject.transform.position =
                   Vector3.MoveTowards(tile1Position, tile0Position,
                       (currentTimeOfSwapping * swapSpeed * distanceBetweenTiles));
   
               yield return new WaitForEndOfFrame();
           }
   
           IsSwapping = false;
       }*/

    #endregion

    // Возвращает список совпавших плиток, если он пустой - совпадений нет
    private HashSet<Tile> GetMatchedTiles()
    {
        HashSet<Tile> tilesToDelete = new HashSet<Tile>();

        // Проходим слева-направо сверху-вниз. Ищем совпадения в рядах. 
        for (int rowNumber = 0; rowNumber < RowsCount; rowNumber++) // 
        {
            int matchedTilesInARow = 1; // количество совпавших плиток подряд

            for (int columnNumber = 1; columnNumber < ColumnsCount; columnNumber++)
            {
                if (playingField[rowNumber, columnNumber].TileType ==
                    playingField[rowNumber, columnNumber - 1].TileType)
                {
                    matchedTilesInARow++;

                    if (matchedTilesInARow >= matchesCount)
                    {
                        for (int shift = 0; shift < matchedTilesInARow; shift++)
                        {
                            tilesToDelete.Add(playingField[rowNumber, columnNumber - shift]);
                        }
                    }
                }
                else
                {
                    matchedTilesInARow = 1;
                }
            }
        }

        // Проходим сверху-вниз слева-направо. Ищем совпадения в  столбцах. 
        for (int columnNumber = 0; columnNumber < ColumnsCount; columnNumber++) // 
        {
            int matchedTilesInARow = 1; // количество совпавших плиток подряд

            for (int rowNumber = 1; rowNumber < RowsCount; rowNumber++)
            {
                if (playingField[rowNumber, columnNumber].TileType ==
                    playingField[rowNumber - 1, columnNumber].TileType)
                {
                    matchedTilesInARow++;

                    if (matchedTilesInARow >= matchesCount)
                    {
                        for (int shift = 0; shift < matchedTilesInARow; shift++)
                        {
                            tilesToDelete.Add(playingField[rowNumber - shift, columnNumber]);
                        }
                    }
                }
                else
                {
                    matchedTilesInARow = 1;
                }
            }
        }

        foreach (var tile in tilesToDelete)
        {
            Debug.Log($"Index of tiles to delete: {tile.TileIndex.Row} - {tile.TileIndex.Column}");
        }

        if (tilesToDelete.IsEmpty())
        {
            Debug.Log("No matches");
        }

        return tilesToDelete;
    }

    private void HideMatchedTiles(HashSet<Tile> matchedTiles)
    {
        foreach (Tile tile in matchedTiles)
        {
            tile.SetActive(false);
        }
    }

    private void DropTiles()
    {
        // Снизу-вверх слева-направо
        for (int columnNumber = 0; columnNumber < ColumnsCount; columnNumber++) // 
        {
            int counter = 0;
            do
            {
                // Последний ряд не берем, если там остается пустой слот - нужно спавнить новые плитки
                for (int rowNumber = RowsCount - 1; rowNumber > 0; rowNumber--)
                {
                    Tile currentTile = playingField[rowNumber, columnNumber];
                    Tile tileAbove = playingField[rowNumber - 1, columnNumber];

                    if (!currentTile.IsActive)
                    {
                        SwapTilesIndexes(currentTile, tileAbove);
                        SwapTilesInArray(currentTile, tileAbove);
                        SwapTilesNoAnimation(currentTile, tileAbove); // TODO - а без анимации работает, блядь - разобраться
                    }
                }

                counter++;
            } while (counter <= RowsCount);
        }
    }

    private void MoveTile(Tile tile, Vector3 destinationPoint, float speedMultiplier)
    {
        TileMover tileMover = tile.GetComponent<TileMover>();
        tileMover.Init(destinationPoint, speedMultiplier);
    }

    #region Editor tools

#if UNITY_EDITOR

    private void Update()
    {
        //SwapRandomTiles(); можно использовать как заставку -  красиво

        if (Input.GetKeyDown(KeyCode.R))
        {
            SwapRandomTiles();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //StartCoroutine(SwapRandomTilesRoutine());

            foreach (var tile in playingField)
            {
                Destroy(tile.gameObject);
            }

            FillPlayingField();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintTilesTypes();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            GetMatchedTiles();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            DropTiles();
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
        return new Vector2Int(Random.Range(0, RowsCount), Random.Range(0, ColumnsCount));
    }

    private void PrintTilesTypes()
    {
        foreach (var tile in playingField)
        {
            Debug.Log($"Tile type: {tile.TileType}");
        }
    }

#endif

    #endregion
}