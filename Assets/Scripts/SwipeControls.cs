using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject cam;
    [SerializeField] private float movementFriction;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float mouseZoomAmount;
    [SerializeField] private float minCamScale, maxCamScale;
    private bool _primaryTouch;
    private bool _secondTouch;
    private float initialFingerDistance;
    private float FingerDistanceDelta;
    private float initialCamScale;

    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
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
            initialFingerDistance = 0f;
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
                if (initialFingerDistance == 0f)
                {
                    initialFingerDistance = Mathf.Sqrt(Mathf.Pow((firstFingerPos.x - secondFingerPos.x), 2) +
                        Mathf.Pow((firstFingerPos.y - secondFingerPos.y), 2));
                    FingerDistanceDelta = 0f;
                    initialCamScale = cam.GetComponent<Camera>().orthographicSize / initialFingerDistance;
                }
                else
                {
                    FingerDistanceDelta = Mathf.Sqrt(Mathf.Pow((firstFingerPos.x - secondFingerPos.x), 2) +
                        Mathf.Pow((firstFingerPos.y - secondFingerPos.y), 2));

                    float delta = FingerDistanceDelta - initialFingerDistance;

                    cam.GetComponent<Camera>().orthographicSize = Mathf.Clamp((initialCamScale * (initialFingerDistance - delta)), minCamScale, maxCamScale);
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