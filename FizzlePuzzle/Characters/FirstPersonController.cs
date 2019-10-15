using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace FizzlePuzzle.Characters
{
    [RequireComponent(typeof(CharacterController))]
    internal class FirstPersonController : FizzleCharacterController
    {
        [SerializeField] private float m_RunSpeed = 10.0F;
        [SerializeField] private float m_WalkSpeed = 5.0F;
        [SerializeField] private float m_JumpSpeed = 10.0F;
        [SerializeField] private float m_GravityMultiplier = 2.0F;
        [SerializeField] private float m_StickToGroundForce = 10.0F;
        [SerializeField] private MouseLook m_MouseLook = new MouseLook();
        private bool isWalking = true;
        private Vector3 moveDir = Vector3.zero;
        private Camera camera;
        private CharacterController characterController;
        private bool isJumping;
        private bool jumped;
        private bool lastGrounded;
        private Vector3 originalCameraPosition;

        protected override void Awake()
        {
            base.Awake();
            characterController = GetComponent<CharacterController>();
        }

        protected override void Start()
        {
            base.Start();
            camera = Camera.main;
            Debug.Assert(camera != null, nameof(camera) + " != null");
            originalCameraPosition = camera.transform.localPosition;
            isJumping = false;
            m_MouseLook.Init(transform, camera.transform);
        }

        protected override void Update()
        {
            base.Update();
            if (!controllerEnabled)
            {
                return;
            }
            RotateView();
            if (!jumped)
            {
                jumped = Input.GetButtonDown("Jump");
            }
            isWalking = !Input.GetButton("Sprint");
            if (!lastGrounded && characterController.isGrounded)
            {
                moveDir.y = 0.0F;
                isJumping = false;
            }

            if (characterController.isGrounded && !isJumping && lastGrounded)
            {
                moveDir.y = 0.0F;
            }
            lastGrounded = characterController.isGrounded;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!controllerEnabled)
            {
                return;
            }
            CharacterMove();
            CharacterJump();
            characterController.Move(moveDir * Time.fixedDeltaTime);
        }

        private void CharacterMove()
        {
            float axis1 = Input.GetAxis("Horizontal");
            float axis2 = Input.GetAxis("Vertical");
            float num = isWalking ? m_WalkSpeed : m_RunSpeed;
            Vector2 vector2 = new Vector2(axis1, axis2);
            if (vector2.sqrMagnitude > 1.0F)
            {
                vector2.Normalize();
            }
            Vector3 vector3 = transform.forward * vector2.y + transform.right * vector2.x;
            moveDir.x = vector3.x * num;
            moveDir.z = vector3.z * num;
        }

        private void CharacterJump()
        {
            if (characterController.isGrounded)
            {
                moveDir.y = -m_StickToGroundForce;
                if (!jumped)
                {
                    return;
                }
                moveDir.y = m_JumpSpeed;
                jumped = false;
                isJumping = true;
            }
            else
            {
                moveDir += Time.fixedDeltaTime * m_GravityMultiplier * Physics.gravity;
            }
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation();
        }
    }
}