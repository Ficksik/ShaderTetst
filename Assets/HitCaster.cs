using BigRookGames.Scripts.Build_Panels;
using UnityEngine;

public class HitCaster : MonoBehaviour
{
    [SerializeField] private GameObject _boomEffect;
    [SerializeField] private float _cooldown = 1f;

    private float _currentCoolDown;
    
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (_currentCoolDown > 0)
        {
            _currentCoolDown -= Time.deltaTime;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            var screenPoint = Input.mousePosition ;
            
            Ray ray = _camera.ScreenPointToRay(screenPoint);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var b = hit.transform.gameObject.GetComponent<HitObject>();
                if (b)
                {
                    var objs = FindObjectsOfType<HitObject>();
                    foreach (var obj in objs)
                    {
                        if (obj != null)
                        {
                            Instantiate(_boomEffect,hit.point,Quaternion.identity);
                            obj.Hit(hit.point);
                        }
                    }
                    b.NextStage();
                    _currentCoolDown = _cooldown;
                }
            }
        }
    }
}
