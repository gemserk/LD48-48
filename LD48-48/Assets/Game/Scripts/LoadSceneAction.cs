using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class LoadSceneAction : MonoBehaviour
    {
        public string sceneName;
        
        public void Execute()
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}