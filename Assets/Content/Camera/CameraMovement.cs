using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Player _player;
    [SerializeField] float _smoothSpeed = 5f;
    [SerializeField] float _lookSmoothSpeed = 2.5f;

    public Vector3 _currentOffset; //quitar luego el public de ambos, ahora es para testeo
    public Vector3 _dialoguePos;

    Transform _lookTarget;
    bool _isDialogueMode = false;

    Vector3 _playerOffset = new Vector3(0, 5, -2.5f);
    Vector3 _dialogueOffset = new Vector3(0, 1.75f, -5f);

    void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        FinishDialogueCam();
    }

    void LateUpdate()
    {
        if (_player == null) return;

        if (_isDialogueMode)
        {
            transform.position = Vector3.Lerp(transform.position, _dialoguePos, _smoothSpeed * Time.deltaTime);

            Quaternion _targetRot = Quaternion.LookRotation(_lookTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, _lookSmoothSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 playerTargetPos = _player.transform.position + _currentOffset;
            transform.position = Vector3.Lerp(transform.position, playerTargetPos, _smoothSpeed * Time.deltaTime);
            transform.LookAt(_player.transform.position);
        }
    }

    public void StartDialogueCam(Transform _newTarget)
    {
        _isDialogueMode = true;
        _dialoguePos = _newTarget.position + _dialogueOffset;
        _lookTarget = _newTarget;
    }
    public void ChangeLookCam(Transform _newLookTarget)
    {
        _lookTarget = _newLookTarget;
    }
    public void FinishDialogueCam()
    {
        _currentOffset = _playerOffset;
        _isDialogueMode = false;
    }

}

