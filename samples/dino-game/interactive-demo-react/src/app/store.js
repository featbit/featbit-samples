import { configureStore } from '@reduxjs/toolkit';
import guidesReducer from '../features/guides/guidesSlice';
import featBitSlice from '../featBit/featBitSlice';

export const store = configureStore({
  reducer: {
    guides: guidesReducer,
    featBit: featBitSlice
  },
});
