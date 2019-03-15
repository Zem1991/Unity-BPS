using BPS;
using BPS.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zem.Directions;

public static class MapStructureTools
{
    public static float CompareTileElevations(Tile tile, Tile other, float tileHeight)
    {
        float value = tile.getElevation() - other.getElevation();
        value = (value / tileHeight) - (value % tileHeight);
        return value;
    }

    public static void Tile_DefineMeshVertices(Tile t, float tileSize, float tileHeight, float cliffHeight)
    {
        t.meshVertices = new Vector3[5];    //new Vector3[9];
        t.heightOfLowestVertex = 0;

        if (t.forcedFlatness)
            return;

        float v0 = 0;// (v1 + v3 + v5 + v7) / 4F;

        //NOT USING ANYMORE!
        //float v2 = Tile_CalculateVertexHeight_AtSide(t, tileSize, tileHeight, cliffHeight,
        //    t.groundNeighbours[(int)Directions.N]);
        //float v4 = Tile_CalculateVertexHeight_AtSide(t, tileSize, tileHeight, cliffHeight,
        //    t.groundNeighbours[(int)Directions.W]);
        //float v6 = Tile_CalculateVertexHeight_AtSide(t, tileSize, tileHeight, cliffHeight,
        //    t.groundNeighbours[(int)Directions.S]);
        //float v8 = Tile_CalculateVertexHeight_AtSide(t, tileSize, tileHeight, cliffHeight,
        //    t.groundNeighbours[(int)Directions.E]);

        float v1 = Tile_CalculateVertexHeight_AtCorner(t, tileSize, tileHeight, cliffHeight,
            t.groundNeighbours[(int)Directions.NE],
            t.groundNeighbours[(int)Directions.E],
            t.groundNeighbours[(int)Directions.N]);
        float v3 = Tile_CalculateVertexHeight_AtCorner(t, tileSize, tileHeight, cliffHeight,
            t.groundNeighbours[(int)Directions.NW],
            t.groundNeighbours[(int)Directions.N],
            t.groundNeighbours[(int)Directions.W]);
        float v5 = Tile_CalculateVertexHeight_AtCorner(t, tileSize, tileHeight, cliffHeight,
            t.groundNeighbours[(int)Directions.SW],
            t.groundNeighbours[(int)Directions.W],
            t.groundNeighbours[(int)Directions.S]);
        float v7 = Tile_CalculateVertexHeight_AtCorner(t, tileSize, tileHeight, cliffHeight,
            t.groundNeighbours[(int)Directions.SE],
            t.groundNeighbours[(int)Directions.S],
            t.groundNeighbours[(int)Directions.E]);

        t.meshVertices[0] = new Vector3(0, v0, 0);                          //CENTER
        t.meshVertices[1] = new Vector3(tileSize / 2, v1, tileSize / 2);    //NE
        //t.meshVertices[2] = new Vector3(0, v2, tileSize / 2);               //N
        t.meshVertices[2] = new Vector3(-tileSize / 2, v3, tileSize / 2);   //NW
        //t.meshVertices[4] = new Vector3(-tileSize / 2, v4, 0);              //W
        t.meshVertices[3] = new Vector3(-tileSize / 2, v5, -tileSize / 2);  //SW
        //t.meshVertices[6] = new Vector3(0, v6, -tileSize / 2);              //S
        t.meshVertices[4] = new Vector3(tileSize / 2, v7, -tileSize / 2);   //SE
        //t.meshVertices[8] = new Vector3(tileSize / 2, v8, 0);               //E

        bool notFlat = true;
        foreach (var item in t.meshVertices)
        {
            if (item.y != 0)
            {
                notFlat = false;
                break;
            }
        }
        if (notFlat)
        {
            t.directionOfMiddleVertex = 0;
            t.heightOfLowestVertex = 0;
        }
        else
        {
            //t.heightOfLowestVertex = -1 * Mathf.Min(v0, v1, v2, v3, v4, v5, v6, v7, v8);
            t.heightOfLowestVertex = -1 * Mathf.Min(v0, v1, v3, v5, v7);
        }
    }

    public static void Tile_DefineMeshTriangles(Tile t)
    {
        //RMEMBER: Clockwise determines which side is visible!
        t.meshTriangles = new int[]
        {
            //0,2,1,
            //0,3,2,
            //0,4,3,
            //0,5,4,
            //0,6,5,
            //0,7,6,
            //0,8,7,
            //0,1,8
            0,2,1,
            0,3,2,
            0,4,3,
            0,1,4
        };
    }

