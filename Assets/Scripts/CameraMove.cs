using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//And Ui script
public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private CharacterController playerControl;
    [SerializeField]
    private Camera settings;
    [SerializeField]
    private Animator animator;

    //UI
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject win;

    private bool followPlayer = false;

    //Initial position
    private void Start()
    {
        settings.orthographicSize = 20;
        gameObject.transform.position = new Vector3(25, 30, -4);
        playerControl.enabled = false;
    }

    //FollowPlayer
    void LateUpdate()
    {
        if (followPlayer)
        {
            playerControl.enabled = true;
            settings.orthographicSize = 7;
            transform.position = new Vector3(player.transform.position.x, 30, player.transform.position.z);

        }
    }

    //Start camera movement from objective to inicial pose
    public void StartGame()
    {
        mainMenu.SetActive(false);
        StartCoroutine("CutScene");
    }

    //when animation end player can play
    private IEnumerator CutScene()
    {
        animator.SetTrigger("StartAnim");

        yield return new WaitForSeconds(3.5f);

        followPlayer = true;
    }

    //Exit
    public void ExitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;

    }

    //Reload
    public void ReloadGame()
    {
        win.SetActive(false);
        gameOver.SetActive(false);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);

    }
}
