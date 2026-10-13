using System.Collections;
using UnityEngine;

public class PirateCoveController : MonoBehaviour
{
    [SerializeField] private GameObject[] stages;        // The 4 sprites, in order (0 = curtain closed)
    [SerializeField] private float swapIntervalMin = 0.2f; // Min time between hook sprite swaps
    [SerializeField] private float swapIntervalMax = 0.4f; // Max time between hook sprite swaps

    private int currentStage = 0;
    private Coroutine hookRoutine;

    void Start()
    {
        ShowOnly(0); // Start with only the first sprite visible
    }

    /// <summary>
    /// Moves the cove to a new stage. It never goes backwards.
    /// </summary>
    public void GoToStage(int index)
    {
        if (index <= currentStage || index >= stages.Length) return;

        currentStage = index;
        ShowOnly(index);

        // Stage 2 (third sprite) starts the hook animation
        if (index == 2 && hookRoutine == null)
            hookRoutine = StartCoroutine(HookLoop());
    }

    // Alternates between the last two sprites forever to fake a moving hook
    private IEnumerator HookLoop()
    {
        bool showThird = true;
        while (true)
        {
            ShowOnly(showThird ? 2 : 3);
            showThird = !showThird;
            yield return new WaitForSeconds(Random.Range(swapIntervalMin, swapIntervalMax));
        }
    }

    // Enables the chosen stage and disables the rest
    private void ShowOnly(int index)
    {
        for (int i = 0; i < stages.Length; i++)
            stages[i].SetActive(i == index);
    }
}