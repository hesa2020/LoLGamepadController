using SharpDX.XInput;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LoLGamepadController
{
    public class LoLController
    {
        private bool IsShopOpen = false;
        private bool HoldingPing = false;
        private Point HoldingPingPosition = Point.Empty;
        private long LastMovementInput = 0;
        private bool movingLeft = false;
        private bool movingRight = false;
        private bool movingUp = false;
        private bool movingDown = false;

        public LoLController()
        {
            InputController.OnButtonDown += InputController_OnButtonDown;
            InputController.OnButtonUp += InputController_OnButtonUp;
        }

        public void Tick()
        {
            HandleJoystickLeft();
            HandleJoystickRight();
        }

        private void HandleJoystickLeft()
        {
            float xx = InputController.JoystickLeftX / 32767f;
            float yy = InputController.JoystickLeftY / 32767f;
            if (IsShopOpen)
            {
                FreeMouse(xx, yy);
            }
            else
            {
                if (!HoldingPing)
                    DoAim(xx, yy);
            }
        }

        private void HandleJoystickRight()
        {
            float xx = InputController.JoystickRightX / 32767f;
            float yy = InputController.JoystickRightY / 32767f;
            if (HoldingPing)
            {
                DoPing(xx, yy);
            }
            else
            {
                MoveCamera(xx, yy);
            }
        }

        private void InputController_OnButtonUp(GamepadButtonFlags button)
        {
            switch (button)
            {
                case GamepadButtonFlags.LeftThumb:
                {
                    //Ctrl
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.LCONTROL);
                }
                break;
                case GamepadButtonFlags.Start:
                {
                    //Shop
                    IsShopOpen = !IsShopOpen;
                    InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_P);
                }
                break;
                case GamepadButtonFlags.Back:
                {
                    //Tab
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.TAB);
                    Console.WriteLine("Sending keyup [Tab]");
                }
                break;
                case GamepadButtonFlags.X:
                {
                    if(InputController.LeftTriggerDown)
                    {
                        //Item 1
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_1);
                    }
                    else
                    {
                        //Q
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
                    }
                }
                break;
                case GamepadButtonFlags.A:
                {
                    if (IsShopOpen)
                    {
                        InputController.LeftButtonUp();
                    }
                    else
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 2
                            InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_2);
                        }
                        else
                        {
                            //W
                            InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.B:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //Item 3
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_3);
                    }
                    else
                    {
                        //E
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
                    }
                }
                break;
                case GamepadButtonFlags.Y:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //Item 4
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_4);
                    }
                    else
                    {
                        //R
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_R);
                    }
                }
                break;
                case GamepadButtonFlags.DPadUp:
                {
                    //B
                    InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_B);
                }
                break;
                case GamepadButtonFlags.DPadDown:
                {
                    //Y
                    InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_Y);
                }
                break;
                case GamepadButtonFlags.LeftShoulder:
                {
                    //D
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_D);
                }
                break;
                case GamepadButtonFlags.RightShoulder:
                {
                    //F
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_F);
                }
                break;
            }
        }

        private void InputController_OnButtonDown(GamepadButtonFlags button)
        {
            switch(button)
            {
                case GamepadButtonFlags.LeftThumb:
                {
                    if (!IsShopOpen)
                    {
                        //Ctrl
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.LCONTROL);
                    }
                }
                break;
                case GamepadButtonFlags.Back:
                {
                    if (!IsShopOpen)
                    {
                        //Tab
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.TAB);
                    }
                }
                break;
                case GamepadButtonFlags.X:
                {
                    if (!IsShopOpen)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 1
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_1);
                        }
                        else
                        {
                            //Q
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.A:
                {
                    if (IsShopOpen)
                    {
                        InputController.LeftButtonDown();
                    }
                    else
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 2
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_2);
                        }
                        else
                        {
                            //W
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.B:
                {
                    if (!IsShopOpen)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 3
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_3);
                        }
                        else
                        {
                            //E
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.Y:
                {
                    if (!IsShopOpen)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 4
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_4);
                        }
                        else
                        {
                            //R
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_R);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.DPadRight:
                {
                    if (!IsShopOpen)
                    {
                        //Alt
                        /*if(HoldingPing == false)
                        {
                            HoldingPing = true;
                            if(InputController.GetCursorPos(out Point position))
                            {
                                HoldingPingPosition = Cursor.Position;
                                //HoldingPingPosition = position;
                                InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.MENU);
                                InputController.LeftButtonDown();
                            }
                        }*/
                    }
                }
                break;
                case GamepadButtonFlags.LeftShoulder:
                {
                    if (!IsShopOpen)
                    {
                        //D
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_D);
                    }
                }
                break;
                case GamepadButtonFlags.RightShoulder:
                {
                    if (!IsShopOpen)
                    {
                        //F
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_F);
                    }
                }
                break;
                //case GamepadButtonFlags.A:
                //{
                //    //Right
                //    InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);
                //    Console.WriteLine("Sending keydown [Right]");
                //}
                //break;
            }
        }

        public void MoveCamera(float x, float y)
        {
            if (x <= -0.25f)
            {
                if(movingRight)
                {
                    movingRight = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.RIGHT);
                }
                if(!movingLeft)
                {
                    movingLeft = true;
                    InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
                }
            }
            else if(x >= 0.25f)
            {
                if (movingLeft)
                {
                    movingLeft = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);
                }
                if (!movingRight)
                {
                    movingRight = true;
                    InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);
                }
            }
            else
            {
                if (movingRight)
                {
                    movingRight = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.RIGHT);
                }
                if (movingLeft)
                {
                    movingLeft = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);
                }
            }
            //
            if (y <= -0.25f)
            {
                if (movingUp)
                {
                    movingUp = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.UP);
                }
                if (!movingDown)
                {
                    movingDown = true;
                    InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.DOWN);
                }
            }
            else if (y >= 0.25f)
            {
                if (movingDown)
                {
                    movingDown = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.DOWN);
                }
                if (!movingUp)
                {
                    movingUp = true;
                    InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.UP);
                }
            }
            else
            {
                if (movingDown)
                {
                    movingDown = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.DOWN);
                }
                if (movingUp)
                {
                    movingUp = false;
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.UP);
                }
            }
        }

        public void MoveMinimap()
        {
            //TODO...
        }

        public void FreeMouse(float x, float y)
        {
            var cursorPos = new Point(Cursor.Position.X, Cursor.Position.Y);

            float h = 25 * x * 1;
            float v = 25 * y * 1;

            cursorPos.X += (int)h;
            cursorPos.Y -= (int)v;

            if (cursorPos != Cursor.Position)
                InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
        }

        public void DoAim(float x, float y)
        {
            var cursorPos = new Point((Screen.PrimaryScreen.Bounds.Width / 2) + Screen.PrimaryScreen.Bounds.X, (Screen.PrimaryScreen.Bounds.Height / 2) + Screen.PrimaryScreen.Bounds.Y);
            if (x <= -0.15f)
            {
                cursorPos.X += (int)(300 * x);
            }
            else if (x >= 0.15f)
            {
                cursorPos.X += (int)(300 * x);
            }
            if (y <= -0.15f)
            {
                cursorPos.Y -= (int)(300 * y);
            }
            else if (y >= 0.15f)
            {
                cursorPos.Y -= (int)(300 * y);
            }
            if(Cursor.Position != cursorPos)
                InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
            if (InputController.RightTriggerDown)
            {
                //Check when was last time sent...
                if(Environment.TickCount - LastMovementInput > 150)
                {
                    LastMovementInput = Environment.TickCount;
                    if (InputController.LeftTriggerDown)
                    {
                        InputController.SendShiftRightClick();
                    }
                    else
                    {
                        InputController.RightClick();
                    }
                }
            }
        }

        public void DoPing(float x, float y)
        {
            var cursorPos = new Point(HoldingPingPosition.X, HoldingPingPosition.Y);
            if(x <= -0.25f)
            {
                cursorPos.X += (int)(75 * x);
            }
            else if (x >= 0.25f)
            {
                cursorPos.X += (int)(75 * x);
            }
            if (y <= -0.25f)
            {
                cursorPos.Y -= (int)(75 * y);
            }
            else if (y >= 0.25f)
            {
                cursorPos.Y -= (int)(75 * y);
            }
            if(HoldingPingPosition != cursorPos)
                InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
        }
    }
}
