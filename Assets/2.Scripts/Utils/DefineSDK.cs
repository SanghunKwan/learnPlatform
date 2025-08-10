using UnityEngine;
using System.Collections;
using System;

namespace DefineSDK
{
    public static class DefineSDKUtils
    {
        public static IEnumerator DelayAction(float delaySecond, Action delayedAction)
        {
            yield return new WaitForSeconds(delaySecond);
            delayedAction();
        }
    }

}
