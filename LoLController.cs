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
        private bool MinimapMode = false;
        private bool LockedCamera = false;
        private Point HoldingPingPosition = Point.Empty;
        private long LastMovementInput = 0;
        private long LastScrollInput = 0;
        private bool movingLeft = false;
        private bool movingRight = false;
        private bool movingUp = false;
        private bool movingDown = false;
        private float MinimapWidth = 0;
        private float MinimapHeight = 0;

        private float MinimapLeftUpCornerX = 0;
        private float MinimapLeftUpCornerY = 0;

        private float MinimapBottomRightCornerX = 0;
        private float MinimapBottomRightCornerY = 0;

        private float MinimapCenterX = 0;
        private float MinimapCenterY = 0;

        public LoLController()
        {
            InputController.OnButtonDown += InputController_OnButtonDown;
            InputController.OnButtonUp += InputController_OnButtonUp;
            //This is using the default minimap scale of 0.33 at 1920x1080
            MinimapWidth = Screen.PrimaryScreen.Bounds.Width / 7.559055118110236f;
            MinimapHeight = MinimapWidth;
            //
            MinimapLeftUpCornerX = Screen.PrimaryScreen.Bounds.Width * 0.862f;
            MinimapLeftUpCornerY = Screen.PrimaryScreen.Bounds.Height * 0.7537f;
            //
            MinimapBottomRightCornerX = MinimapLeftUpCornerX + MinimapWidth;
            MinimapBottomRightCornerY = MinimapLeftUpCornerY + MinimapHeight;

            MinimapCenterX = MinimapLeftUpCornerX + (MinimapWidth / 2);
            MinimapCenterY = MinimapLeftUpCornerY + (MinimapHeight / 2);
        }

        public void Tick()
        {
            if(!MinimapMode) HandleJoystickLeft();
            HandleJoystickRight();
        }

        private void HandleJoystickLeft()
        {
            float xx = (InputController.JoystickLeftX / 32767f) * 100;
            float yy = (InputController.JoystickLeftY / 32767f) * 100;
            if (IsShopOpen)
            {
                FreeMouse(xx, yy);
            }
            else
            {
                //if (!HoldingPing)
                    DoAim(xx, yy);
            }
        }

        private void HandleJoystickRight()
        {
            float xx = (InputController.JoystickRightX / 32767f) * 100;
            float yy = (InputController.JoystickRightY / 32767f) * 100;
            if (IsShopOpen)
            {
                DoScroll(xx, yy);
            }
            else
            {
                if (MinimapMode)
                {
                    MoveMinimap(xx, yy);
                }
                /*else if (HoldingPing)
                {
                    //DoPing(xx, yy);
                }*/
                else
                {
                    MoveCamera(xx, yy);
                }
            }
        }

        private void InputController_OnButtonUp(GamepadButtonFlags button)
        {
            switch (button)
            {
                case GamepadButtonFlags.LeftThumb:
                {
                    if(!IsShopOpen && !MinimapMode)
                        //Ctrl
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.LCONTROL);
                }
                break;
                case GamepadButtonFlags.RightThumb:
                {
                    MinimapMode = !MinimapMode;
                    if (MinimapMode)
                    {
                        //Move mouse over the minimap center.
                        InputController.SetCursorPosition((int)MinimapCenterX, (int)MinimapCenterY);
                        //Left mouse down...
                        InputController.LeftButtonDown();
                    }
                    else
                    {
                        //Release left mouse
                        InputController.LeftButtonUp();
                    }
                }
                break;
                case GamepadButtonFlags.Start:
                {
                    //Shop
                    IsShopOpen = !IsShopOpen;
                    if (IsShopOpen) MinimapMode = false;
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
                    //Q
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
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
                        //W
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }
                }
                break;
                case GamepadButtonFlags.B:
                {
                    //E
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
                }
                break;
                case GamepadButtonFlags.Y:
                {
                    //R
                    InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_R);
                }
                break;
                case GamepadButtonFlags.DPadLeft:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //Missing ping
                        //InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_H);
                        //Item 1
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_1);
                    }
                    else if(!MinimapMode)
                    {
                        //T
                        InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_T);
                    }
                }
                break;
                case GamepadButtonFlags.DPadUp:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //Danger ping
                        //InputController.SendVClick();
                        //Item 4
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_4);
                    }
                    else if(!MinimapMode)
                    {
                        //B
                        InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_B);
                    }
                }
                break;
                case GamepadButtonFlags.DPadRight:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //OnMyWay ping
                        //InputController.SendKeyShift(WindowsInput.Native.VirtualKeyCode.VK_V);
                        //Item 3
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_3);
                    }
                    else
                    {
                        //Ctrl+4
                        InputController.SendKeyCtrl(WindowsInput.Native.VirtualKeyCode.VK_4);
                    }
                }
                break;
                case GamepadButtonFlags.DPadDown:
                {
                    if (InputController.LeftTriggerDown)
                    {
                        //InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_K);
                        //Item 2
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.VK_2);
                    }
                    else if(!MinimapMode)
                    {
                        //Y
                        LockedCamera = !LockedCamera;
                        InputController.SendKey(WindowsInput.Native.VirtualKeyCode.VK_Y);
                    }
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
                    if (!IsShopOpen && !MinimapMode)
                    {
                        //Q
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);

                        if (InputController.LeftTriggerDown)
                        {
                            //Item 1
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_1);
                        }
                        else
                        {
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
                    else if(!MinimapMode)
                    {
                        //W
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);

                        if (InputController.LeftTriggerDown)
                        {
                            //Item 2
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_2);
                        }
                        else
                        {
                        }
                    }
                }
                break;
                case GamepadButtonFlags.B:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        //E
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);

                        if (InputController.LeftTriggerDown)
                        {
                            //Item 3
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_3);
                        }
                        else
                        {
                        }
                    }
                }
                break;
                case GamepadButtonFlags.Y:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        //R
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_R);
                    }
                }
                break;
                case GamepadButtonFlags.DPadLeft:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 1
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_1);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.DPadDown:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 2
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_2);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.DPadRight:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 3
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_3);
                        }
                    }
                }
                break;
                case GamepadButtonFlags.DPadUp:
                {
                    if (!IsShopOpen && !MinimapMode)
                    {
                        if (InputController.LeftTriggerDown)
                        {
                            //Item 4
                            InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.VK_4);
                        }
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
            }
        }

        private bool middleDown = false;
        public void MoveCamera(float x, float y)
        {
            if (LockedCamera)
            {
                if (x <= -25f)
                {
                    if (movingRight)
                    {
                        movingRight = false;
                        InputController.SendKeyUp(WindowsInput.Native.VirtualKeyCode.RIGHT);
                    }
                    if (!movingLeft)
                    {
                        movingLeft = true;
                        InputController.SendKeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
                    }
                }
                else if (x >= 25f)
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
                if (y <= -25f)
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
                else if (y >= 25f)
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
            else
            {
                var cursorPos = new Point(Cursor.Position.X, Cursor.Position.Y);
                int h = (int)(0.35f * x);
                int v = (int)(0.35f * y);
                if ((h < -3 || h > 3) || (v < -3 || v > 3))
                {
                    if(middleDown == false)
                    {
                        //Press it up
                        middleDown = true;
                        InputController.MiddleButtonDown();
                    }
                    bool inverted = false;
                    if(inverted)
                    {
                        cursorPos.X -= (int)h;
                        cursorPos.Y += (int)v;
                    }
                    else
                    {
                        cursorPos.X += (int)h;
                        cursorPos.Y -= (int)v;
                    }

                    if (cursorPos != Cursor.Position)
                        InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
                }
                else if(middleDown)
                {
                    //Free it up
                    middleDown = false;
                    InputController.MiddleButtonUp();
                }
            }
        }

        public T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public void MoveMinimap(float x, float y)
        {
            var cursorPos = new Point(Cursor.Position.X, Cursor.Position.Y);

            int h = (int)(0.035f * x);
            int v = (int)(0.035f * y);

            cursorPos.X += h;
            cursorPos.Y -= v;

            cursorPos.X = Clamp(cursorPos.X, (int)MinimapLeftUpCornerX, (int)MinimapBottomRightCornerX);
            cursorPos.Y = Clamp(cursorPos.Y, (int)MinimapLeftUpCornerY, (int)MinimapBottomRightCornerY);

            if (cursorPos != Cursor.Position)
                InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
            if (InputController.RightTriggerDown)
            {
                //Check when was last time sent...
                if (Environment.TickCount - LastMovementInput > 150)
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

        public void FreeMouse(float x, float y)
        {
            var cursorPos = new Point(Cursor.Position.X, Cursor.Position.Y);

            int h = (int)(0.25f * x);
            int v = (int)(0.25f * y);

            cursorPos.X += (int)h;
            cursorPos.Y -= (int)v;

            if (cursorPos != Cursor.Position)
                InputController.SetCursorPosition(cursorPos.X, cursorPos.Y);
        }

        public void DoScroll(float x, float y)
        {
            if (Environment.TickCount - LastScrollInput > 150)
            {
                LastScrollInput = Environment.TickCount;
                if (y <= -25)
                {
                    InputController.ScrollUp();
                }
                else if (y >= 25)
                {
                    InputController.ScrollDown();
                }
            }
        }

        public void DoAim(float x, float y)
        {
            if (middleDown) return;
            var cursorPos = new Point((Screen.PrimaryScreen.Bounds.Width / 2) + Screen.PrimaryScreen.Bounds.X, (Screen.PrimaryScreen.Bounds.Height / 2) + Screen.PrimaryScreen.Bounds.Y);

            cursorPos.X += (int)(3 * x);
            cursorPos.Y -= (int)(3 * y);

            if (Cursor.Position != cursorPos)
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
