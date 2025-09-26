using FirstGearGames.SmoothCameraShaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class NPC : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshPro _textNPC;
    TextMeshPro _textPlayer;
    [Header("Explotions")]
    [SerializeField] List<GameObject> _explotions = new List<GameObject>();
    public ShakeData _explotionShakeData;
    [Header("Dialogue")]
    [SerializeField] List<DialogueBlock> _dialogueBlocks = new List<DialogueBlock>();
    int _blockIndex = 0;
    int _lineIndex = 0;
    bool _isInDialogue = false;

    Player _player;
    CameraMovement _camera;

    float _lifeTime = 30;
    bool _willExplote = false;
    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _camera = FindAnyObjectByType<CameraMovement>();
        _textPlayer = _player.GetComponentInChildren<TextMeshPro>();
        _textPlayer.enabled = false;
        _textNPC.enabled = false;
    }

    void Update()
    {
        float _distance = Vector3.Distance(transform.position, _player.transform.position);

        if (_distance < 1 && !_isInDialogue && !_willExplote)
        {
            _textNPC.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                _isInDialogue = true;
                if (_blockIndex == 0) _camera.StartDialogueCam(transform);
                _lineIndex = 0;
                ShowLine();
            }
        }
        else if (_isInDialogue && Input.GetKeyDown(KeyCode.E)) NextLine();
        else if (!_willExplote && !_isInDialogue) _textNPC.enabled = false;


        if (_willExplote && _lifeTime > 0)
        {

            _lifeTime -= Time.deltaTime;
            _textNPC.text = _lifeTime.ToString("00");
        }

        if (_lifeTime <= 0 || Input.GetKey(KeyCode.F) && _willExplote)
        {
            GameObject _explotion = Instantiate(_explotions[Random.Range(0, _explotions.Count)], transform.position, Quaternion.identity);
            Destroy(_explotion, 10);
            CameraShakerHandler.Shake(_explotionShakeData);
            Destroy(gameObject);
        }
        if (_textPlayer.enabled)
        {
            Vector3 _cameraPos = _camera.transform.position - _textPlayer.transform.position;
            _cameraPos.y = 0;
            _textPlayer.transform.rotation = Quaternion.LookRotation(-_cameraPos);
        }

    }
    void NextLine()
    {
        _lineIndex++;

        if (_lineIndex < _dialogueBlocks[_blockIndex]._lines.Count)
        {
            ShowLine();
        }
        else
        {
            _blockIndex++;
            _lineIndex = 0;

            if (_blockIndex < _dialogueBlocks.Count) ShowLine();
            else
            {
                _isInDialogue = false;
                _willExplote = true;
                _textNPC.enabled = true;
                _textPlayer.enabled = false;
                _camera.FinishDialogueCam();
            }
        }
    }
    void ShowLine()
    {
        DialogueBlock currentBlock = _dialogueBlocks[_blockIndex];

        string _line = _dialogueBlocks[_blockIndex]._lines[_lineIndex];

        if (currentBlock.speaker == DialogueBlock.Speaker.NPC)
        {
            _textPlayer.enabled = false;
            _textNPC.enabled = true;
            _textNPC.text =  _line;
            _camera.ChangeLookCam(transform);
        }
        else //player
        {
            _textNPC.enabled = false;
            _textPlayer.enabled = true;
            _camera.ChangeLookCam(_player.transform);
            _textPlayer.text = _line;
        }
    }

    [System.Serializable]
    public class DialogueBlock
    {
        public enum Speaker { NPC, Player }
        public Speaker speaker;

        [TextArea]
        public List<string> _lines = new List<string>();
    }
}
