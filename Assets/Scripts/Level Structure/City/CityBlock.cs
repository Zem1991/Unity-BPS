using BPS.Population;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlock : MonoBehaviour {
    public CityBlockUsage cityBlockType;
    public PurposeZoning purposeZoning;
    public EconomicalZoning economicalZoning;

    public List<Tile> allTiles = new List<Tile>();
    public List<Tile> allBuildableTiles = new List<Tile>();
    public bool isSquare;
    public bool isFlat;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        foreach (var item in allTiles)
        {
            Gizmos.DrawWireCube(item.transform.position, item.transform.lossyScale);
        }
        Gizmos.color = Color.yellow;
        foreach (var item in allBuildableTiles)
        {
            Gizmos.DrawWireCube(item.transform.position, item.transform.lossyScale * 2);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    public void centralizePosition()
    {
        Vector3 avgPos = Vector3.zero;
        foreach (Tile item in allTiles)
        {
            avgPos += item.transform.position;
        }
        avgPos /= allTiles.Count;
        transform.position = avgPos;
    }

    

    //public CityObject createCityBuilding(float tileSize, CityObject cityBuilding, GameObject parent, bool southFacingFirst,
    //    List<CityObject> otherBuildingsToKeepDistance, float minimumDistanceToKeepFromOthers, float minimumDistanceToKeepFromSame)
    //{
    //    int possibilities = allBuildableTiles.Count;
    //    foreach (Tile buildableTile in allBuildableTiles)
    //    {
    //        //First we check against all buildings listed in 'buildingsToAvoid'. This list may or may not contain the 'cityBuilding' we desire to instantiate.
    //        Collider[] colliders_MinDist = Physics.OverlapSphere(buildableTile.transform.position, minimumDistanceToKeepFromOthers);
    //        foreach (Collider col in colliders_MinDist)
    //        {
    //            if (col.gameObject.GetComponent<CityObject>())
    //            {
    //                foreach (CityObject bldg in otherBuildingsToKeepDistance)
    //                {
    //                    if (col.gameObject.GetComponent<CityObject>().levelObjectName == bldg.levelObjectName)        //TODO DONT CHECK USING NAMES!
    //                    {
    //                        Debug.Log("AVOIDED BY: minimumDistance");
    //                        possibilities--;
    //                    }
    //                }
    //            }
    //        }

    //        //Then we check against the desired 'cityBuilding', if we should keep even more distance from any copies of it.
    //        Collider[] colliders_MinDistSame = Physics.OverlapSphere(buildableTile.transform.position, minimumDistanceToKeepFromSame);
    //        foreach (Collider col in colliders_MinDistSame)
    //        {
    //            if (col.gameObject.GetComponent<CityObject>())
    //            {
    //                if (col.gameObject.GetComponent<CityObject>().levelObjectName == cityBuilding.levelObjectName)        //TODO DONT CHECK USING NAMES!
    //                {
    //                    Debug.Log("AVOIDED BY: minimumDistanceIfSameObject");
    //                    possibilities--;
    //                }
    //            }
    //        }
    //    }

    //    //If we still have possibilities of spawning the desired CityBuilding in any of the buildable tiles, then we go ahead and instantiate it finally.
    //    if (possibilities > 0)
    //        return createCityBuilding(tileSize, cityBuilding, parent, southFacingFirst);
    //    else
    //        return null;
    //}

    //public List<Tile> getFreeBuildableTiles()
    //{
    //    List<Tile> tiles = new List<Tile>(allBuildableTiles);
    //    foreach (Tile item in allBuildableTiles)
    //    {
    //        if (item.mapFeature || item.cityBuilding)
    //            tiles.Remove(item);
    //    }
    //    return tiles;
    //}

    //public List<Tile> getUsedBuildableTiles()
    //{
    //    List<Tile> tiles = new List<Tile>(allBuildableTiles);
    //    foreach (Tile item in allBuildableTiles)
    //    {
    //        if (!item.mapFeature && !item.cityBuilding)
    //            tiles.Remove(item);
    //    }
    //    return tiles;
    //}

    //public void checkSizeAndShape(float tileSize, float cityBlockSize, out bool correctSize, out bool correctShape)
    //{
    //    correctSize = false;
    //    correctShape = false;

    //    float minX = float.NaN;
    //    float maxX = float.NaN;
    //    float minZ = float.NaN;
    //    float maxZ = float.NaN;
    //    float minY = float.NaN;
    //    float maxY = float.NaN;

    //    foreach (Tile t in allTiles)
    //    {
    //        if (float.IsNaN(minX) || float.IsNaN(maxX))
    //        {
    //            minX = t.transform.position.x;
    //            maxX = t.transform.position.x;
    //        }
    //        else
    //        {
    //            if (minX > t.transform.position.x)
    //                minX = t.transform.position.x;
    //            else if (maxX < t.transform.position.x)
    //                maxX = t.transform.position.x;
    //        }

    //        if (float.IsNaN(minZ) || float.IsNaN(maxZ))
    //        {
    //            minZ = t.transform.position.z;
    //            maxZ = t.transform.position.z;
    //        }
    //        else
    //        {
    //            if (minZ > t.transform.position.z)
    //                minZ = t.transform.position.z;
    //            else if (maxZ < t.transform.position.z)
    //                maxZ = t.transform.position.z;
    //        }

    //        if (float.IsNaN(minY) || float.IsNaN(maxY))
    //        {
    //            minY = t.transform.position.y;
    //            maxY = t.transform.position.y;
    //        }
    //        else
    //        {
    //            if (minY > t.transform.position.y)
    //                minY = t.transform.position.y;
    //            else if (maxY < t.transform.position.y)
    //                maxY = t.transform.position.y;
    //        }
    //    }

    //    if (((maxX - minX + tileSize) / tileSize == cityBlockSize) &&
    //        ((maxZ - minZ + tileSize) / tileSize == cityBlockSize))
    //        correctSize = true;
    //    if (maxY == minY)
    //        correctShape = true;
    //}

    //public bool checkLandUsage(int landUsageThreshold)
    //{
    //    float used = (float)getUsedBuildableTiles().Count / allBuildableTiles.Count;
    //    float minimum = (float)landUsageThreshold / 100;
    //    return (used >= minimum);
    //}
}
