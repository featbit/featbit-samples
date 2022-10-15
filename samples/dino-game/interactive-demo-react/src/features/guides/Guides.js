import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  // next,
  // previous,
  // currentStep,
  currentTask
} from './guidesSlice';
import './Guides.css';
import { Steps } from 'antd';
import { StepReleaseDinoGame } from './components/StepReleaseDinoGame';
import { StepGameDifficulty } from './components/StepGameDifficulty';
import { StepComplete } from './components/StepComplete';
const { Step } = Steps;

export function Guides() {
  const currentStepIndex = useSelector(currentTask);
  const dispatch = useDispatch();

  return (
    <div className="steps-wrapper">
      <Steps current={currentStepIndex}>
        <Step title="Release Dino Game" />
        <Step title="Change Game Difficulty" />
        <Step title="Demo complete" />
      </Steps>
      <div className="steps-content">
        {
          currentStepIndex == 0 ?
            <StepReleaseDinoGame /> : (currentStepIndex == 1 ?
              <StepGameDifficulty /> : <StepComplete />)
        }
      </div>
    </div>
  );
}
