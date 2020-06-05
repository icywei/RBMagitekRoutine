﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Magitek.Utilities.Routines
{
    public class StateMachine<T> : IStateMachine where T : IComparable
    {
        private Dictionary<T, State<T>> mStateDict;
        private T mCurrentState;
        private T mNextState;
        private T mDefaultState;

        public void ResetToDefaultState()
        {
            Logger.WriteInfo("Resetting State Machine");
            mCurrentState = mDefaultState;
            mNextState = mDefaultState;
        }

        private void LogStateChange(T current, T next)
        {
            if (current.CompareTo(next) != 0)
            {
                Logger.WriteInfo($"State transition: {mCurrentState} -> {mNextState}");
            }
        }

        public async Task<bool> Pulse()
        {
            if (Casting.LastSpellSucceeded)
            {
                LogStateChange(mCurrentState, mNextState);
                mCurrentState = mNextState;
            }

            List<StateTransition<T>> transitions = mStateDict[mCurrentState].Transitions;
            foreach (StateTransition<T> st in transitions)
            {
                if (await st.TryDoAction())
                {
                    mNextState = st.NextState;
                    if (st.ImmediateTransition)
                    {
                        LogStateChange(mCurrentState, mNextState);
                        mCurrentState = mNextState;
                        return await Pulse();
                    }
                    return true; 
                }
            }

            return false;
        }

        public StateMachine(T defaultState, Dictionary<T, State<T>> stateDict)
        {
            mDefaultState = defaultState;
            mCurrentState = defaultState;
            mStateDict = stateDict;
        }
    }
}
