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


export default function Chat() {
    const {data: currentUser} = useGetCurrentUserQuery();
    const [ws, setWs] = useState<signalR.HubConnection | null>(null);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [messages, setMessages] = useState<ChatMessage[]>([]);
    const token = useSelector((state: RootState) => state.auth.token)

    useEffect(() => {
        setupConnection()
            .then(c => setWs(c));
    }, [])

    useEffect(() => {
        return () => {
            ws && closeConnection(ws);
        }
    }, [ws])


    return (
        <>
            <ChatBox>
                {messages.map(({userLogin, message, messageId}) => {
                        if (userLogin === currentUser!.data!.login) {
                            return (
                                <SenderMessage key={messageId} avatar={<Avatar>{userLogin.slice(0, 1)}</Avatar>}>
                                    {message}
                                </SenderMessage>
                            )
                        }
                        return (
                            <ReceiverMessage key={messageId} avatar={<Avatar>{userLogin.slice(0, 1)}</Avatar>}>
                                {message}
                            </ReceiverMessage>
                        )
                    }
                )}
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

    async function setupConnection() {
        const connection = createSignalRConnection('ws', token!);
        connection.on("messageReceived", messageReceived);
        connection.on("historyReceived", historyReceived);
        await connection.start();
        await connection.invoke('JoinRoom', 'default');
        return connection;
    }

    async function closeConnection(ws: signalR.HubConnection) {
        await ws.invoke('LeaveRoom', 'default');
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

    function onKeyDown(e: React.KeyboardEvent<HTMLDivElement>) {
        if (e.key === 'Enter')
            sendMessage();
    }
}
