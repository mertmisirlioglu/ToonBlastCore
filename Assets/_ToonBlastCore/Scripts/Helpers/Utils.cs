using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers
{
    public class Utils
    {
        public static IEnumerator DelayedAction(Action action, float duration)
        {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }

        public static T RandomEnumValue<T>()
        {
            var values = Enum.GetValues(typeof(T));
            int random = Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }

        public static Dictionary<TileTypes,Tile> CreateEnumToTilesDictionary(Tile[] baseTiles)
        {
            if (baseTiles == null)
            {
                throw new UnityException("Tile list is empty!");
            }

            Dictionary<TileTypes,Tile> dictionary = new Dictionary<TileTypes, Tile>();

            for (int i = 0; i < baseTiles.Length; i++)
            {
                dictionary.Add(baseTiles[i].tileType, baseTiles[i]);
            }

            return dictionary;
        }
    }
}
