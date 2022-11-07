using System;
using System.Collections;
using UnityEngine;

namespace Helpers
{
    public class Utils
    {
        public static IEnumerator DelayedAction(Action action, float duration)
        {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }
    }
}
