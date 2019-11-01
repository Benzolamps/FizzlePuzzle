using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace FizzlePuzzle.TimeEffect
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class TimeController : FizzleBehaviour, IRewind, IFork
    {
        [SerializeField] internal MaxRewindSpeedOption MaxRewindSpeed = MaxRewindSpeedOption.X16;
        [SerializeField] [Range(10.0F, 1000.0F)] internal int MaxSeconds = 300;
        [SerializeField] internal bool EnableFork;

        internal bool timeControllerEnabled;
        private int rewindSpeedIndex;
        private List<int> rewindSpeedOptions;
        private float secondsBeforeRewinding;
        private float rewindSpeedAxisFrame;

        internal bool Rewinding { get; private set; }

        internal float RewindSeconds { get; private set; }

        internal float GlobalSeconds { get; private set; }

        internal bool TimeOut { get; private set; }

        internal int CurrentRewindSpeed => rewindSpeedOptions[rewindSpeedIndex];

        internal bool Overflowed => RewindSeconds >= MaxSeconds;

        private static IEnumerable<RewindController> RewindControllers => FindObjectsOfType<RewindController>();

        private void InvokeThis()
        {
            if (!timeControllerEnabled)
            {
                return;
            }
            string name = CommonTools.GetCallerMethod().Name;
            foreach (RewindController rewindController in RewindControllers)
            {
                rewindController.SendMessage(name, SendMessageOptions.DontRequireReceiver);
            }
        }

        internal event FizzleEvent beginRewinding = () => { };

        internal event FizzleEvent endRewinding = () => { };

        internal event FizzleEvent timeOutRewinding = () => { };

        internal event FizzleEvent beginForking = () => { };

        internal event FizzleEvent endForking = () => { };

        internal bool Forking => ForkSecondsRemain > 0.0F;

        internal float ForkSecondsRemain { get; private set; }

        public void Fork()
        {
            InvokeThis();
        }

        public void BeginForking()
        {
            InvokeThis();
            beginForking();
        }

        public void EndForking()
        {
            InvokeThis();
            endForking();
        }

        public void EnableForking()
        {
            InvokeThis();
            EnableFork = true;
            ForkSecondsRemain = 0.0F;
        }

        public void DisableForking()
        {
            InvokeThis();
            if (ForkSecondsRemain > 0.0F)
            {
                EndForking();
                ForkSecondsRemain = 0.0F;
            }

            EnableFork = false;
        }

        private void ChangeRewindSpeed(bool increase)
        {
            rewindSpeedIndex = Mathf.Clamp(rewindSpeedIndex + (increase ? 1 : -1), 0, rewindSpeedOptions.Count - 1);
        }

        private void InitRewindSpeedOptions()
        {
            rewindSpeedOptions = new List<int> {0};
            for (int index = 0; (int) Mathf.Pow(2.0F, index) <= (int) MaxRewindSpeed; ++index)
            {
                rewindSpeedOptions.Add((int) Mathf.Pow(2.0F, index));
                rewindSpeedOptions.Add(-(int) Mathf.Pow(2.0F, index));
            }

            rewindSpeedOptions.Sort();
        }

        private void ResetRewindSpeed()
        {
            for (short index = 0; index < rewindSpeedOptions.Count; ++index)
            {
                if (rewindSpeedOptions[index] == -1)
                {
                    rewindSpeedIndex = index;
                }
            }
        }

        public void BeginRecording()
        {
            InvokeThis();
            Rewinding = false;
            if (!EnableFork)
                return;
            BeginForking();
        }

        public void Record()
        {
            InvokeThis();
            if (EnableFork)
            {
                if ((ForkSecondsRemain -= Time.fixedDeltaTime) > 0.0F)
                {
                    Fork();
                }
                if (this.StatusDetect("EndForking", ForkSecondsRemain <= 0.0F, true))
                {
                    EndForking();
                }
            }

            if (RewindSeconds < MaxSeconds)
            {
                RewindSeconds += Time.fixedDeltaTime;
            }
            if (RewindSeconds < 0.0F)
            {
                RewindSeconds = 0.0F;
            }
            if (RewindSeconds <= MaxSeconds)
            {
                return;
            }
            RewindSeconds = MaxSeconds;
        }

        public void EndRecording()
        {
            InvokeThis();
        }

        public void BeginRewinding()
        {
            InvokeThis();
            beginRewinding();
            if (ForkSecondsRemain > 0.0F)
            {
                ForkSecondsRemain = 0.0F;
                EndForking();
            }

            Rewinding = true;
            secondsBeforeRewinding = RewindSeconds;
        }

        public void Rewind()
        {
            InvokeThis();
        }

        public void TimeOutRewinding()
        {
            InvokeThis();
            timeOutRewinding();
        }

        public void PauseRewinding()
        {
            InvokeThis();
        }

        public void EndRewinding()
        {
            InvokeThis();
            Rewinding = false;
            endRewinding();
            this.StatusDetect("EndForking", false, false);
            if (EnableFork)
            {
                ForkSecondsRemain = secondsBeforeRewinding - RewindSeconds + 5.0F;
            }
            secondsBeforeRewinding = 0.0F;
            ResetRewindSpeed();
            BeginRecording();
        }

        protected override void Update()
        {
            if (Input.GetButtonDown("Rewind"))
                BeginRewinding();
            if (Input.GetButton("Rewind"))
            {
                float axis = Input.GetAxis("Rewind Speed");
                if (Math.Abs(axis) > 0.0F)
                {
                    if (rewindSpeedAxisFrame >= 0.25F)
                    {
                        rewindSpeedAxisFrame = 0.0F;
                    }
                    if (axis > 0.0F && Math.Abs(rewindSpeedAxisFrame) <= 0.01F)
                    {
                        ChangeRewindSpeed(true);
                    }
                    if (axis < 0.0F && (double) Math.Abs(rewindSpeedAxisFrame) <= 0.01F)
                    {
                        ChangeRewindSpeed(false);
                    }
                    rewindSpeedAxisFrame += Time.fixedDeltaTime;
                }
                else
                {
                    rewindSpeedAxisFrame = 0.0F;
                    if (Input.GetButtonDown("Rewind Speed Acce"))
                    {
                        ChangeRewindSpeed(true);
                    }
                    if (Input.GetButtonDown("Rewind Speed Dece"))
                    {
                        ChangeRewindSpeed(false);
                    }
                }
            }

            if (!Input.GetButtonUp("Rewind"))
            {
                return;
            }
            EndRewinding();
        }

        protected override void Start()
        {
            base.Start();
            Time.fixedDeltaTime = 0.02F;
            InitRewindSpeedOptions();
            ResetRewindSpeed();
            BeginRecording();
        }

        protected override void FixedUpdate()
        {
            if (!timeControllerEnabled)
            {
                return;
            }
            GlobalSeconds += Time.fixedDeltaTime;
            if (!Rewinding)
            {
                Record();
            }
            else
            {
                if (this.StatusDetect("PauseRewinding", CurrentRewindSpeed == 0, true))
                {
                    PauseRewinding();
                }
                RewindSeconds = Mathf.Clamp(RewindSeconds + CurrentRewindSpeed * Time.fixedDeltaTime, 0.0F, secondsBeforeRewinding);
                bool given = RewindSeconds >= secondsBeforeRewinding || RewindSeconds <= 0.0F;
                if (this.StatusDetect("TimeOutRewinding", given, true))
                {
                    TimeOutRewinding();
                    TimeOut = true;
                }

                if (given)
                {
                    return;
                }
                Rewind();
                TimeOut = false;
            }
        }

        internal enum MaxRewindSpeedOption
        {
            X1 = 1,
            X2 = 2,
            X4 = 4,
            X8 = 8,
            X16 = 16
        }
    }
}
