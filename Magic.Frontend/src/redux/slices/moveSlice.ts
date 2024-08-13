import {createSlice} from "@reduxjs/toolkit";

export interface MoveState {
    movingCharacterId: string | null;
    path: LocationRequest[];
}

export interface LocationRequest {
    x: number;
    y: number;
}

const initialState: MoveState = {
    movingCharacterId: null,
    path: [],
};

const slicePrefix = 'move';

export const moveSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        setMoving: (state, action) => {
            state.movingCharacterId = action.payload;
            state.path = [];
        },
        addMove: (state, action) => {
            if (!state.movingCharacterId) return;

            const move = action.payload as LocationRequest;
            state.path.push(move);
        },
        backTrackLastMove: (state) => {
            state.path.pop();
        },
    },
});

export const {setMoving, addMove, backTrackLastMove} = moveSlice.actions;
