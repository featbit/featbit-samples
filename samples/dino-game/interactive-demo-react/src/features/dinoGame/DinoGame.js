import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { DinoGameCore } from './DinoGameCore';
import './DinoGame.css';

export function DinoGame() {
    useEffect(() => {
    });
    return (
        <div className="dino-game-wrapper">
            <DinoGameCore />
            {/* <div className="comingsoon-wrapper" v-if="featBitStore.flags['game-runner'] == false">
                <h1>Swith "game-runner" feature flag to ON to release Dino Game</h1>
            </div> */}
        </div>
    );
}
