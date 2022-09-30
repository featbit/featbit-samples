import ffcClient from 'ffc-js-client-side-sdk';
import { flagsDefaultValues } from './config';

export const createFlagsProxy = () => {
    return new Proxy({}, {
        get(target, prop, receiver) {
            if (typeof prop === 'string' && !prop.startsWith('__v_')) {
                const variation = ffcClient.variation(prop, flagsDefaultValues[prop] || '');

                return variation;
            }

            return '';
        }
    })
}