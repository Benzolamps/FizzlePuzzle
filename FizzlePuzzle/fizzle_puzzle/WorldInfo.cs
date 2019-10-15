using System.Diagnostics.CodeAnalysis;
using FizzlePuzzle.Core;
using FizzlePuzzle.Scene;
using UnityEngine;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WorldInfo
    {
        public string level_id => FizzleScene.LevelId;

        public string level_name => FizzleScene.LevelName;

        public int level_count => FizzleScene.LevelCount;

        public int level_index => FizzleScene.LevelIndex;

        public float level_dest_time => FizzleScene.LevelDestTime;

        public float global_time => FizzleScene.TimeCtrl.GlobalSeconds;

        public float rewind_time => FizzleScene.TimeCtrl.RewindSeconds;

        public int max_rewind_speed => (int) FizzleScene.TimeCtrl.MaxRewindSpeed;

        public float fork_time_remain => FizzleScene.TimeCtrl.ForkSecondsRemain;

        public string fork_activator => FizzleScene.ForkActivator ?? "None";

        public bool rewinding => FizzleScene.TimeCtrl.Rewinding;

        public bool forking => FizzleScene.TimeCtrl.Forking;

        public int max_rewind_time => FizzleScene.TimeCtrl.MaxSeconds;

        public int current_rewind_speed => FizzleScene.TimeCtrl.CurrentRewindSpeed;

        public float frames_per_second => 1.0F / Time.deltaTime;

        public event FizzleEvent begin_rewinding
        {
            add
            {
                FizzleScene.TimeCtrl.beginRewinding += value.Invoke;
            }
            remove
            {
                FizzleScene.TimeCtrl.beginRewinding -= value.Invoke;
            }
        }

        public event FizzleEvent end_rewinding
        {
            add
            {
                FizzleScene.TimeCtrl.endRewinding += value.Invoke;
            }
            remove
            {
                FizzleScene.TimeCtrl.endRewinding -= value.Invoke;
            }
        }

        public event FizzleEvent time_out_rewinding
        {
            add
            {
                FizzleScene.TimeCtrl.timeOutRewinding += value.Invoke;
            }
            remove
            {
                FizzleScene.TimeCtrl.timeOutRewinding -= value.Invoke;
            }
        }

        public event FizzleEvent begin_forking
        {
            add
            {
                FizzleScene.TimeCtrl.beginForking += value.Invoke;
            }
            remove
            {
                FizzleScene.TimeCtrl.beginForking -= value.Invoke;
            }
        }

        public event FizzleEvent end_forking
        {
            add
            {
                FizzleScene.TimeCtrl.endForking += value.Invoke;
            }
            remove
            {
                FizzleScene.TimeCtrl.endForking -= value.Invoke;
            }
        }

        public event FizzleEvent level_finished
        {
            add
            {
                FizzleScene.levelFinished += value.Invoke;
            }
            remove
            {
                FizzleScene.levelFinished -= value.Invoke;
            }
        }

        public FizzleCharacter first_fizzle_character { get; } = new FirstFizzleCharacter();

        public FizzleCharacter fork_fizzle_character { get; } = new ForkFizzleCharacter();

        internal WorldInfo()
        {
        }

        public bool get_fork_enabled() => FizzleScene.TimeCtrl.EnableFork;

        public void enable_fork()
        {
            FizzleScene.TimeCtrl.EnableForking();
        }
        
        public void disable_fork()
        {
            FizzleScene.TimeCtrl.DisableForking();
        }
    }
}
