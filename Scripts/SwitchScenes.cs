using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Entities;

public class SwitchScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene( int i)
    {
        DestroyAllEntities();
        SceneManager.LoadScene(i);
    }

    public void RestartScene()
    {
        DestroyAllEntities();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void DestroyAllEntities()
    {
        var entityManager = World.Active.EntityManager;

        entityManager.DestroyEntity(entityManager.UniversalQuery);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