    public static void Tile_DefineMeshUVs(Tile t)
    {
        //REMEMBER: 0,0 is bottom left and 1,1 is top right!
        t.meshUVs = new Vector2[]
        {
            new Vector2(0.5F,   0.5F),
            //new Vector2(1,      0.5F),
            new Vector2(1,      1),
            //new Vector2(0.5F,   1),
            new Vector2(0,      1),
            //new Vector2(0,      0.5F),
            new Vector2(0,      0),
            //new Vector2(0.5F,   0),
            new Vector2(1,      0)
        };
    }

    //private static float Tile_CalculateVertexHeight_AtSide(Tile tile, float tileSize, float cliffHeight,
    //    Tile opposite)
    //{
    //    if (tile.directionOfMiddleVertex == 0 ||
    //        opposite == null ||
    //        tile.mapFeature != null || 
    //        tile.building != null)
    //        return 0;

    //    float differenceToOpposite = 
    //        tile.getElevation() - opposite.getElevation();
    //    bool opposite_isCliff =
    //        (differenceToOpposite >= cliffHeight) ||
    //        (differenceToOpposite <= -cliffHeight);

    //    if (opposite_isCliff)
    //        return 0;

    //    return -differenceToOpposite;
    //}

    //private static float Tile_CalculateVertexHeight_AtSide(Tile tile, float tileSize, float tileHeight, float cliffHeight, 
    //    Tile opposite)
    //{
    //    //Current tile must be non-flat to receive vertex values different from zero.
    //    if (tile.forcedFlatness || tile.directionOfMiddleVertex == 0) return 0;
    //    //Current tile cannot have something placed above it.
    //    if (tile.mapFeature != null || tile.building != null) return 0;
    //    //Opposite tile must exist.
    //    if (!opposite) return 0;

    //    float elev = CompareTileElevations(tile, opposite, tileHeight);
    //    float signTile = Mathf.Sign(tile.directionOfMiddleVertex);
    //    float signOpposite = Mathf.Sign(opposite.directionOfMiddleVertex);

    //    if (elev > 0)
    //    {
    //        if (signTile == 1 
    //            //&&
    //            //tile.getElevation() == opposite.getElevation() - opposite.directionOfMiddleVertex
    //            )
    //        {
    //            //if (tile.getElevation() != opposite.getElevation() - opposite.directionOfMiddleVertex)
    //            return -tile.directionOfMiddleVertex;
    //        }

    //    }
    //    if (elev < 0)
    //    {
    //        if (signTile == -1 
    //            //&&
    //            //tile.getElevation() == opposite.getElevation() - opposite.directionOfMiddleVertex
    //            )
    //        {
    //            //if (tile.getElevation() != opposite.getElevation() - opposite.directionOfMiddleVertex)
    //            return -tile.directionOfMiddleVertex;
    //        }
    //    }
    //    return 0;


    //    //float differenceToOpposite =
    //    //    (opposite ?
    //    //    tile.getElevation() - opposite.getElevation() : 0);
    //    //bool opposite_isCliff =
    //    //    (differenceToOpposite >= cliffHeight) ||
    //    //    (differenceToOpposite <= -cliffHeight);

    //    //if (opposite_isCliff)
    //    //    return 0;


    //}

