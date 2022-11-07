using ManExe.Core;
using ManExe.Entity.StateMachine;
using ManExe.Scriptable_Objects;
using ManExe.World;
using TMPro;
using UnityEngine;

namespace ManExe.UI
{
    public class DebugScreen : MonoBehaviour
    {
        
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject debugMenuPanel;
        [SerializeField] private LayerMask canLookAtLayerMask;// = 193; // Layer 0: Default = 1 + Layer 6: VoxelTerrain = 64  + Layer 7:Interactable = 128
        private Transform _player;
        private GameObject _world;
        private TimeManager _clock;
        private PlayerStateMachine _playerStateMachine;
        private Camera _camera;
        private float _frameRate;
        private float _timer;
        private InputReader _inputReader;
        public static readonly string RedColor = "#b01f0c";
        public static readonly string GreenColor = "#25d918";
        //==========================================
        //======MonoBehaviour Methods
        private void Awake()
        {
            
            _inputReader = Resources.Load<InputReader>("Input/Default Input Reader");
        }

        void Start()
        {
            _player =  GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            _playerStateMachine = _player.GetComponent<PlayerStateMachine>();
            var cameraMainTransform = Camera.main.transform; 
            _camera = cameraMainTransform.gameObject.GetComponent<Camera>();
            
            _world = GameObject.FindGameObjectWithTag("World");
            if(_world != null)
                _clock = _world.GetComponent<TimeManager>();
            debugMenuPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            if(debugMenuPanel.activeInHierarchy== false) return;
            UpdateDebugText();
        }

        private void OnEnable()
        {
            _inputReader.DebugMenuEvent += ManageDebugWindow;
        }

        private void OnDisable()
        {
            _inputReader.DebugMenuEvent -= ManageDebugWindow;
        }
        //==========================================
        //======Methods
        private void ManageDebugWindow()
        {
            debugMenuPanel.gameObject.SetActive(!debugMenuPanel.activeInHierarchy);
            
        }

        private void CalculateFrameRate()
        {
            if (_timer > 1f)
            {
                _frameRate = (int)(1f / Time.unscaledDeltaTime);
                _timer = 0;
            }
            else
                _timer += Time.deltaTime;
        }

        private void UpdateDebugText()
        {
            CalculateFrameRate();
            string debugText = "<size=60>ManExe: Nightmare game.</size>\n\n";

            var framerateColor = _frameRate < 60 ? RedColor : GreenColor;
            debugText += $"Frame rate: <color={framerateColor}>" + _frameRate + "</color> fps \n\n";
            
            if (_player != null)
            {
                var position = _player.position;
                debugText +=
                    $"Position XYZ: {position.x:n2} / {position.y:n2} / {position.z:n2}\n"; // :n2 - interpolated strings
                debugText += "Player Chunk XZ: " + (int)position.x / GameData.ChunkWidth + " / " +
                             (int)position.z / GameData.ChunkWidth + "\n";
            }

            if (_camera != null && _player != null)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));

                if (Physics.Raycast(ray, out var hit, 255, canLookAtLayerMask))
                {
                    var canReach = hit.distance < _playerStateMachine.PlayerReach
                        ? $"<color={GreenColor}>Yes</color>"
                        : $"<color={RedColor}>No</color>";
                    debugText +=
                        $"Looking at XYZ: {hit.point.x:n2} / {hit.point.y:n2} / {hit.point.z:n2}. Distance: {hit.distance:n2}. " +
                        $"Can reach: {canReach}\n\n";
                }
            }

            if (_clock != null)
            {
                debugText += _clock.GetTimeFormatted();
            }

            text.text = debugText;
        }
    }
}