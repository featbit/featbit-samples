
import React from 'react';
import { useDispatch } from 'react-redux';
import fbClient from "featbit-js-client-sdk";
import { updateFeatBitFlags } from './featBitSlice';


export const flagsDefaultValues = {
    "game-runner": "false",
    "difficulty-mode": "easy"
}

export const FeatBit = (props) => {
    const { children } = props;
    let envkey = window.location.search.substring(1).replace('envKey=', ''); 

    fbClient.init({
        secret: envkey,
        api: "http://localhost:5100",
        user: {
            keyId: 'my-user',
            name: 'my user',
            customizedProperties: [
                {
                    "name": "frequency",
                    "value": "3.5"
                },
                {
                    "name": "subLevel",
                    "value": "Free"
                },
                {
                    "name": "orgId",
                    "value": "org0001"
                },
            ]
        },
    });

    const dispatch = useDispatch();

    fbClient.on("ff_update", (changes) => {
        return changes.length ?  dispatch(updateFeatBitFlags({})) : null;
    });

    fbClient.waitUntilReady().then((changes) => {
        return changes.length ? dispatch(updateFeatBitFlags({})) : null;
    });

    return (
        <>
            {children}
        </>
    );
};
