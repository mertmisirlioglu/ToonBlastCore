using Array2DEditor;
using UnityEditor;

namespace Level
{
    [CustomPropertyDrawer(typeof(Array2DTilesEnum))]
    public class Array2DExampleEnumDrawer : Array2DEnumDrawer<TileTypes> {}
}
