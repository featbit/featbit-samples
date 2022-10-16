import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { DinoGameCore } from './DinoGameCore';
import { featBitFlags } from '../../featBit/featBitSlice'
import './DinoGame.css';

export function DinoGame() {
    useEffect(() => {
    });
    const featureFlags = useSelector(featBitFlags);
    return (
        <div className="dino-game-wrapper">
            {
                featureFlags["game-runner"] == true ? 

                <DinoGameCore difficulty={featureFlags["difficulty-mode"]} />
                : 
                <div className="comingsoon-wrapper" v-if="featBitStore.flags['game-runner'] == false">
                    <h1>Swith "game-runner" feature flag to ON to release Dino Game</h1>
                </div>
            }
        </div>
    );
}
