import {GameSessionInfo} from "../../../models/websocket/gameStartedInfo";
import {BaseGameSessionMessage} from "../../../models/websocket/ChatMessage";
import {PlayerInfo} from "../../../components/gameSession";
import {createSlice} from "@reduxjs/toolkit";

export interface GameSessionFullState {
    gameSessionInfo: GameSessionInfo | null;
    messages: BaseGameSessionMessage[];
    playerInfos: Record<string, PlayerInfo>;
}

const initialState: GameSessionFullState = {
    gameSessionInfo: null,
    messages: [],
    playerInfos: {},
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
                    isOnline: null,
                }
            };
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
} = gameSessionSlice.actions;
