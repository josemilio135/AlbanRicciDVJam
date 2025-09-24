using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Player _player;

    [SerializeField] Vector3 _offset = new Vector3(0, 5, -2.5f);
    [SerializeField] float _smoothSpeed = 5f;

    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
    }

    void LateUpdate()
    {
        if (_player == null) return;


        Vector3 _dir = _player.transform.position + _offset;
     
        transform.position = Vector3.Lerp(transform.position, _dir, _smoothSpeed * Time.deltaTime);

        transform.LookAt(_player.transform.position);
    }
    
}

