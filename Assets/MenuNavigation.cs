
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button firstSelectedButton;

    private void OnEnable()
    {
        StartCoroutine(SelectButton());
    }

    private IEnumerator SelectButton()
    {
        // Aguarda a atualização da interface.
        yield return null;

        if (EventSystem.current == null ||
            firstSelectedButton == null)
            yield break;

        // Limpa a seleção anterior.
        EventSystem.current.SetSelectedGameObject(null);

        // Seleciona o primeiro botão.
        EventSystem.current.SetSelectedGameObject(
            firstSelectedButton.gameObject
        );
    }
}
