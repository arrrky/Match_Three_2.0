using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<Sprite> tilesSprites = new List<Sprite>();

    public GameObject GetTileGameObject(TileType tileType)
    {
        GameObject tileGameObject = ConstructTile(tileType);
        return tileGameObject;
    }

    private GameObject ConstructTile(TileType tileType)
    {
        GameObject tileGameObject = tilePrefab;

        Sprite tileSprite = tilesSprites.Find(sprite => sprite.name == tileType.ToString().ToLower());
        tileGameObject.GetComponent<SpriteRenderer>().sprite = tileSprite;

        tileGameObject.GetComponent<Tile>().tileType = tileType; //TODO - убрать, если не понадобится

        return tileGameObject;
    }
}
