using Projectiles;
using UnityEngine;


public class InputController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Camera _mainCamera;
    private ProjectileOrbitalController _orbitalController;
    private RhythmTimer _rTimer;
    private Player _player;
    private static readonly string SPrevBulletKey = "q";
    private static readonly string SNextBulletKey = "e";

    [SerializeField] private float speed = 5.0f;
    [field: SerializeField] public bool IsRhythmActive { get; private set; } = true;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rTimer = GetComponentInParent<RhythmTimer>();
        _orbitalController = GetComponent<ProjectileOrbitalController>();
        _player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleShootInput();
        MovementControl();
        HandleChangeBullet();
    }

    private void MovementControl()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _rigidbody.AddTorque(new Vector3(vertical / 3, 0, -horizontal / 3) * speed);
    }

    private void HandleShootInput()
    {
        if (Input.GetButtonDown("Fire1") && (_rTimer.CheckTime(0.10f) || !IsRhythmActive))
        {
            // Cast a ray from the camera to see where the click intersects with the floor
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                _orbitalController.EnqueueShoot(hit.point);
            }
        }
    }

    private void HandleChangeBullet()
    {
        if (Input.GetKeyDown(SPrevBulletKey))
        {
            var currentBulletIndex = _player.AvailableBullets.FindIndex(
                bullet => bullet == _player.CurrentBullet
            );

            if (currentBulletIndex - 1 < 0) return;

            var newBullet = _player.AvailableBullets[currentBulletIndex - 1];
            _player.CurrentBullet = newBullet;
        }

        if (Input.GetKeyDown(SNextBulletKey))
        {
            var currentBulletIndex = _player.AvailableBullets.FindIndex(
                bullet => bullet == _player.CurrentBullet
            );

            if (currentBulletIndex + 1 > _player.AvailableBullets.Count) return;

            var newBullet = _player.AvailableBullets[currentBulletIndex + 1];
            _player.CurrentBullet = newBullet;
        }
    }
}