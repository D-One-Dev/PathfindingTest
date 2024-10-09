using UnityEngine;
using Zenject;

public class SwipeControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementFriction;
    [SerializeField] private float mouseZoomAmount;
    [SerializeField] private float minCamScale, maxCamScale;

    [Inject(Id = "Camera")]
    private readonly GameObject cam;
    [Inject(Id = "Camera")]
    private readonly Rigidbody2D rb;

    private bool _primaryTouch;
    private bool _secondTouch;
    private float _initialFingerDistance;
    private float _fingerDistanceDelta;
    private float _initialCamScale;

    private Controls _controls;

    [Inject]
    public void Construct(Controls controls)
    {
        _controls = controls;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    void Start()
    {
        _controls.Gameplay.PrimaryTouch.started += ctx => _primaryTouch = true;
        _controls.Gameplay.PrimaryTouch.canceled += ctx => _primaryTouch = false;
        _controls.Gameplay.SecondTouch.started += ctx =>
        {
            _secondTouch = true;
            _initialFingerDistance = 0f;
        };
        _controls.Gameplay.SecondTouch.canceled += ctx => _secondTouch = false;
    }

    private void Update()
    {
        float scroll = _controls.Gameplay.MouseScroll.ReadValue<float>();
        if (scroll > 0) ZoomCamera(true);
        else if (scroll < 0) ZoomCamera(false);
    }
    void FixedUpdate()
    {
        if (_primaryTouch)
        {
            if (_secondTouch)
            {
                rb.velocity = Vector3.zero;
                Vector2 firstFingerPos = _controls.Gameplay.PrimaryTouchPos.ReadValue<Vector2>();
                Vector2 secondFingerPos = _controls.Gameplay.SecondTouchPos.ReadValue<Vector2>();
                if (_initialFingerDistance == 0f)
                {
                    _initialFingerDistance = Mathf.Sqrt(Mathf.Pow((firstFingerPos.x - secondFingerPos.x), 2) +
                        Mathf.Pow((firstFingerPos.y - secondFingerPos.y), 2));
                    _fingerDistanceDelta = 0f;
                    _initialCamScale = cam.GetComponent<Camera>().orthographicSize / _initialFingerDistance;
                }
                else
                {
                    _fingerDistanceDelta = Mathf.Sqrt(Mathf.Pow((firstFingerPos.x - secondFingerPos.x), 2) +
                        Mathf.Pow((firstFingerPos.y - secondFingerPos.y), 2));

                    float delta = _fingerDistanceDelta - _initialFingerDistance;

                    cam.GetComponent<Camera>().orthographicSize = Mathf.Clamp((_initialCamScale * (_initialFingerDistance - delta)), minCamScale, maxCamScale);
                }
            }

            else
            {
                Vector2 movement = _controls.Gameplay.PrimaryTouchDelta.ReadValue<Vector2>();
                movement *= -moveSpeed;

                Vector3 localMovement = new(movement.x, movement.y, 0f);
                Vector3 globalMovement = cam.transform.TransformDirection(localMovement);
                rb.velocity = globalMovement * (cam.GetComponent<Camera>().orthographicSize / 5f);
            }
        }
        else
        {
            float frx, frz;
            if (rb.velocity.x > .01f) frx = rb.velocity.x - Mathf.Lerp(movementFriction, Mathf.Abs(rb.velocity.x), .1f);
            else if (rb.velocity.x < -.01f) frx = rb.velocity.x + Mathf.Lerp(movementFriction, Mathf.Abs(rb.velocity.x), .1f);
            else frx = 0f;

            if (rb.velocity.y > .01f) frz = rb.velocity.y - Mathf.Lerp(movementFriction, Mathf.Abs(rb.velocity.y), .1f);
            else if (rb.velocity.y < -.01f) frz = rb.velocity.y + Mathf.Lerp(movementFriction, Mathf.Abs(rb.velocity.y), .1f);
            else frz = 0f;

            rb.velocity = new Vector2(frx, frz);
        }
    }

    private void ZoomCamera(bool direction)
    {
        if (direction) cam.GetComponent<Camera>().orthographicSize =
                Mathf.Clamp((cam.GetComponent<Camera>().orthographicSize - mouseZoomAmount), minCamScale, maxCamScale);
        else cam.GetComponent<Camera>().orthographicSize =
                Mathf.Clamp((cam.GetComponent<Camera>().orthographicSize + mouseZoomAmount), minCamScale, maxCamScale);
    }
}