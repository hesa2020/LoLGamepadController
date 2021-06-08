using SharpDX.XInput;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace LoLGamepadController
{
    public static class InputController
    {
        public static short JoystickLeftX = 0;
        public static short JoystickLeftY = 0;
        public static short JoystickRightX = 0;
        public static short JoystickRightY = 0;

        public static bool LeftTriggerDown = false;
        public static bool RightTriggerDown = false;
        public static List<GamepadButtonFlags> PressedButtons = new List<GamepadButtonFlags>();
        public delegate void ButtonEventDelegate(GamepadButtonFlags button);
        public static event ButtonEventDelegate OnButtonDown;
        public static event ButtonEventDelegate OnButtonUp;
        private static InputSimulator inputSimulator = new InputSimulator();

        internal static void TriggerOnButtonDown(GamepadButtonFlags button)
        {
            if(OnButtonDown != null && OnButtonDown.GetInvocationList().Length > 0)
                OnButtonDown(button);
        }

        internal static void TriggerOnButtonUp(GamepadButtonFlags button)
        {
            if (OnButtonUp != null && OnButtonUp.GetInvocationList().Length > 0)
                OnButtonUp(button);
        }

        public static void SetMousePos(float x, float y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);
            //OnMouseMoved(x, y);
        }

        public static void SendKeyDown(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.KeyDown(key);
        }

        public static void SendKeyUp(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.KeyUp(key);
        }

        public static void SendKey(VirtualKeyCode key)
        {
            SendKeyDown(key);
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(35);
                SendKeyUp(key);
            });
        }

        public static void SendKeyShift(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, key);
        }

        public static void SendKeyCtrl(VirtualKeyCode key)
        {
            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LCONTROL, key);
        }

        public static void LeftButtonDown()
        {
            inputSimulator.Mouse.LeftButtonDown();
        }

        public static void LeftButtonUp()
        {
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void RightButtonDown()
        {
            inputSimulator.Mouse.RightButtonDown();
        }

        public static void RightButtonUp()
        {
            inputSimulator.Mouse.RightButtonUp();
        }

        public static void LeftClick()
        {
            inputSimulator.Mouse.LeftButtonClick();
        }

        public static void LeftClickDelayed()
        {
            inputSimulator.Mouse.LeftButtonDown();
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(35);
                inputSimulator.Mouse.LeftButtonUp();
            });
        }

        public static void RightClick()
        {
            inputSimulator.Mouse.RightButtonClick();
        }

        public static void SendVClick()
        {
            Task.Factory.StartNew(async () =>
            {
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_V);
                await Task.Delay(10);
                inputSimulator.Mouse.LeftButtonDown();
                await Task.Delay(35);
                inputSimulator.Mouse.LeftButtonUp();
                await Task.Delay(10);
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_V);
            });
        }

        public static void SendShiftRightClick()
        {
            Task.Factory.StartNew(async () =>
            {
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
                await Task.Delay(10);
                inputSimulator.Mouse.RightButtonDown();
                await Task.Delay(35);
                inputSimulator.Mouse.RightButtonUp();
                await Task.Delay(10);
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            });
        }

        public static void SetCursorPosition(int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(x, y);
        }
    }
}
