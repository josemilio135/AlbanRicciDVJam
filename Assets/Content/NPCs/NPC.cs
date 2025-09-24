using FirstGearGames.SmoothCameraShaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class NPC : MonoBehaviour
{
    [SerializeField] TextMeshPro _text;
    [SerializeField] List<GameObject> _explotions = new List<GameObject>();
    public ShakeData _explotionShakeData;
    Player _player;
    float _lifeTime = 30;
    bool _willExplote = false;
    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _text.enabled = false;
    }

    void Update()
    {
        float _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance < 1)
        {
            _text.enabled = true;
            if (Input.GetKey(KeyCode.E))
            {
                _willExplote = true;
            }
           
        }
        else if (!_willExplote)_text.enabled = false;

        if (_willExplote && _lifeTime >= 0)
        {

            _lifeTime -= Time.deltaTime;
            _text.text = _lifeTime.ToString("00");
        }

        if (_lifeTime <= 0 || Input.GetKey(KeyCode.F) && _willExplote)
        {
            GameObject _explotion = Instantiate(_explotions[Random.Range(0, _explotions.Count)], transform.position, Quaternion.identity);
            Destroy(_explotion, 10);
            CameraShakerHandler.Shake(_explotionShakeData);
            Destroy(gameObject);
        }
    }
}
