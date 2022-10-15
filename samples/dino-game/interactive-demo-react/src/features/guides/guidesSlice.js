import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { fetchCount } from './guidesAPI';

const initialState = {
  currentStep: 0,
};

export const guidesSlice = createSlice({
  name: 'guides',
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    next: (state) => {
      state.currentStep += 1;
    },
    previous: (state) => {
      state.currentStep -= 1;
    },
    toTask2: (state) => {
      state.currentStep = 2;
    },
    toTask3: (state) => {
      state.currentStep = 3;
    }
  }
});

export const { next, previous, toTask2, toTask3 } = guidesSlice.actions;

// The function below is called a selector and allows us to select a value from
// the state. Selectors can also be defined inline where they're used instead of
// in the slice file. For example: `useSelector((state: RootState) => state.counter.value)`
export const currentStep = (state) => state.guides.currentStep;
export const currentTask = (state) => {
  if (state.guides.currentStep < 2)
    return 0;
  else if (state.guides.currentStep == 2)
    return 1;
  return 2;
}

export default guidesSlice.reducer;
