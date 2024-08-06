import React, {useState} from "react";
import {ChatBox, ReceiverMessage, SenderMessage} from "mui-chat-box";
import {Avatar} from "@mui/material";
import {SignalRProps, useSignalR, WSActions, WSEvents} from "../../utils/webSocket";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {
    BaseGameSessionMessage, ChatGameSessionMessage, CubeTypeEnum, DiceGameSessionMessage,
    GameSessionMessageTypeEnum,
    ServerGameSessionMessage,
} from "../../models/websocket/ChatMessage";
import Typography from "@mui/material/Typography";
import Dice from "../dice/Dice";

export interface ChatProps {
    gameSessionId: string;
}

export default function Chat(props: ChatProps) {
    const {data: currentUser} = useGetCurrentUserQuery();
    const ws = useSignalR(createSignalRConfig());
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [messages, setMessages] = useState<BaseGameSessionMessage[]>([]);

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Typography mb={4} component="h1" variant="h5">
                Save Halsin player's chat
            </Typography>
            <ChatBox>
                {messages.map(renderMessage)}
            </ChatBox>
            <Box>
                <TextField
                    type={'text'}
                    margin="normal"
                    required
                    fullWidth
                    value={currentMessage}
                    onKeyDown={onKeyDown}
                    onChange={onMessageChange}
                />
                <Button onClick={sendMessage}>SEND</Button>
                <Dice onDiceRoll={rollDice}/>
            </Box>
        </Box>
    );

    async function rollDice() {
        await ws.invoke(WSActions.rollDice, CubeTypeEnum.D6);
    }

    function renderMessage(baseMessage: BaseGameSessionMessage) {
        let mes, login, isSender;
        switch (baseMessage.gameSessionMessageTypeEnum) {
            case GameSessionMessageTypeEnum.Server:
                const serverMessage = baseMessage as ServerGameSessionMessage;
                mes = serverMessage.message;
                login = 'S';
                isSender = false;
                break;
            case GameSessionMessageTypeEnum.Chat:
                const chatMessage = baseMessage as ChatGameSessionMessage;
                mes = chatMessage.message;
                login = chatMessage.author.login;
                isSender = chatMessage.authorId === currentUser!.data!.id
                break;
            case GameSessionMessageTypeEnum.Dice:
                const diceMessage = baseMessage as DiceGameSessionMessage;
                mes = `Player ${diceMessage.author.login} rolled ${diceMessage.roll} on ${CubeTypeEnum[diceMessage.cubeTypeEnum]}`
                login = diceMessage.author.login;
                isSender = diceMessage.authorId === currentUser!.data!.id
                break;
        }
        const Message = isSender
            ? SenderMessage
            : ReceiverMessage;
        return (
            <Message key={baseMessage.id} avatar={<Avatar>{login?.slice(0, 1) || 'Й'}</Avatar>}>
                {mes}
            </Message>
        )
    }

    function createSignalRConfig(): SignalRProps {
        return {
            beforeStart: (ws) => {
                ws.on(WSEvents.messageReceived, messageReceived);
                ws.on(WSEvents.historyReceived, historyReceived);
            },
            afterStart: async (ws) => {
                await ws.invoke(WSActions.joinGameSession, props.gameSessionId);
            },
            beforeStop: async (ws) => {
                await ws.invoke(WSActions.leaveGameSession, props.gameSessionId);
            }
        }
    }

    function messageReceived(message: BaseGameSessionMessage) {
        setMessages(prevState => [...prevState, message]);
    }

    function historyReceived(newMessages: BaseGameSessionMessage[]) {
        console.log(newMessages);
        setMessages(newMessages);
    }

    async function sendMessage() {
        if (!ws || currentMessage.length === 0) return;

        setCurrentMessage('');
        await ws.invoke('NewMessage', currentMessage);
    }

    function onMessageChange(e: React.ChangeEvent<HTMLInputElement>) {
        const val = e.target.value;
        setCurrentMessage(val);
    }

    async function onKeyDown(e: React.KeyboardEvent<HTMLDivElement>) {
        if (e.key === 'Enter')
            await sendMessage();
    }
}
