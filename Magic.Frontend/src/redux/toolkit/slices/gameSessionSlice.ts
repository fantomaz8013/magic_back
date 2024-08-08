import {GameSessionInfo} from "../../../models/websocket/gameStartedInfo";
import {BaseGameSessionMessage} from "../../../models/websocket/ChatMessage";
import {PlayerInfo} from "../../../components/gameSession/GameSession";
import {createSlice} from "@reduxjs/toolkit";
import {CharacterCharacteristicIds} from "../../../models/response/characterTemplateResponse";

export interface GameSessionFullState {
    gameSessionInfo: GameSessionInfo | null;
    messages: BaseGameSessionMessage[];
    playerInfos: Record<string, PlayerInfo>;
    requestedSaveThrow: RequestedSaveThrow | null;
    requestedSaveThrowPassed: RequestedSaveThrowPassed | null;
}

export interface RequestedSaveThrow {
    characterCharacteristicId: CharacterCharacteristicIds;
    value: number;
    callerId: string;
    userId: string;
}

export interface RequestedSaveThrowPassed extends RequestedSaveThrow {
    resultRollValue: number;
}

const initialState: GameSessionFullState = {
    gameSessionInfo: null,
    messages: [],
    playerInfos: {},
    requestedSaveThrow: null,
    requestedSaveThrowPassed: null,
};

const slicePrefix = 'gameSession';

export const gameSessionSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        setGameSessionInfo: (state, action) => {
            state.gameSessionInfo = action.payload;
        },
        setPlayerInfos: (state, action) => {
            state.playerInfos = action.payload;
        },
        playerLeft: (state, action) => {
            const userId = action.payload;
            state.playerInfos = {
                ...state.playerInfos,
                [userId]: {
                    ...state.playerInfos[userId],
                    lockedCharacterId: null,
                    isOnline: false,
                }
            };
        },
        setRequestSaveThrow: (state, action) => {
            state.requestedSaveThrow = action.payload;
        },
        setRequestSaveThrowPassed: (state, action) => {
            state.requestedSaveThrowPassed = action.payload;
        },
        characterLocked: (state, action) => {
            const {userId, lockedCharacterTemplateId} = action.payload;
            state.playerInfos = {
                ...state.playerInfos,
                [userId]: {
                    ...state.playerInfos[userId],
                    lockedCharacterId: lockedCharacterTemplateId,
                }
            };
        },
        characterUnlocked: (state, action) => {
            const userId = action.payload;
            state.playerInfos = {
                ...state.playerInfos,
                [userId]: {
                    ...state.playerInfos[userId],
                    lockedCharacterId: null,
                }
            };
        },
        setMessages: (state, action) => {
            state.messages = action.payload;
        },
        addMessage: (state, action) => {
            state.messages.push(action.payload);
        }
    },
});

export const {
    setGameSessionInfo,
    setPlayerInfos,
    setMessages,
    addMessage,
    playerLeft,
    characterLocked,
    characterUnlocked,
    setRequestSaveThrow,
    setRequestSaveThrowPassed,
} = gameSessionSlice.actions;
