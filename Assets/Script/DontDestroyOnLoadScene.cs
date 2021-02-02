using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadScene : MonoBehaviour
{

    public GameObject[] objetcs;

    public static DontDestroyOnLoadScene instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DontDestroyOnLoadScene dans la scène");
            return;
        }

        instance = this;

        foreach (var element in objetcs)
        {
            DontDestroyOnLoad(element);
        }
    }


    public void RemoveFromDontDestroyOnLoad()
    {
        foreach (var element in objetcs)
        {
            SceneManager.MoveGameObjectToScene(element, SceneManager.GetActiveScene());
        }
    }

}
