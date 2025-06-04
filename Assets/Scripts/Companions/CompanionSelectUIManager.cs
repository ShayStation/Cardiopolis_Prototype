using System.Collections;
using UnityEngine;

public class CompanionSelectUIManager : MonoBehaviour
{
    public GameObject companionCardPrefab;
    public Transform contentParent;

    private void Start()
    {
        StartCoroutine(WaitForManager());
    }

    private IEnumerator WaitForManager()
    {
        // Wait until CompanionManager.Instance exists
        while (CompanionManager.Instance == null)
            yield return null;

        Populate();
    }

    private void Populate()
    {
        foreach (var companion in CompanionManager.Instance.OwnedCompanions)
        {
            GameObject card = Instantiate(companionCardPrefab, contentParent);
            var cardUI = card.GetComponent<CompanionCardUI>();
            cardUI.Setup(companion);
        }
    }
}
