using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private TileSpritesDictionary tileSprites = new TileSpritesDictionary();

    public GameObject GetTileGameObject(TileType tileType)
    {
        GameObject tileGameObject = ConstructTile(tileType);
        return tileGameObject;
    }

    private GameObject ConstructTile(TileType tileType)
    {
        GameObject tileGameObject = tilePrefab;

        Sprite tileSprite = tileSprites[tileType];
        tileGameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;

        tileGameObject.GetComponent<Tile>().TileType = tileType; //TODO - убрать, если не понадобится

        return tileGameObject;
    }
}
