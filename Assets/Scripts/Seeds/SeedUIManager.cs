using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SeedUIManager : MonoBehaviour
{
    public TextMeshProUGUI seedsGrownText;
    public TextMeshProUGUI seedCompleteMessage;
    public Slider growthSlider;

    public SeedGrowthManager seedGrowthManager;

    void OnEnable()
    {
        SeedGrowthManager.OnSeedFullyGrown += HandleSeedGrown;
    }

    void OnDisable()
    {
        SeedGrowthManager.OnSeedFullyGrown -= HandleSeedGrown;
    }

    void Update()
    {
        if (seedGrowthManager != null)
        {
            growthSlider.value = seedGrowthManager.CurrentGrowth;
            seedsGrownText.text = $"Seeds Grown: {seedGrowthManager.SeedsGrown}";
        }
    }

    void HandleSeedGrown()
    {
        StartCoroutine(ShowSeedCompleteMessage());
    }

    IEnumerator ShowSeedCompleteMessage()
    {
        seedCompleteMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        seedCompleteMessage.gameObject.SetActive(false);
    }
}