using TMPro;
using UnityEngine;

public class SelectMainScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private void Start()
    {
        text.text = $"{Screen.mainWindowDisplayInfo.name} \n" +
                    $"{Screen.mainWindowDisplayInfo.width} \n" +
                    $"{Screen.mainWindowDisplayInfo.height} \n";
    }
}
