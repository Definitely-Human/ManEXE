using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ManExe
{
    public class World : MonoBehaviour
    {

        public float[,] heightMap = new float[GameData.ChunkHeight * GameData.WorldSizeInChunks, GameData.ChunkHeight * GameData.WorldSizeInChunks];



        [SerializeField] private int _timeOfDay;
        [SerializeField] private TextMeshProUGUI clock;

        [SerializeField] private int dayStartTime = 240; // 4:00
        [SerializeField] private int dayEndTime = 1320; // 22:00

        private int dayLength { get { return dayEndTime - dayStartTime; } }
        private float sunDayRotationPerMinute { get { return 180f / dayLength; } }
        private float sunNightRotationPerMinute { get { return 180f / (1440-dayLength); } }

        public Transform sun;
        public int Day = 1;

        [Range(4f,0.01f)]
        public float ClockSpeed = 1f;

        public int TimeOfDay
        {
            get { return _timeOfDay; }
            set
            {
                _timeOfDay = value;
                if (_timeOfDay >= 1440)
                {
                    Day += _timeOfDay / 1440;
                    _timeOfDay %= 1440;

                }

                UpdateClock();

                float rotAmount;

                // The start of the "day" is zero rotation on the sunlight, so that's the most straightforward
                // calculation.
                if (_timeOfDay > dayStartTime && _timeOfDay < dayEndTime)
                {

                    rotAmount = (_timeOfDay - dayStartTime) * sunDayRotationPerMinute;

                    // At the end of the "day" we switch to night rotation speed, but in order to keep the rotation
                    // seamless, we need to account for the daytime rotation as well.
                }
                else if (_timeOfDay >= dayEndTime)
                {

                    // Calculate the amount of rotation through the day so far.
                    rotAmount = dayLength * sunDayRotationPerMinute;
                    // Add the rotation since the end of the day.
                    rotAmount += ((_timeOfDay - dayStartTime - dayLength) * sunNightRotationPerMinute);

                    // Else we're at the start of a new day but because we're still in the same rotation cycle, we need to
                    // to account for all the previous rotation since dayStartTime the previous day.
                }
                else
                {

                    rotAmount = dayLength * sunDayRotationPerMinute; // Previous day's rotation.
                    rotAmount += (1440 - dayEndTime) * sunNightRotationPerMinute; // Previous night's rotation.
                    rotAmount += _timeOfDay * sunNightRotationPerMinute; // Rotation since midnight.

                }

                sun.eulerAngles = new Vector3(rotAmount, 0f, 0f);
            }
        }

        private void UpdateClock()
        {
            int hours = TimeOfDay / 60;
            int minutes = TimeOfDay % 60;

            clock.text = string.Format("Day: {0} Time: {1}:{2}",Day.ToString(), hours.ToString("D2"), minutes.ToString("D2"));
        }

        private float secondCounter = 0;
        private void Update()
        {
            secondCounter += Time.deltaTime;
            if(secondCounter >= ClockSpeed)
            {
                TimeOfDay+= (int)(secondCounter / ClockSpeed);
                secondCounter %= ClockSpeed;
            }
        }
    }
}
