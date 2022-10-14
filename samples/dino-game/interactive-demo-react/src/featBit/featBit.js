import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { RootState, store } from '../app/store';
import ffcClient from "ffc-js-client-side-sdk";
import {
    updateFeatBitFlags,
    featBitFlags,
} from './featBitSlice';


export const flagsDefaultValues = {
    "game-runner": "false",
    "difficulty-mode": "easy"
}


export const FeatBit = (props) => {
    const { children } = props;

    let envkey = window.location.search.substring(1).replace('key=', ''); // http://localhost:5173?key=ZTczLTFiMTctNCUyMDIyMDkyOTA1MDUwOV9fMTU5X18yMzVfXzQ1MV9fZGVmYXVsdF9lY2RjMA==

    ffcClient.init({
        secret: envkey,
        user: {
            id: 'my-user',
            userName: 'my user',
            customizedProperties: [
                {
                    "name": "kamar",
                    "value": "100"
                },
                {
                    "name": "Kamar",
                    "value": "100"
                },
                {
                    "name": "frequency",
                    "value": "3.5"
                },
                {
                    "name": "subLevel",
                    "value": "Free"
                },
            ]
        },
    });

    const dispatch = useDispatch();
    ffcClient.on("ff_update", (changes) => {
        return changes.length ? 
                dispatch(updateFeatBitFlags({})) : null;
    });
    ffcClient.waitUntilReady().then((changes) => {
        return changes.length ? 
                dispatch(updateFeatBitFlags({})) : null;
    });

    return (
        <>
            {children}
        </>
    );
};
