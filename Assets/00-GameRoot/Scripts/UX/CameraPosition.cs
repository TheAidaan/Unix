using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.generateBoard)
        {
            transform.position = new Vector3(88f, 87f, 94.9f);
        }
    }

}
