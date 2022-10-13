import React, { useState, useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import Runner from './DinoGameRunner.js';

export function DinoGameCore() {
    useEffect(() => {
        new Runner(".interstitial-wrapper", 6);
    });
    return (
        <div id="dino-game-core" className="interstitial-wrapper">
            <div style={{ position: "relative" }}>
                <h2 >Welcome to DINO GAME! Mode Level :
                    {/* <strong style="text-transform:uppercase;">{{ difficulty }}</strong> */}
                </h2>
                <p >Press Space key to start the game</p>
            </div>
            <div id="main-content">
                <div className="icon icon-offline" alt=""></div>
            </div>
            <div id="offline-resources">
                <img id="offline-resources-1x" src="assets/default_100_percent/100-offline-sprite.png" />
                <img id="offline-resources-2x" src="assets/default_200_percent/200-offline-sprite.png" />
            </div>
        </div>
    );
}
