// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.AwakeModeHelper.cs
// Created on: 20210205
// -----------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace BatteryStatus
{
    public static class AwakeModeHelper
    {
        // ReSharper disable InconsistentNaming

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS        = 0x80000000,
            ES_DISPLAY_REQUIRED  = 0x00000002,
            ES_SYSTEM_REQUIRED   = 0x00000001

            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        public static bool ForceSystemAwake() =>
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS       |
                                    EXECUTION_STATE.ES_DISPLAY_REQUIRED |
                                    EXECUTION_STATE.ES_SYSTEM_REQUIRED  |
                                    EXECUTION_STATE.ES_AWAYMODE_REQUIRED) != 0;

        public static bool ResetSystemDefault() => SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS) != 0;
    }
}