    private static float Tile_CalculateVertexHeight_AtCorner(Tile tile, float tileSize, float tileHeight, float cliffHeight,
        Tile opposite, Tile lateralA, Tile lateralB)
    {
        //Current tile must be non-flat to receive vertex values different from zero.
        if (tile.forcedFlatness || tile.directionOfMiddleVertex == 0) return 0;
        //Current tile cannot have something placed above it.
        if (tile.mapFeature != null || tile.building != null) return 0;
        //Opposite tile must exist.
        if (!opposite) return 0;

        float elev = CompareTileElevations(tile, opposite, tileHeight);
        float signTile = Mathf.Sign(tile.directionOfMiddleVertex);
        float signOpposite = Mathf.Sign(opposite.directionOfMiddleVertex);

        if (elev > 0)
        {
            if (signTile == 1
                //&&
                //tile.getElevation() == opposite.getElevation() - opposite.directionOfMiddleVertex
                )
            {
                //if (tile.getElevation() != opposite.getElevation() - opposite.directionOfMiddleVertex)
                return -tile.directionOfMiddleVertex;
            }

        }
        if (elev < 0)
        {
            if (signTile == -1
                //&&
                //tile.getElevation() == opposite.getElevation() - opposite.directionOfMiddleVertex
                )
            {
                //if (tile.getElevation() != opposite.getElevation() - opposite.directionOfMiddleVertex)
                return -tile.directionOfMiddleVertex;
            }
        }
        return 0;

        //!!

        float differenceToOpposite =
            (opposite ?
            tile.getElevation() - opposite.getElevation() : 0);
        bool opposite_isCliff =
            (differenceToOpposite >= cliffHeight) ||
            (differenceToOpposite <= -cliffHeight);

        float differenceToLateralA =
            (lateralA ?
            tile.getElevation() - lateralA.getElevation() : 0);
        bool lateralA_isCliff =
            (differenceToLateralA >= cliffHeight) ||
            (differenceToLateralA <= -cliffHeight);

        float differenceToLateralB =
            (lateralB ?
            tile.getElevation() - lateralB.getElevation() : 0);
        bool lateralB_isCliff =
            (differenceToLateralB >= cliffHeight) ||
            (differenceToLateralB <= -cliffHeight);

        if (!opposite)
        {
            if (tile.directionOfMiddleVertex != 0)
            {
                if (lateralA && !lateralB)
                    return -differenceToLateralA;
                if (lateralB && !lateralA)
                    return -differenceToLateralB;
            }
            return 0;
        }

        if (opposite_isCliff || lateralA_isCliff || lateralB_isCliff)
            return 0;

        if (differenceToLateralA == differenceToLateralB)
        {
            if (differenceToOpposite == differenceToLateralA)
            {
                if (differenceToOpposite != 0)
                    return -differenceToOpposite;
            }
            else
            {
                if (lateralA.directionOfMiddleVertex == 0)
                    return -tile.directionOfMiddleVertex;
            }
        }
        else
        {
            if (tile.directionOfMiddleVertex != 0)
            {
                if (differenceToOpposite == differenceToLateralA ||
                    differenceToOpposite == differenceToLateralB)
                    return -differenceToOpposite;
            }
        }
        return 0;

        ////if (differenceToLateralA != differenceToLateralB)
        ////{
        ////    if (differenceToLateralA == 0)
        ////        return -differenceToLateralB;
        ////    if (differenceToLateralB == 0)
        ////        return -differenceToLateralA;
        ////}
        //if (differenceToLateralA != differenceToLateralB)
        //{
        //    if (differenceToLateralA == 0 || differenceToLateralB == 0)
        //        return -tile.directionOfMiddleVertex;

        //    if (Mathf.Sign(differenceToLateralA) == Mathf.Sign(differenceToLateralB))
        //        return -tile.directionOfMiddleVertex;
        //}
        //else if (differenceToLateralA != 0)
        //{
        //    if (differenceToOpposite != 0)
        //        return vertexToOpposite;
        //    else
        //        return -tile.directionOfMiddleVertex;
        //}
        //return 0;
    }

    public static bool TileSide_IdentifyVertexes(
        Vector3 tileVertexA, Vector3 tileVertexB, float tileElevation,
        float lowestHeightPossible,
        out Vector3 topA, out Vector3 topB, out Vector3 bottomA, out Vector3 bottomB)
    {
        topA = new Vector3();
        topB = new Vector3();
        bottomA = new Vector3();
        bottomB = new Vector3();

        float tA, tB, bA, bB;
        tA = tileElevation + tileVertexA.y;
        tB = tileElevation + tileVertexB.y;
        bA = lowestHeightPossible;
        bB = lowestHeightPossible;

        if (tA > bA || tB > bB)
        {
            Vector3 fixA = new Vector3();
            Vector3 fixB = new Vector3();
            fixA.y = tA - bA;
            fixB.y = tB - bB;

            topA = tileVertexA;
            topB = tileVertexB;
            bottomA = tileVertexA - fixA;
            bottomB = tileVertexB - fixB;

            return true;
        }
        return false;
    }

    public static bool TileSide_IdentifyVertexes(
        Vector3 tileVertexA, Vector3 tileVertexB, float tileElevation,
        Vector3 otherTileVertexA, Vector3 otherTileVertexB, float otherTileElevation,
        out Vector3 topA, out Vector3 topB, out Vector3 bottomA, out Vector3 bottomB)
    {
        topA = new Vector3();
        topB = new Vector3();
        bottomA = new Vector3();
        bottomB = new Vector3();

        float tA, tB, bA, bB;
        tA = tileElevation + tileVertexA.y;
        tB = tileElevation + tileVertexB.y;
        bA = otherTileElevation + otherTileVertexA.y;
        bB = otherTileElevation + otherTileVertexB.y;

        if (tA > bA || tB > bB)
        {
            Vector3 fixA = new Vector3();
            Vector3 fixB = new Vector3();
            fixA.y = tA - bA;
            fixB.y = tB - bB;

            topA = tileVertexA;
            topB = tileVertexB;
            bottomA = tileVertexA - fixA;
            bottomB = tileVertexB - fixB;

            return true;
        }
        return false;
    }
}
