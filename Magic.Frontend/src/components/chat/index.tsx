import React, {useEffect, useState} from "react";
import {ChatBox, ReceiverMessage, SenderMessage} from "mui-chat-box";
import {Avatar} from "@mui/material";
import * as signalR from "@microsoft/signalr";
import {createSignalRConnection} from "../../utils/webSocket";
import Button from "@mui/material/Button";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";

type Message = { author: string, message: string, sender?: boolean };

const defaultMessages = [
    {author: 'R', message: 'Приветствую в Magic! Напишите сообщение ниже и мастер скоро к вам прийдет, давать пизды!'},
];

export default function Chat() {
    const {data: currentUser} = useGetCurrentUserQuery();
    const [ws, setWs] = useState<signalR.HubConnection | null>(null);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const [messages, setMessages] = useState<Message[]>([...defaultMessages]);
    useEffect(() => {
        const connection = createSignalRConnection('ws', signalR.LogLevel.Information);
        connection.start();
        setWs(connection);
    }, [])

    useEffect(() => {
        ws?.off("messageReceived");
        ws?.on("messageReceived", messageReceived);
    }, [currentUser])

    function messageReceived(author: string, message: string) {
        const isSender = author === currentUser!.data!.login ;
        const newMessage = {author, message, sender: isSender};
        setMessages(prevState => [...prevState, newMessage]);
    }

    async function sendMessage() {
        if (!ws || currentMessage.length === 0) return;

        setCurrentMessage('');
        await ws.invoke('NewMessage', currentUser?.data?.login || 'А', currentMessage);
    }

    return (
        <>
            <ChatBox>
                {messages.map(({author, message, sender}, index) => {
                        if (sender) {
                            return (
                                <SenderMessage key={index} avatar={<Avatar>{author.slice(0, 1)}</Avatar>}>
                                    {message}
                                </SenderMessage>
                            )
                        }
                        return (
                            <ReceiverMessage key={index} avatar={<Avatar>{author.slice(0, 1)}</Avatar>}>
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
