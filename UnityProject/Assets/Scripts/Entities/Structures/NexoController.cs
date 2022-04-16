using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NexoController : EntityController
{
    protected override void Die()
    {
        base.Die();
        
        GameController.Instance.StopScore();
        FindObjectOfType<CameraMovement>().FocusCenter();
        StartCoroutine(GameOverAfterSeconds());
    }
    
    private IEnumerator GameOverAfterSeconds()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOver");
    }
}
