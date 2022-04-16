using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
    }
}
