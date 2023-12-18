import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
    next,
    previous,
    currentStep,
} from '../guidesSlice';
import {
    featBitFlags
} from '../../../featBit/featBitSlice'
import { Button } from 'antd';
import '../Guides.css';
import GetStart from '../../../assets/imgs/get-started-in-portal.png';

export function StepComplete() {
    const currentStepIndex = useSelector(currentStep);
    const dispatch = useDispatch();
    const featureFlags = useSelector(featBitFlags);
    return (
        <div className="complete-step">
            <div className="title">
                <h1>Demo Complete</h1>
            </div>
            <div className="steps">
                <div >
                    <p>Congratulations! You have completed the Interactive Demo section of the Quick start section</p>
                    <p>Next, we recommend that you check out the <a href="https://docs.featbit.co/getting-started/connect-an-sdk">Connect an SDK</a> section for a quick overview of how to use FeatBit to connect the Feature flags we've connected in this demo!
                    </p>
                    <ul>
                        <li><a href="https://docs.featbit.co/getting-started/connect-an-sdk">Click here to go to the documentation page of Connect an SDK</a></li>
                        <li><a href="https://github.com/featbit/featbit-samples/tree/main/samples/dino-game/interactive-demo-vue"
                            target="_blank">Click here to download the source code for the Interactive Demo</a></li>
                    </ul>
                    <p>
                        Or return to the FeatBit portal and check the Quick Start Panel for the rest of lessons.
                        <br />
                        Note: The content completed in <a href="https://docs.featbit.co/getting-started/connect-an-sdk">Connect an SDK</a> will be used in most of the articles in How-to guides
                    </p>
                    <img src={GetStart} />
                </div>
                <div className="steps-action">
                    <Button onClick={() => dispatch(previous())}>
                    Previous
                </Button>
            </div>
        </div>
    </div >
    );
}
