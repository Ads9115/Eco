using UnityEngine;

public class HexCell : MonoBehaviour
{
    public int x;
    public int z;

    public bool isTree;
    public bool isWater;
    public bool isLand;
    public bool isGrass;


    public HexCell[] neighbors = new HexCell[6];
}