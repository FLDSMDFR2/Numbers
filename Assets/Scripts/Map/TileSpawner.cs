using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject TilePrefab;

    public virtual void SpawnTile(MapSector sector)
    {
        if (TilePrefab == null) return;

        var center = (sector.SectorSizeX / 2);
        var obj = Instantiate(TilePrefab, new Vector3((sector.SectorKey.x * sector.SectorSizeX) + center, 0f, (sector.SectorKey.y * sector.SectorSizeY) + center), Quaternion.identity, this.gameObject.transform);

        var tilecript = obj.GetComponent<Tile>();
        if (tilecript == null) return;

        tilecript.SetSector(sector);
    }
}
