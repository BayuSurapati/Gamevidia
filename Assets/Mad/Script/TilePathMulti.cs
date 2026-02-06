using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathMulti : MonoBehaviour
{
    public Transform[] topPath;
    public Transform[] bottomPath;

    [Tooltip("Jika player masuk tile dengan y >= splitY -> pakai topPath")]
    public float splitY = 0f;

    public Transform[] GetPath(Vector3 playerPos)
    {
        if (playerPos.y >= splitY && topPath != null && topPath.Length > 0)
            return topPath;

        return bottomPath;
    }
}
