using Unity.Mathematics;
using UnityEngine;

public class TileFactory : MonoBehaviour, ITileFactory
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private TileSpritesDictionary tileSprites = new TileSpritesDictionary();

    private GameObject defaultTile;
    private Vector3 defaultTilePosition;

    private void Start()
    {
        InstantiateDefaultTile();
    }

    // Чтобы не изменять оригинальный префаб, а брать копии с объекта на сцене
    private void InstantiateDefaultTile()
    {
        Vector3 screenBounds = MiscTools.GetScreenBounds();
        defaultTilePosition = new Vector3(screenBounds.x + 1, screenBounds.y + 1, 0);
        defaultTile = Instantiate(tilePrefab, defaultTilePosition, quaternion.identity, gameObject.transform);
    }

    public GameObject CreateTile(TileType tileType)
    {
        GameObject tileGameObject = ConstructTile(tileType);
        return tileGameObject;
    }

    private GameObject ConstructTile(TileType tileType)
    {
        GameObject tileToConstruct = defaultTile;

        Sprite tileSprite = tileSprites[tileType];
        tileToConstruct.GetComponent<SpriteRenderer>().sprite = tileSprite;

        tileToConstruct.GetComponent<Tile>().TileType = tileType;

        return tileToConstruct;
    }
}
