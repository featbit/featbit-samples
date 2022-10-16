import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    next,
    previous,
    toTask2,
    currentStep,
} from '../guidesSlice';
import { featBitFlags } from '../../../featBit/featBitSlice'
import { Button } from 'antd';
import '../Guides.css';
import TheFlagList from '../../../assets/imgs/the-flags-list.png'
import FFSwitchOn from '../../../assets/imgs/ff-switch-on.png'

export function StepReleaseDinoGame() {
    const currentStepIndex = useSelector(currentStep);
    const dispatch = useDispatch();
    const featureFlags = useSelector(featBitFlags);
    return (
        <div className="release-runner">
            <div className="title">
                <h1>Releasing Dino Game</h1>
            </div>
            <div className="steps">
                {
                    currentStepIndex == 0 ?

                        <div className="step1">
                            <p>// Set up</p>
                            <p>We're connecting your FeatBit account to Dino Game so you can use
                                feature flags to release and manage the game.
                            </p>
                            <p>
                                Using your client-side ID to initiate a connection...
                            </p>
                            <p>
                                <span style={{ color: "yellow", fontWeight: "600", marginRight: "15px" }}>Success !</span>
                                Now that FeatBit is connected, we can release the Dino Game now.
                            </p>
                        </div>
                        :
                        (currentStepIndex == 1 ?

                            <div className="step2" >
                                <p>// Release Dino Game</p>
                                <p>Back to the flags list portal which contains "game-runner" feature flag.
                                    Click on the name or detail link to go to the feature flag targeting page.</p>
                                <img src={TheFlagList} />
                                <p>Switch feature flag from Off to ON. The feature flag will serve variation "true"
                                    to the Dino Game. It means the Dino Game (below) will be released immediately.
                                </p>
                                <img src={FFSwitchOn} />
                            </div> : null
                        )
                }

                <div className="steps-action">
                    {
                        currentStepIndex == 1 ?
                            <Button onClick={() => dispatch(previous())}>
                                Previous
                            </Button> : null
                    }

                    {
                        currentStepIndex == 0 ?
                            <Button onClick={() => dispatch(next())}>
                                Next
                            </Button> : null
                    }
                    {
                        featureFlags['game-runner'] == true ?
                            <Button
                                onClick={() => dispatch(toTask2())}
                                type="primary" style={{ float: "right" }}>Next Task</Button> : null
                    }

                </div>

                {
                    featureFlags['game-runner'] == true ?
                        <div className="step2"
                            style={{ marginTop: "20px", marginBottom: "5px" }}>
                            <p style={{ fontSize: "27px", marginBottom: "0px" }}>
                                <span style={{ color: "yellow", fontWeight: "600", marginRight: "10px" }}>Success !</span>
                                Dino Game was released. Look ↓↓↓↓↓↓ <br />
                                Press space key to start the game
                            </p>
                        </div> : null
                }
            </div >
        </div >
    );
}
