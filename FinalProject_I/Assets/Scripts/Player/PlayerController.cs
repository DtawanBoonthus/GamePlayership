using Spaceship;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSpaceship playerSpaceship;
        private Vector2 movementInput = Vector2.zero;
        private ShipInputActions inputActions;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private void Awake()
        {
            InitInput();
            CreateMovementBoundary();
        }
        
        private void InitInput()
        {
            inputActions = new ShipInputActions();
            inputActions.Player.Move.performed += OnMove;
            inputActions.Player.Move.canceled += OnMove;
            inputActions.Player.Fire.performed += OnFire;
        }

        private void OnFire(InputAction.CallbackContext obj)
        {
            playerSpaceship.Fire();
        }

        public void OnMove(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                movementInput = obj.ReadValue<Vector2>();
            }
            
            if (obj.canceled)
            {
                movementInput = Vector2.zero; 
            }
        }
        
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var inputVelocity = movementInput * playerSpaceship.Speed;

            var newPosition = transform.position;
            newPosition.x = transform.position.x + inputVelocity.x * Time.smoothDeltaTime;
            newPosition.y = transform.position.y + inputVelocity.y * Time.smoothDeltaTime;

            // Clamp movement within boundary
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            transform.position = newPosition;
        }
        
        private void CreateMovementBoundary()
        {
            var mainCamera = Camera.main;
            Debug.Assert(mainCamera != null, "Main camera cannot be null");
            
            var spriteRenderer = playerSpaceship.GetComponent<SpriteRenderer>();
            Debug.Assert(spriteRenderer != null, "spriteRenderer cannot be null");
            
            var offset = spriteRenderer.bounds.size;
            minX = mainCamera.ViewportToWorldPoint(mainCamera.rect.min).x + offset.x / 2;
            maxX = mainCamera.ViewportToWorldPoint(mainCamera.rect.max).x - offset.x / 2;
            minY = mainCamera.ViewportToWorldPoint(mainCamera.rect.min).y + offset.y / 2;
            maxY = mainCamera.ViewportToWorldPoint(mainCamera.rect.max).y - offset.y / 2;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
    }
}
