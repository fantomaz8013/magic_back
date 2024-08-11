import {createSlice} from "@reduxjs/toolkit";
import {cellKey} from "../../components/gameSession/map/map.utils";

export interface MoveState {
    movingUserId: string | null;
    path: LocationRequest[];
    pathHashSet: Record<string, boolean>;
}

export interface LocationRequest {
    x: number;
    y: number;
}

const initialState: MoveState = {
    movingUserId: null,
    path: [],
    pathHashSet: {},
};

const slicePrefix = 'move';

export const moveSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        setMoving: (state, action) => {
            state.movingUserId = action.payload;
            state.path = [];
            state.pathHashSet = {};
        },
        addMove: (state, action) => {
            if (!state.movingUserId) return;

            const move = action.payload as LocationRequest;
            state.path.push(move);
            state.pathHashSet = {...state.pathHashSet, [cellKey(move.y, move.x)]: true}
        },
        backTrackLastMove: (state) => {
            const lastMove = state.path.pop();
            if (!lastMove) return;

            const newPathHashSet = {...state.pathHashSet};
            delete newPathHashSet[cellKey(lastMove.y, lastMove.x)];
            state.pathHashSet = newPathHashSet;
        },
    },
});

export const {setMoving, addMove, backTrackLastMove} = moveSlice.actions;
