import React, {useEffect, useState} from "react";
import * as signalR from "@microsoft/signalr";
import {
    BaseGameSessionMessage, ChatGameSessionMessage, CubeTypeEnum, DiceGameSessionMessage,
    GameSessionMessageTypeEnum,
    ServerGameSessionMessage
} from "../../../models/websocket/ChatMessage";
import {ChatBox, ReceiverMessage, SenderMessage} from "mui-chat-box";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import {Avatar} from "@mui/material";
import {useGetCurrentUserQuery} from "../../../redux/toolkit/api/userApi";
import {WSActions, WSEvents} from "../../../utils/webSocket";
import Typography from "@mui/material/Typography";

export interface ChatProps {
    ws: signalR.HubConnection;
}

export default function Chat(props: ChatProps) {
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [messages, setMessages] = useState<BaseGameSessionMessage[]>([]);
    const {data: currentUser,} = useGetCurrentUserQuery();

    useEffect(() => {
        props.ws.on(WSEvents.messageReceived, messageReceived);
        props.ws.on(WSEvents.historyReceived, historyReceived);
        return () => {
            props.ws.off(WSEvents.messageReceived);
            props.ws.off(WSEvents.historyReceived);
        }
    }, []);

    return (
        <>
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
            </Box>
        </>
    );

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

    function messageReceived(message: BaseGameSessionMessage) {
        setMessages(prevState => [...prevState, message]);
    }

    function historyReceived(newMessages: BaseGameSessionMessage[]) {
        setMessages(newMessages);
    }

    async function sendMessage() {
        if (currentMessage.length === 0) return;

        setCurrentMessage('');
        await props.ws.invoke(WSActions.newMessage, currentMessage);
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
