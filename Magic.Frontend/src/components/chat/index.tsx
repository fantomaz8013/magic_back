import React, {useEffect, useState} from "react";
import {ChatBox, ReceiverMessage, SenderMessage} from "mui-chat-box";
import {Avatar} from "@mui/material";
import * as signalR from "@microsoft/signalr";
import {createSignalRConnection} from "../../utils/webSocket";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {useSelector} from "react-redux";
import {RootState} from "../../redux";
import {ChatMessage} from "../../models/websocket/ChatMessage";
import Typography from "@mui/material/Typography";


export default function Chat() {
    const {data: currentUser} = useGetCurrentUserQuery();
    const [ws, setWs] = useState<signalR.HubConnection | null>(null);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const token = useSelector((state: RootState) => state.auth.token)

    useEffect(() => {
        async function setupConnection(roomName: string) {
            if (ws) return null;

            const connection = createSignalRConnection('ws', token!);
            connection.on("messageReceived", messageReceived);
            connection.on("historyReceived", historyReceived);
            await connection.start();
            await connection.invoke('JoinRoom', roomName);
            return connection;
        }

        setupConnection('default')
            .then(c => c && setWs(c));

        return () => {
            ws && closeConnection(ws, 'default');
        }
    }, [token, ws])

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
            </Box>
        </Box>
    );

    function renderMessage({userLogin, message, messageId}: ChatMessage) {
        const Message = userLogin === currentUser!.data!.login
            ? SenderMessage
            : ReceiverMessage;
        return (
            <Message key={messageId} avatar={<Avatar>{userLogin.slice(0, 1)}</Avatar>}>
                {message}
            </Message>
        )
    }

    async function closeConnection(ws: signalR.HubConnection, roomName: string) {
        await ws.invoke('LeaveRoom', roomName);
        await ws.stop();
    }

    function messageReceived(message: ChatMessage) {
        setMessages(prevState => [...prevState, message]);
    }

    function historyReceived(newMessages: ChatMessage[]) {
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
