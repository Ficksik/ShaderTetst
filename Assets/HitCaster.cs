using BigRookGames.Scripts.Build_Panels;
using UnityEngine;

public class HitCaster : MonoBehaviour
{
    private BasicWoodWallController[] _objs;

    private void Start()
    {
        _objs = FindObjectsOfType<BasicWoodWallController>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                foreach (var obj in _objs)
                {
                    if (obj != null)
                    {
                        obj.Hit(hit.point);
                    }
                }
            }
        }
    }
}
