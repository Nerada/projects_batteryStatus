// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.AwakeModeHelper.cs
// Created on: 20210205
// -----------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace BatteryStatus
{
    public class AwakeModeHelper
    {
        // ReSharper disable InconsistentNaming

        // The return value of SetThreadExecutionState is used to indicate success or failure.
        // If the function succeeds, the return value is the previous thread execution state. If the function fails, the return value is NULL

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        private EXECUTION_STATE? _initialState;

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

        public bool ForceSystemAwake()
        {
            EXECUTION_STATE oldState = SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS       |
                                                               EXECUTION_STATE.ES_DISPLAY_REQUIRED |
                                                               EXECUTION_STATE.ES_SYSTEM_REQUIRED  |
                                                               EXECUTION_STATE.ES_AWAYMODE_REQUIRED);

            if (_initialState == null && oldState != 0) _initialState = oldState;

            return oldState != 0;
        }

        public bool ResetSystemDefault() => _initialState is { } initialState && initialState != 0 && SetThreadExecutionState(initialState) != 0;
    }
}