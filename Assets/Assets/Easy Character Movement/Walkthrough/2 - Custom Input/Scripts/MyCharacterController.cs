using ECM.Controllers;
using UnityEngine;

namespace ECM.Walkthrough.CustomInput
{
    /// <summary>
    /// Example of a custom character controller.
    ///
    /// This show how to create a custom character controller extending one of the included 'Base' controller.
    /// In this example, we override the HandleInput method to add our custom input code.
    /// </summary>

    public class MyCharacterController : BaseCharacterController
    {
        [SerializeField] private Joystick _joystick;
        protected override void Animate()
        {
            // Add animator related code here...
        }

        protected override void HandleInput()
        {
            // Default ECM Input as used in BaseCharacterController HandleInput method.
            // Replace this with your custom input code here...

            // Toggle pause / resume.
            // By default, will restore character's velocity on resume (eg: restoreVelocityOnResume = true)

            if (Input.GetKeyDown(KeyCode.P))
                pause = !pause;

            // Handle user input

            moveDirection = new Vector3
            {
                x = _joystick.Horizontal,
                y = 0.0f,
                z = _joystick.Vertical
            };

            jump = Input.GetButton("Jump");

            crouch = Input.GetKey(KeyCode.C);
        }

        private void ChangeSpeed(int newSpeed)
        {
            speed = newSpeed;
        }

        protected virtual void OnEnable()
        {
            HeroStates.SpeedChanged += ChangeSpeed;
        }

        protected virtual void OnDisable()
        {
            HeroStates.SpeedChanged -= ChangeSpeed;
        }
    }
}
