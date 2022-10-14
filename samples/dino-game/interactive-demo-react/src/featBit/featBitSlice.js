import ffcClient from 'ffc-js-client-side-sdk';
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IFeatureFlag, IFeatureFlagChange, IOption, IVariationOption } from 'ffc-js-client-side-sdk/esm/types';

import { flagsDefaultValues } from './featBit';

const initialState = {
    flags: {}
};

export const featBitSlice = createSlice({
    name: 'featBit',
    initialState,
    reducers: {
        // Use the PayloadAction type to declare the contents of `action.payload`
        updateFeatBitFlags: (state, PayloadAction) => {
            const flags = new Proxy({}, {
                get(target, prop, receiver) {
                    if (typeof prop === 'symbol') {
                        return;
                    }
                    return ffcClient.variation(prop, flagsDefaultValues[prop] || '');
                }
            })

            state.flags = flags;
        }
    }
});

export const { updateFeatBitFlags } = featBitSlice.actions;

// The function below is called a selector and allows us to select a value from
// the state. Selectors can also be defined inline where they're used instead of
// in the slice file. For example: `useSelector((state: RootState) => state.counter.value)`
export const featBitFlags = (state) => {
    return state.featBit.flags;
}

export default featBitSlice.reducer;