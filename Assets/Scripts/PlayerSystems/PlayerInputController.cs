using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float movementThreshold;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    public static PlayerInputController Instance { get; private set; }
    public bool IsRunning => Input.GetKey(runKey);
    public bool IsInteracting => Input.GetKey(interactKey);
    public bool IsMoving => MovementDirection().magnitude > movementThreshold;

    Vector3 _smoothInput = Vector3.zero;
    public float SmoothMagnitude => _smoothInput.magnitude;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector3 MovementDirection()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void Update()
    {
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        _smoothInput = Vector3.Lerp(_smoothInput, rawInput, Time.deltaTime * 5f);
    }
}
