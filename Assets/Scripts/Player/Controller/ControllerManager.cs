using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour
{
    public int maxPlayersAllowed;

    private List<Controller> controllers = new List<Controller>();

    public static ControllerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public void OnPlayerJoin()
    {
        foreach (var controller in FindObjectsOfType<Controller>())
        {
            if (controllers.Contains(controller))
                continue;

            controllers.Add(controller);
            AssignController(controller);
        }
    }

    private void AssignController(Controller controller)
    {
        controller.transform.parent = transform;

        controller.SetIndex(controllers.Count);
        controller.IsAssigned = true;

        FindObjectOfType<PlayerManager>().AddPlayerToGame(controller);
    }
}
