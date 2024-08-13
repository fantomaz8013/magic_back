import React, {useState, useEffect, useRef} from "react";
import {
    BaseGameSessionMessage, ChatGameSessionMessage, CubeTypeEnum, DiceGameSessionMessage,
    GameSessionMessageTypeEnum,
    ServerGameSessionMessage
} from "../../../models/websocket/chatMessage";
import {ChatBox, ReceiverMessage, SenderMessage} from "mui-chat-box";
import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import {Avatar} from "@mui/material";
import {useGetCurrentUserQuery} from "../../../redux/api/userApi";
import Typography from "@mui/material/Typography";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {socket} from "../../../webSocket/webSocket";


export default function Chat() {
    const scrollRef = useRef<HTMLDivElement | null>(null);
    const [currentMessage, setCurrentMessage] = useState<string>('');
    const messages = useSelector((state: RootState) => state.gameSession.messages)
    const {data: currentUser,} = useGetCurrentUserQuery();

    useEffect(() => {
        if (scrollRef.current) {
            (scrollRef.current?.parentNode as HTMLElement).scrollTop = scrollRef.current.offsetTop;
        }
    }, [messages]);

    return (
        <Box sx={{height: 300, overflowY: 'scroll'}}>
            <ChatBox>
                {messages.map(renderMessage)}
            </ChatBox>
            <div ref={scrollRef}/>
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
    );

    function renderMessage(baseMessage: BaseGameSessionMessage, i: number) {
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
            <Message key={baseMessage.id} avatar={<Avatar>{login?.slice(0, 1)}</Avatar>}>
                <Typography sx={{whiteSpace: 'wrap', wordBreak: 'break-word'}}>
                    {mes}
                </Typography>
            </Message>
        )
    }

    async function sendMessage() {
        if (currentMessage.length === 0) return;

        setCurrentMessage('');
        await socket?.newMessage(currentMessage);
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
