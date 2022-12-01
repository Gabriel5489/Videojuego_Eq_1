using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PantallaCarga : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject panelCarga;

    public void SceneLoad(string escena)
    {
        panelCarga.SetActive(true);
        StartCoroutine(LoadAsync(escena));
    }

    IEnumerator LoadAsync(string escena)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(escena);

        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress;
            yield return null;
        }
    }
}
