import { configureStore } from '@reduxjs/toolkit';
import counterReducer from '../features/counter/counterSlice';
import guidesReducer from '../features/guides/guidesSlice';
import featBitSlice from '../featBit/featBitSlice';

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    guides: guidesReducer,
    featBit: featBitSlice
  },
});
