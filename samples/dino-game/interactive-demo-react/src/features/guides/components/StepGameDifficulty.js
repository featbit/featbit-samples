import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    next,
    previous,
    currentStep,
    toTask3,
} from '../guidesSlice';
import {
    featBitFlags
} from '../../../featBit/featBitSlice'
import { Button } from 'antd';
import '../Guides.css';
import FFSwitchDifficulty from '../../../assets/imgs/ff-switch-difficulty.png';

export function StepGameDifficulty() {
    const currentStepIndex = useSelector(currentStep);
    const dispatch = useDispatch();
    const featureFlags = useSelector(featBitFlags);
    return (
        <div className="game-difficulty">
            <div className="title">
                <h1>Change Game Difficulty</h1>
            </div>
            <div className="steps">
                <div >
                    <p>// Task 2: Change Difficulty Mode</p>
                    <p>The game launches in easy mode. You can use anothor flag to enable
                        'normal' mode or 'hard' mode.
                    </p>
                    <p>
                        Back to the flag list, go to targeting page of feature flag "difficulty mode".
                        Find the 'Default rule' dropdown. Choose a flag variation (<strong>"hard"</strong> or
                        <strong>"normal"</strong> but not "easy") from the dropdown and save.
                    </p>
                    <img src={FFSwitchDifficulty} />
                </div>
                <div className="steps-action">
                    <Button onClick={() => dispatch(previous())}>
                        Previous
                    </Button>
                    {
                        featureFlags['difficulty-mode'] !== 'easy' ?
                            <Button type="primary" style={{ float: "right" }}
                                onClick={() => dispatch(toTask3())}>Next Task</Button> : null
                    }
                </div>


                {
                    featureFlags['difficulty-mode'] !== 'easy' ?
                        <div style={{ marginTop: "20px", marginBottom: "5px" }}>
                            <p style={{ fontSize: "27px", marginBottom: "0px" }}>
                                <span style={{ color: "yellow", fontWeight: "600", marginRight: "10px" }}>Success !</span>
                                The game difficulty mode has been changed. Try the game once again !
                            </p>
                        </div> : null
                }

            </div>
        </div>
    );
}
