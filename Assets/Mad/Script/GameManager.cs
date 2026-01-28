using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Edit Hints")]
    public ArrowHintAnimator arrowHint;
    public TileLineAnimator[] tileLines;

    [Header("UI Buttons")]
    public GameObject playButton;
    public GameObject restartButton;

    [Header("Tiles")]
    public TileDragSwap[] draggableTiles; // optional kalau mau set manual


    public bool isPlayMode;

    public PlayerAutoRunner player;
    public Transform playerStart;

    private void Start()
    {
        SetEditMode();
    }
    private void Awake()
    {
        Instance = this;
    }
    public void Play()
    {
        if (isPlayMode) return;

        SetPlayMode();
        StartCoroutine(PlayRoutine());
    }
    public void Restart()
    {
        StopAllCoroutines(); // stop movement coroutine
        player.SetPosition(playerStart.position);

        SetEditMode();
    }
    IEnumerator PlayRoutine()
    {
        player.SetPosition(playerStart.position);

        var tiles = FindObjectsOfType<TileDragSwap>()
            .OrderBy(t => t.slotIndex)
            .ToArray();

        List<Vector3> fullPath = new List<Vector3>();
        Vector3 currentPos = player.transform.position;

        foreach (var tile in tiles)
        {
            // Multi path
            var multi = tile.GetComponent<TilePathMulti>();
            if (multi != null)
            {
                var chosen = multi.GetPath(currentPos);
                AddPath(chosen, fullPath, ref currentPos);
                continue;
            }

            // Single path
            var single = tile.GetComponent<TilePath>();
            if (single != null)
            {
                AddPath(single.path, fullPath, ref currentPos);
                continue;
            }

            Debug.LogWarning($"Tile {tile.name} tidak punya TilePath / TilePathMulti");
        }

        yield return player.FollowPath(fullPath);

        isPlayMode = false;
        ShowEditUI();
    }
    void AddPath(Transform[] wps, List<Vector3> list, ref Vector3 currentPos)
    {
        if (wps == null) return;

        foreach (var wp in wps)
        {
            if (wp == null) continue;
            list.Add(wp.position);
            currentPos = wp.position;
        }
    }
    void SetEditMode()
    {
        isPlayMode = false;

        // UI
        if (playButton != null) playButton.SetActive(true);
        if (restartButton != null) restartButton.SetActive(false);

        // hints
        if (arrowHint != null) arrowHint.ShowInstant();
        if (tileLines != null)
            foreach (var l in tileLines) if (l != null) l.ShowInstant();

        // enable drag tile
        SetTileDragEnabled(true);
    }

    void SetPlayMode()
    {
        isPlayMode = true;

        // UI
        if (playButton != null) playButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(true);

        // hints hide anim
        if (arrowHint != null) arrowHint.PlayHideAnimation();
        if (tileLines != null)
            foreach (var l in tileLines) if (l != null) l.PlayHideAnimation();

        // disable drag tile
        SetTileDragEnabled(false);
    }

    void SetTileDragEnabled(bool enabled)
    {
        TileDragSwap[] tiles = draggableTiles != null && draggableTiles.Length > 0
            ? draggableTiles
            : FindObjectsOfType<TileDragSwap>();

        foreach (var t in tiles)
        {
            if (t == null) continue;
            t.enabled = enabled;
        }
    }
    public void PlayerFall()
    {
        if (!isPlayMode) return;

        StopAllCoroutines();

        // optional animasi jatuh
        StartCoroutine(FallAndWaitRestart());
    }

    IEnumerator FallAndWaitRestart()
    {
        // kalau kamu mau animasi jatuh:
        // yield return player.FallDown();

        // tetap di play mode tapi player berhenti
        // restart button tetap aktif
        yield break;
    }

    void HideEditUI() { }
    void ShowEditUI() { }
}
