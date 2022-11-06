using Array2DEditor;
using UnityEngine;

namespace Level
{
    [System.Serializable]
    public class Array2DTilesEnum : Array2D<TileTypes>
    {
        [SerializeField]
        CellRowExampleEnum[] cells = new CellRowExampleEnum[Consts.defaultGridSize];

        protected override CellRow<TileTypes> GetCellRow(int idx)
        {
            return cells[idx];
        }
    }

    [System.Serializable]
    public class CellRowExampleEnum : CellRow<TileTypes> { }
}
