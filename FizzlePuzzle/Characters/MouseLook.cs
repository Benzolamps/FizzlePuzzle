using System;
using UnityEngine;

namespace FizzlePuzzle.Characters
{
    [Serializable]
    internal class MouseLook
    {
        [SerializeField] [Range(0.0F, 360.0F)] private float m_MaxDepressionAngle = 90.0F;
        [SerializeField] [Range(0.0F, 360.0F)] private float m_MaxElevationAngle = 90.0F;
        [SerializeField] private float m_SmoothTime = 5.0F;
        [SerializeField] [Range(0.0F, 10.0F)] private float m_SensitivityX = 2.0F;
        [SerializeField] [Range(0.0F, 10.0F)] private float m_SensitivityY = 2.0F;
        private Transform camera;
        private Quaternion cameraTargetRotation;
        private Transform character;
        private Quaternion characterTargetRotation;

        internal void Init(Transform character, Transform camera)
        {
            this.character = character;
            this.camera = camera;
        }

        internal void LookRotation()
        {
            float y = Input.GetAxis("Mouse X") * m_SensitivityX;
            float num = Input.GetAxis("Mouse Y") * m_SensitivityY;
            characterTargetRotation = character.localRotation;
            cameraTargetRotation = camera.localRotation;
            characterTargetRotation *= Quaternion.Euler(0.0F, y, 0.0F);
            cameraTargetRotation *= Quaternion.Euler(-num, 0.0F, 0.0F);
            cameraTargetRotation = ClampRotation(cameraTargetRotation);
            character.localRotation = characterTargetRotation;
            camera.localRotation = cameraTargetRotation;
            ApplyRotation();
        }

        private void ApplyRotation()
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRotation, m_SmoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRotation, m_SmoothTime * Time.deltaTime);
        }

        private Quaternion ClampRotation(Quaternion quaternion)
        {
            quaternion.x /= quaternion.w;
            quaternion.y /= quaternion.w;
            quaternion.z /= quaternion.w;
            quaternion.w = 1.0F;
            float num = Mathf.Clamp(114.5916F * Mathf.Atan(quaternion.x), -m_MaxElevationAngle, m_MaxDepressionAngle);
            quaternion.x = Mathf.Tan((float) Math.PI / 360.0F * num);
            return quaternion;
        }
    }
}
