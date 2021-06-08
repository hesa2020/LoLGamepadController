using System;
using System.Threading;
using SharpDX.XInput;

namespace LoLGamepadController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            LoLController LoL = new LoLController();
            Console.WriteLine("Start XGamepadApp");
            // Initialize XInput
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };

            // Get 1st controller available
            Controller controller = null;
            foreach (var selectControler in controllers)
            {
                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller == null)
            {
                Console.WriteLine("No XInput controller instwalled");
            }
            else
            {

                Console.WriteLine("Found a XInput controller available");
                Console.WriteLine("Press buttons on the controller to display events or escape key to exit... ");

                // Poll events from joystick
                var previousState = controller.GetState();
                while (controller.IsConnected)
                {
                    if (IsKeyPressed(ConsoleKey.Escape))
                    {
                        break;
                    }
                    var state = controller.GetState();
                    if (previousState.PacketNumber != state.PacketNumber)
                    {
                        Console.WriteLine(state.Gamepad);
                        foreach (GamepadButtonFlags value in Enum.GetValues(state.Gamepad.Buttons.GetType()))
                        {
                            if (value == GamepadButtonFlags.None) continue;
                            if (state.Gamepad.Buttons.HasFlag(value))
                            {
                                if(!InputController.PressedButtons.Contains(value))
                                {
                                    lock(InputController.PressedButtons)
                                    {
                                        InputController.PressedButtons.Add(value);
                                    }
                                    InputController.TriggerOnButtonDown(value);
                                }
                            }
                            else
                            {
                                if (InputController.PressedButtons.Contains(value))
                                {
                                    lock (InputController.PressedButtons)
                                    {
                                        InputController.PressedButtons.Remove(value);
                                    }
                                    InputController.TriggerOnButtonUp(value);
                                }
                            }
                        }
                        if (state.Gamepad.LeftTrigger == 255)
                        {
                            InputController.LeftTriggerDown = true;
                        }
                        else
                        {
                            InputController.LeftTriggerDown = false;
                        }
                        //
                        if (state.Gamepad.RightTrigger == 255)
                        {
                            InputController.RightTriggerDown = true;
                        }
                        else
                        {
                            InputController.RightTriggerDown = false;
                        }

                        InputController.JoystickLeftX = state.Gamepad.LeftThumbX;
                        InputController.JoystickLeftY = state.Gamepad.LeftThumbY;
                        InputController.JoystickRightX = state.Gamepad.RightThumbX;
                        InputController.JoystickRightY = state.Gamepad.RightThumbY;
                    }
                    LoL.Tick();
                    Thread.Sleep(10);
                    previousState = state;
                }
            }
            Console.WriteLine("End XGamepadApp");
            Console.ReadLine();
        }

        /// <summary>
        /// Determines whether the specified key is pressed.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is pressed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(ConsoleKey key)
        {
            return Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }
    }
}