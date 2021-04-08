using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MiscTools 
{
    private static Array tileTypeEnumValues = Enum.GetValues(typeof(TileType));

    /// <summary>
    /// По сути возвращает Vector3 координаты правого верхнего угла.
    /// Но учитывая, что центр экрана в нулевых координатах, дает представление о размере сцены в целом.
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetScreenBounds()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenVector = new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(screenVector);
    }

    /// <summary>
    /// Узнаем расстояние смещения для того, чтобы префаб со спрайтом, который мы инстанируем, не вылезал за сцену
    /// </summary>
    /// <param name="prefabWithSprite"></param>
    /// <returns></returns>
    public static Vector2 GetSpriteShift(GameObject prefabWithSprite)
    {
        SpriteRenderer spriteRenderer = prefabWithSprite.GetComponent<SpriteRenderer>();
        Vector3 spriteRendererBoundsExtents = spriteRenderer.bounds.extents;
        return new Vector2(spriteRendererBoundsExtents.x, spriteRendererBoundsExtents.y);
    }    
    
    public static Vector2 GetSpriteShift(Sprite sprite)
    {
        Vector3 spriteBoundsExtents = sprite.bounds.extents;
        return new Vector2(spriteBoundsExtents.x, spriteBoundsExtents.y);
    }    
    
    public static TileType GetRandomTypeOfTile()
    {
        TileType randomTileType = (TileType)tileTypeEnumValues.GetValue(Random.Range(1, tileTypeEnumValues.Length));
        return randomTileType;
    }
}
