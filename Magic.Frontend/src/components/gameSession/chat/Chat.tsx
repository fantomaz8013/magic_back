import React, { useState, useEffect, useRef } from "react";
import {
  BaseGameSessionMessage,
  ChatGameSessionMessage,
  CubeTypeEnum,
  DiceGameSessionMessage,
  GameSessionMessageTypeEnum,
  ServerGameSessionMessage,
} from "../../../models/websocket/chatMessage";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { useGetCurrentUserQuery } from "../../../redux/api/userApi";
import { useSelector } from "react-redux";
import { RootState } from "../../../redux/redux";
import { socket } from "../../../webSocket/webSocket";
import { className } from "./chat.style";
import { IconBrandTelegram } from "@tabler/icons-react";
import { Style } from "./Style";

export default function Chat() {
  const scrollRef = useRef<any>(null);
  const [currentMessage, setCurrentMessage] = useState<string>("");
  const messages = useSelector(
    (state: RootState) => state.gameSession.messages
  );
  const { data: currentUser } = useGetCurrentUserQuery();

  useEffect(() => {
    if (scrollRef.current) {
      (scrollRef.current?.parentNode as HTMLElement).scrollTop =
        scrollRef.current.offsetTop;
    }
  }, [messages]);

  return (
    <Box style={className.chatBox} className={'chat'}>
      <div style={className.chatContainer}>
        {messages.map(renderMessage)}
        <div ref={scrollRef} />
      </div>
      <Style />
      <div style={className.chatController}>
        <input
          style={className.input}
          placeholder="Введите сообщение"
          value={currentMessage}
          onKeyDown={onKeyDown}
          onChange={onMessageChange}
        />
        {currentMessage && (
          <Button onClick={sendMessage}>
            <IconBrandTelegram style={className.chatIcon} />
          </Button>
        )}
      </div>
    </Box>
  );

  function renderMessage(baseMessage: BaseGameSessionMessage, i: number) {
    let mes, login, isSender;
    switch (baseMessage.gameSessionMessageTypeEnum) {
      case GameSessionMessageTypeEnum.Server:
        const serverMessage = baseMessage as ServerGameSessionMessage;
        mes = serverMessage.message;
        login = "server";
        isSender = false;
        break;
      case GameSessionMessageTypeEnum.Chat:
        const chatMessage = baseMessage as ChatGameSessionMessage;
        mes = chatMessage.message;
        login = chatMessage.author.login;
        isSender = chatMessage.authorId === currentUser!.data!.id;
        break;
      case GameSessionMessageTypeEnum.Dice:
        const diceMessage = baseMessage as DiceGameSessionMessage;
        mes = `Player ${diceMessage.author.login} rolled ${
          diceMessage.roll
        } on ${CubeTypeEnum[diceMessage.cubeTypeEnum]}`;
        login = diceMessage.author.login;
        isSender = diceMessage.authorId === currentUser!.data!.id;
        break;
    }

    return (
      <div
        key={baseMessage.id}
        style={
          isSender
            ? { ...className.message, ...className.senderMessage }
            : { ...className.message, ...className.receiverMessage }
        }
      >
        {login !== "server" && (
          <div style={className.avatar}>{login?.slice(0, 1)}</div>
        )}
        <div
          style={
            login === "server" ? className.serverMessage : className.userMessage
          }
        >
          {mes}
        </div>
      </div>
    );
  }

  async function sendMessage() {
    setCurrentMessage("");
    await socket?.newMessage(currentMessage);
  }

  function onMessageChange(e: React.ChangeEvent<HTMLInputElement>) {
    const val = e.target.value;
    setCurrentMessage(val);
  }

  async function onKeyDown(e: React.KeyboardEvent<HTMLDivElement>) {
    if (e.key === "Enter") await sendMessage();
  }
}
