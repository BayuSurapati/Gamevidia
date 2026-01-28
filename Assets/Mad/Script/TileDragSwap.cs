using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider2D))]
public class TileDragSwap : MonoBehaviour
{
    public int slotIndex; // 0..4

    private SlotManager manager;
    private bool dragging;

    private Vector3 startPos;
    private Vector3 dragOffset;

    [Header("Swap Settings")]
    public float swapThreshold = 0.6f;

    [Header("Smooth Move")]
    public float moveSpeed = 10f;

    private Coroutine moveRoutine;
    private SortingGroup sortingGroup;
    private int originalOrder;
    public int dragTopOrder = 999;

    private void Start()
    {
        manager = FindObjectOfType<SlotManager>();
        sortingGroup = GetComponent<SortingGroup>();
        if (sortingGroup != null)
            originalOrder = sortingGroup.sortingOrder;
        SnapToSlotInstant();
    }

    // ===== SNAP =====
    void SnapToSlotInstant()
    {
        transform.position = manager.GetSlotPos(slotIndex);
    }

    public void SnapToSlotSmooth()
    {
        StartMove(manager.GetSlotPos(slotIndex));
    }

    void StartMove(Vector3 target)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(target));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = target;
        moveRoutine = null;
    }

    // ===== INPUT =====
    private void OnMouseDown()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPlayMode)
            return;

        if (manager == null) return;

        // slot 1 dan 5 tidak bisa digeser
        if (!manager.IsMovableSlot(slotIndex))
            return;

        // baru naikin sorting
        if (sortingGroup != null)
        {
            originalOrder = sortingGroup.sortingOrder;
            sortingGroup.sortingOrder = dragTopOrder;
        }

        dragging = true;

        // stop animasi kalau sedang bergerak
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        startPos = transform.position;

        Vector3 mouseWorld = GetMouseWorld();
        dragOffset = transform.position - mouseWorld;
    }

    private void OnMouseDrag()
    {
        if (!dragging) return;

        Vector3 mouseWorld = GetMouseWorld();
        Vector3 pos = mouseWorld + dragOffset;

        // lock Y
        pos.y = startPos.y;
        pos.z = startPos.z;

        transform.position = pos;
    }

    private void OnMouseUp()
    {
        if (!dragging) return;
        dragging = false;

        float dx = transform.position.x - startPos.x;

        if (dx > swapThreshold) TrySwap(+1);
        else if (dx < -swapThreshold) TrySwap(-1);
        else SnapToSlotSmooth();

        // sorting balikin setelah semua proses
        if (sortingGroup != null)
            sortingGroup.sortingOrder = originalOrder;
    }

    // ===== SWAP =====
    void TrySwap(int dir)
    {
        int targetSlot = slotIndex + dir;

        // jangan keluar batas
        if (targetSlot < 0 || targetSlot >= manager.slots.Length)
        {
            SnapToSlotSmooth();
            return;
        }

        // tidak boleh swap ke slot terkunci (slot 1 dan 5)
        if (!manager.IsMovableSlot(targetSlot))
        {
            SnapToSlotSmooth();
            return;
        }

        TileDragSwap other = FindTileAtSlot(targetSlot);
        if (other == null)
        {
            SnapToSlotSmooth();
            return;
        }

        // swap index slot
        int temp = slotIndex;
        slotIndex = other.slotIndex;
        other.slotIndex = temp;
        // smooth move keduanya
        SnapToSlotSmooth();
        other.SnapToSlotSmooth();
    }

    TileDragSwap FindTileAtSlot(int idx)
    {
        TileDragSwap[] all = FindObjectsOfType<TileDragSwap>();
        foreach (var t in all)
        {
            if (t != this && t.slotIndex == idx)
                return t;
        }
        return null;
    }

    Vector3 GetMouseWorld()
    {
        Vector3 m = Input.mousePosition;
        m.z = 10f;
        return Camera.main.ScreenToWorldPoint(m);
    }
}
