using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    protected virtual void Awake(){
        DontDestroyOnLoad(gameObject);
    }
}
