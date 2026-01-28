using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public Transform[] slots; // isi 5 slot dari inspector

    public bool IsMovableSlot(int slotIndex)
    {
        // yang bisa digeser hanya slot 2,3,4
        // index array: 0..4
        return slotIndex >= 1 && slotIndex <= 3;
    }

    public Vector3 GetSlotPos(int slotIndex)
    {
        return slots[slotIndex].position;
    }
}
