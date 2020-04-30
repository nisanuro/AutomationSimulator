using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace _05_AutomationSimulator
{
    public enum State
    {
        Start,
        Slow,
        Position,
        Stop
    }

    public class Controller
    {
        public static State CurrentState = State.Start;
        public static double TimeMidTrig, TimeRightTrig;

        public static Outputs Update(Inputs inputs)
        {
            Outputs outputs = new Outputs();

            double positionTime = (TimeRightTrig - TimeMidTrig) / 4;


            if (CurrentState == State.Start && inputs.ProximitySensorMiddle)
            {
                CurrentState = State.Slow;
                TimeMidTrig = inputs.CurrentTimeInMilliseconds;
            } 
           
            else if (CurrentState == State.Slow && inputs.ProximitySensorRight)
            {
                CurrentState = State.Position;
                TimeRightTrig = inputs.CurrentTimeInMilliseconds;
            }               

            else if (CurrentState == State.Position && (inputs.CurrentTimeInMilliseconds > (TimeRightTrig + positionTime)))
            {
                CurrentState = State.Stop;
            }                

            else if(inputs.ProximitySensorLeft)
            {
                CurrentState = State.Stop;
            }


            if(inputs.PositioningEnabled)
            {
                switch (CurrentState)
                {
                    case State.Start:
                        outputs.MoveRight = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedFast;
                        break;

                    case State.Slow:
                        outputs.MoveRight = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedSlow;
                        break;

                    case State.Position:
                        outputs.MoveLeft = true;
                        outputs.MoveSpeed = Configuration.MotorSpeedSlow;
                        break;
                    case State.Stop:
                        outputs.MoveSpeed = 0;
                        break;
                }
            }
            
            return outputs;
        }
    }
}
