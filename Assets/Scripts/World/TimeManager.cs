using UnityEngine;

namespace ManExe.World
{

    public class TimeManager: MonoBehaviour
    {
        // === References ===
        [SerializeField] private Transform sun;

        // === Data ===
        [SerializeField] 
        private int timeOfDay = 360; // 6:00

        [SerializeField]
        private int dayStartTime = 240; // 4:00
        [SerializeField]
        private int dayEndTime = 1320; // 22:00

        [SerializeField]
        private int day = 1;

        [Range(4f, 0.01f)]
        [SerializeField]
        private float clockSpeed = 1f;
        
        private float _secondCounter = 0;
        // === Properties ===
        private int DayLength { get { return dayEndTime - dayStartTime; } }
        private float SunDayRotationPerMinute { get { return 180f / DayLength; } }
        private float SunNightRotationPerMinute { get { return 180f / (1440 - DayLength); } }

        public int TimeOfDay
        {
            get { return timeOfDay; }
            set
            {
                timeOfDay = value;
                if (timeOfDay >= 1440)
                {
                    day += timeOfDay / 1440;
                    timeOfDay %= 1440;

                }


                float rotAmount;

                // The start of the "day" is zero rotation on the sunlight, so that's the most straightforward
                // calculation.
                if (timeOfDay > dayStartTime && timeOfDay < dayEndTime)
                {

                    rotAmount = (timeOfDay - dayStartTime) * SunDayRotationPerMinute;

                    // At the end of the "day" we switch to night rotation speed, but in order to keep the rotation
                    // seamless, we need to account for the daytime rotation as well.
                }
                else if (timeOfDay >= dayEndTime)
                {

                    // Calculate the amount of rotation through the day so far.
                    rotAmount = DayLength * SunDayRotationPerMinute;
                    // Add the rotation since the end of the day.
                    rotAmount += ((timeOfDay - dayStartTime - DayLength) * SunNightRotationPerMinute);

                    // Else we're at the start of a new day but because we're still in the same rotation cycle, we need to
                    // to account for all the previous rotation since dayStartTime the previous day.
                }
                else
                {

                    rotAmount = DayLength * SunDayRotationPerMinute; // Previous day's rotation.
                    rotAmount += (1440 - dayEndTime) * SunNightRotationPerMinute; // Previous night's rotation.
                    rotAmount += timeOfDay * SunNightRotationPerMinute; // Rotation since midnight.

                }

                sun.eulerAngles = new Vector3(rotAmount, 0f, 0f);
            }
        }
        //===============================
        // === MonoBehaviour Methods ===
        //===============================
        private void Update()
        {
            _secondCounter += Time.deltaTime;
            if (_secondCounter >= clockSpeed)
            {
                TimeOfDay += (int)(_secondCounter / clockSpeed);
                _secondCounter %= clockSpeed;
            }
        }
        
        // === Public Methods ===
        //===============================
        
        public string GetTimeFormatted()
        {
            int hours = TimeOfDay / 60;
            int minutes = TimeOfDay % 60;

            return string.Format("Day: {0} Time: {1}:{2}", day.ToString(), hours.ToString("D2"), minutes.ToString("D2"));
        }
        
        // === Private Methods ===
        //===============================

        
    }


}
