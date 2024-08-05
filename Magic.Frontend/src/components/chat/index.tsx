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
        const connection = createSignalRConnection('ws', token!, signalR.LogLevel.Information);
        setWs(connection);
        connection.on("messageReceived", messageReceived);
        connection.on("historyReceived", historyReceived);
        connection.start();
        return () => {
            connection.stop();
        }
    }, [])

    function messageReceived(message: ChatMessage) {
        setMessages(prevState => [...prevState, message]);
    }

    function historyReceived(messages: ChatMessage[]) {
        setMessages(prevState => [...messages]);
    }

    async function sendMessage() {
        if (!ws || currentMessage.length === 0) return;

        setCurrentMessage('');
        await ws.invoke('NewMessage', currentMessage);
    }

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

    function onMessageChange(e: React.ChangeEvent<HTMLInputElement>) {
        const val = e.target.value;
        setCurrentMessage(val);
    }

    function onKeyDown(e: React.KeyboardEvent<HTMLDivElement>) {
        if (e.key === 'Enter')
            sendMessage();
    }
}
