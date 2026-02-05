using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Intro")]
    public FadeScreen fade;
    [HideInInspector] public bool isIntro;
    private bool chanceUIActive = false;
    [HideInInspector] public bool introPlayerDead;
    public Transform introSpawnPoint; // posisi ayam di luar kiri
    public Transform introStopPoint;  // posisi ayam berhenti di dalam (kiri)
    public float holdAfterEnter = 1.5f; // setelah ayam masuk, nunggu dulu baru kamera balik

    [Header("Chance")]
    public int maxChance = 3;
    public int currentChance;

    [Header("Edit Hints")]
    public ArrowHintAnimator arrowHint;
    public TileLineAnimator[] tileLines;

    [Header("UI Buttons")]
    public GameObject playButton;
    public GameObject restartButton;

    [Header("Tiles")]
    public TileDragSwap[] draggableTiles; // optional kalau mau set manual

    [Header("Chance UI")]
    [SerializeField] private Animator chanceAnimator; // assign di inspector
    [SerializeField] private float showDelay = 2f;

    public bool isPlayMode;

    public PlayerAutoRunner player;
    public Transform playerStart;
    private Coroutine playCo;

    private void Start()
    {
        isIntro = true;
        StartCoroutine(IntroRoutine());
    }
    IEnumerator IntroRoutine()
    {
        introPlayerDead = false;
        isPlayMode = true;
        SetTileDragEnabled(false);

        if (playButton != null) playButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(false);

        if (arrowHint != null) arrowHint.HideInstant();
        if (tileLines != null)
            foreach (var l in tileLines) if (l != null) l.HideInstant();

        if (fade != null) fade.SetAlpha(1f);
        if (fade != null) yield return fade.FadeIn();

        // Ayam masuk
        if (player != null && introSpawnPoint != null && introStopPoint != null)
            yield return player.IntroEnter(introSpawnPoint.position, introStopPoint.position);

        // Diam 1 detik
        yield return new WaitForSeconds(1f);

        // Jalan mengikuti waypoints → injak trap
        yield return player.FollowWaypointsIntro();

        // JIKA MATI → JALANKAN ANIMASI MATI & TUNGGU SELESAI
        if (introPlayerDead)
        {
            yield return StartCoroutine(PlayerFallIntro());
        }

        // Show UI chance
        yield return ShowChanceUI();

        // Masuk edit mode
        isIntro = false;
        SetEditMode();
        currentChance = maxChance;
    }
    private void Awake()
    {
        Instance = this;
    }
    public void Play()
    {
        if (isPlayMode) return;
        SetPlayMode();
        playCo = StartCoroutine(PlayRoutine());
    }
    public void Restart()
    {
        StopAllCoroutines();
        StartCoroutine(FlashResetToEdit());
    }

    IEnumerator PlayRoutine()
    {
        // tunggu arrow out selesai dulu baru ayam jalan
        if (arrowHint != null)
            yield return arrowHint.PlayOutRoutine();
        // (JANGAN hide arrow di sini, karena sudah di SetPlayMode())
        player.SetPosition(playerStart.position);

        var tiles = FindObjectsOfType<TileDragSwap>()
            .OrderBy(t => t.slotIndex)
            .ToArray();

        List<Vector3> fullPath = new List<Vector3>();
        Vector3 currentPos = player.transform.position;

        foreach (var tile in tiles)
        {
            var multi = tile.GetComponent<TilePathMulti>();
            if (multi != null)
            {
                var chosen = multi.GetPath(currentPos);
                AddPath(chosen, fullPath, ref currentPos);
                continue;
            }

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
        if (arrowHint != null) arrowHint.PlayIn();
        if (tileLines != null)
            foreach (var l in tileLines) if (l != null) l.PlayIn();

        // enable drag tile
        SetTileDragEnabled(true);
    }

    void SetPlayMode()
    {
        isPlayMode = true;
        // UI
        if (playButton != null) playButton.SetActive(false);
        if (restartButton != null) restartButton.SetActive(true);

        if (tileLines != null)
            foreach (var l in tileLines) if (l != null) l.PlayOut();

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
        if (player.IsDead) return;

        StopAllCoroutines();
        StartCoroutine(PlayerFallPlay());
    }
    public IEnumerator PlayerFallIntro()
    {
        player.Die();

        yield return new WaitForSeconds(1.7f); // waktu anim mati

        yield return fade.FlashQuick(() =>
        {
            player.SetPosition(introStopPoint.position);
        });

        player.Revive();
    }
    IEnumerator PlayerFallPlay()
    {
        player.Die();

        yield return new WaitForSeconds(1.7f); // waktu anim mati

        currentChance--;

        if (currentChance > 0)
        {
            // AUTO RESTART
            yield return StartCoroutine(FlashResetToEdit());
        }
        else
        {
            // HABIS → MENU
            yield return fade.FadeOut(0.8f);
            GoToMainMenu();
        }
    }

    IEnumerator FallAndWaitRestart()
    {
        yield break;
    }
    IEnumerator ShowChanceUI()
    {
        if (chanceUIActive) yield break; // sudah aktif, jangan panggil lagi
        chanceUIActive = true;

        if (chanceAnimator != null)
        {
            chanceAnimator.gameObject.SetActive(true); // pastikan aktif
            chanceAnimator.SetBool("inKah", true);
            chanceAnimator.SetBool("outKah", false);

            // reset posisi agar aman
            chanceAnimator.transform.localPosition = Vector3.zero;
        }

        // Hide "Press Enter" text sampai UI siap
        Transform pressEnter = chanceAnimator.transform.Find("PressEnterText");
        if (pressEnter != null) pressEnter.gameObject.SetActive(false);

        // tunggu delay sebelum bisa tekan enter
        yield return new WaitForSeconds(showDelay);

        // sekarang tampilkan tulisan "Press Enter"
        if (pressEnter != null) pressEnter.gameObject.SetActive(true);

        // tunggu player tekan enter
        yield return StartCoroutine(WaitForEnter());

        // trigger anim out sekali saja
        if (chanceAnimator != null)
        {
            chanceAnimator.SetBool("inKah", false);
            chanceAnimator.SetBool("outKah", true);

            // tunggu durasi anim out
            yield return new WaitForSeconds(1f);

            // reset posisi dan langsung hilangkan UI
            chanceAnimator.transform.localPosition = Vector3.zero;
        }

        // sekarang masuk edit mode
        SetEditMode();
    }
    IEnumerator FlashResetToEdit()
    {
        yield return fade.FlashQuick(() =>
        {
            // reset player
            player.SetPosition(playerStart.position);
            player.Revive();

            // reset tile
            var tiles = FindObjectsOfType<TileDragSwap>();
            foreach (var t in tiles)
                t.ResetToStart();
        });

        // balik edit mode
        SetEditMode();
    }

    IEnumerator WaitForEnter()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
            yield return null;
    }
    void GoToMainMenu()
    {
        // contoh
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void PlayerReachPortal()
    {
        if (!isPlayMode) return;

        StopAllCoroutines();
        StartCoroutine(PortalRoutine());
    }
    IEnumerator PortalRoutine()
    {
        player.SetWalking(false);

        yield return fade.FadeOut(1f);

        // pindah ke cutscene
        UnityEngine.SceneManagement.SceneManager.LoadScene("cutscene");
    }
    void HideEditUI() { }
    void ShowEditUI() { }
}
