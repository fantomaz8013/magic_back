import React, {useEffect, useState} from "react";
import {useGameSessionWS} from "../../utils/webSocket";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Dice from "./dice/Dice";
import Chat from "./chat/Chat";
import CharactersList from "./charactersList/CharactersList";
import CharacterLeftMenu from "./charactersList/CharacterLeftMenu";
import {useParams} from "react-router-dom";
import {
    FormControl,
    InputLabel,
    LinearProgress,
    Modal,
    Select,
    SelectChangeEvent,
    Snackbar,
    TextField
} from "@mui/material";
import {GameSessionStatusTypeEnum} from "../../models/websocket/gameSessionStatus";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../redux";
import {GameSessionCharacter} from "../../models/websocket/gameStartedInfo";
import {MasterCommands, MasterToPlayerCommand} from "./masterToPlayerCommand/MasterToPlayerCommand";
import Typography from "@mui/material/Typography";
import {CharacterCharacteristicIds} from "../../models/response/characterTemplateResponse";
import Button from "@mui/material/Button";
import MenuItem from "@mui/material/MenuItem";
import {useGetCharacteristicsQuery} from "../../redux/toolkit/api/characterApi";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {RequestedSaveThrowPassed, setRequestSaveThrow} from "../../redux/toolkit/slices/gameSessionSlice";

export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
    isOnline?: boolean;
}

export const SavingThrowEnum = {
    VeryEasy: 5,
    Easy: 10,
    Medium: 15,
    Hard: 20,
    VeryHard: 25,
    Impossible: 30,
}

export const SavingThrowEnumMapper = {
    [SavingThrowEnum.VeryEasy]: 'Очень лёгкая',
    [SavingThrowEnum.Easy]: 'Лёгкая',
    [SavingThrowEnum.Medium]: 'Средняя',
    [SavingThrowEnum.Hard]: 'Сложная',
    [SavingThrowEnum.VeryHard]: 'Очень сложная',
    [SavingThrowEnum.Impossible]: 'Практически невозможная',
}


const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    border: '2px solid #000',
    boxShadow: 24,
    p: 4,
};

interface FillableField {
    name: string;
    type: React.InputHTMLAttributes<unknown>['type'];
}

interface ChangingField {
    name: string;
    value: string;
    initialValue: string;
}


const fillableFields: Partial<Record<(keyof GameSessionCharacter), FillableField>> = {
    //id: {},
    // ownerId: {},
    name: {
        name: 'Имя',
        type: 'text',
    },
    characteristics: {
        name: 'Характеристики',
        type: 'text',
    },
    //characterClass: {},
    characterClassId: {
        name: 'ID класса',
        type: 'number',
    },
    // characterRace: {},
    characterRaceId: {
        name: 'ID рассы',
        type: 'number',
    },
    description: {
        name: 'Описание',
        type: 'text',
    },
    //gameSessionId: {},
    armor: {
        name: 'Класс брони',
        type: 'number',
    },
    avatarUrL: {
        name: 'Аватар',
        type: 'text',
    },
    abilitieIds: {
        name: 'Способности',
        type: 'text',
    },
    currentHP: {
        name: 'Текущий уровень здоровья',
        type: 'number',
    },
    currentShield: {
        name: 'Щит',
        type: 'number',
    },
    initiative: {
        name: 'Инициатива',
        type: 'number',
    },
    maxHP: {
        name: 'Максимальный уровень здоровья',
        type: 'number',
    },
    positionX: {
        name: 'Позиция X',
        type: 'number',
    },
    positionY: {
        name: 'Позиция Y',
        type: 'number',
    },
    speed: {
        name: 'Скорость',
        type: 'number',
    },
}

export default function GameSession() {
    const {state, ...api} = useGameSessionWS();
    const {gameSessionId} = useParams();

    const [command, setCommand] = React.useState<{ type: MasterCommands, userId: string } | null>(null);
    const [ref, setRef] = useState<HTMLElement | null>(null);
    const [snackBarMessage, setSnackBarMessage] = useState<string | null>(null);
    const [changingParameter, setChangingParameter] = useState<ChangingField | null>(null);
    const [characteristic, setCharacteristic] = React.useState<{ type: CharacterCharacteristicIds, value: number }>({
        type: CharacterCharacteristicIds.Strength,
        value: 5
    });

    const {data: characteristics} = useGetCharacteristicsQuery();
    const {data: currentUser} = useGetCurrentUserQuery();
    const dispatch = useDispatch<AppDispatch>();
    const gameSessionFullState = useSelector((state: RootState) => state.gameSession)

    const isGameMaster = (currentUser && currentUser.data && gameSessionFullState?.playerInfos[currentUser.data.id]?.isMaster) || false;

    useEffect(() => {
        if (gameSessionId && state === "Connected")
            api.joinGameSession(gameSessionId);
        return () => {
            if (state === "Connected")
                api.leaveGameSession();
        }
    }, [gameSessionId, state]);

    useEffect(() => {
        if (!!gameSessionFullState?.requestedSaveThrowPassed) {
            setSnackBarMessage(buildSnackBarMessage(gameSessionFullState.requestedSaveThrowPassed));
        }
    }, [gameSessionFullState.requestedSaveThrowPassed]);

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            {renderGameSessionPage()}
            {isGameMaster &&
                <MasterToPlayerCommand anchorEl={ref} onStartCommandClick={startCommand} onClose={closeMenu}/>}
            {renderModal()}
            <Snackbar
                open={!!snackBarMessage}
                autoHideDuration={6000} onClick={closeSnackBar}
                message={snackBarMessage}
            />
        </Box>
    )

    function buildSnackBarMessage(requestPassed: RequestedSaveThrowPassed) {
        const hasPassed = requestPassed.resultRollValue >= requestPassed.value;
        return `Проверка ` + (hasPassed ? 'пройдена' : 'провалена');
    }

    function closeSnackBar() {
        setSnackBarMessage(null);
    }

    function renderGameSessionPage() {
        if (!gameSessionFullState || !gameSessionFullState.gameSessionInfo)
            return (
                <LinearProgress color="inherit"/>
            )

        switch (gameSessionFullState.gameSessionInfo.gameSessionStatus) {
            case GameSessionStatusTypeEnum.WaitingForStart:
                return (
                    <>
                        <CharactersList/>
                        <Box sx={{
                            width: '95%',
                            display: 'flex',
                            flexDirection: ' row-reverse',
                        }}>
                            <Chat/>
                        </Box>
                    </>
                );
            case GameSessionStatusTypeEnum.InGame:
                return (
                    <>
                        <Grid container spacing={2}>
                            <Grid item xs={1}>
                                <Box sx={{display: 'flex', flexDirection: 'column'}}>
                                    {gameSessionFullState.gameSessionInfo.characters?.map((c: GameSessionCharacter) => {
                                        return (
                                            <CharacterLeftMenu
                                                onClick={onClick}
                                                key={c.id}
                                                character={{...c, name: `${c.name} (${c.ownerId})`}}
                                            />
                                        );
                                    })}
                                </Box>
                            </Grid>
                            <Grid item xs={11}>
                                <Box sx={{
                                    height: '85vh',
                                    display: 'flex',
                                    alignItems: 'end'
                                }}>
                                    <Grid container>
                                        <Grid item xs={1} sx={{
                                            display: 'flex',
                                            alignItems: 'end'
                                        }}>
                                            <Box>
                                                <Dice/>
                                            </Box>
                                        </Grid>
                                        <Grid item xs={11}>
                                            <Box sx={{
                                                width: '95%',
                                                display: 'flex',
                                                flexDirection: ' row-reverse',
                                            }}>
                                                <Chat/>
                                            </Box>
                                        </Grid>
                                    </Grid>
                                </Box>
                            </Grid>
                        </Grid>
                    </>
                );
            case GameSessionStatusTypeEnum.Finished:
                break;
        }
    }

    function renderModal() {
        const open = !!gameSessionFullState.requestedSaveThrow || !!command;

        const getCharacteristicTitle = (characterCharacteristicId: number) => {
            return characteristics!.data!.find(c => c.id === characterCharacteristicId)!.title;
        }
        const getCharacteristicValue = (characterCharacteristicValue: number) => {
            return SavingThrowEnumMapper[characterCharacteristicValue] || characterCharacteristicValue;
        }
        const requestedSaveThrow = gameSessionFullState?.requestedSaveThrow;
        return (
            <Modal
                open={open}
                onClose={closeModal}
            >
                <Box sx={style}>
                    {!!command && renderCommandInModal()}
                    {requestedSaveThrow &&
                        <>
                            <Typography variant="h6" component="h2">
                                Сделайте спас-бросок
                            </Typography>
                            <Typography>
                                Характеристика: {getCharacteristicTitle(requestedSaveThrow.characterCharacteristicId)}
                                <br/>
                                Сложность: {getCharacteristicValue(requestedSaveThrow.value)}
                            </Typography>
                            <Button sx={{mt: 2}} onClick={onSaveRollDiceClick}>Сделать бросок</Button>
                        </>
                    }
                </Box>
            </Modal>
        );
    }

    async function onSaveRollDiceClick() {
        await api.rollSaveDice();
        dispatch(setRequestSaveThrow(null));
    }

    function renderCommandInModal() {
        switch (command?.type) {
            case MasterCommands.RequestSaveThrow: {
                // const charId = gameSessionFullState.playerInfos[command.userId]
                // const info = gameSessionFullState.gameSessionInfo?.characters
                return (
                    <>
                        <Typography variant="h6" component="h2">
                            Запросить спас-бросок
                        </Typography>
                        <FormControl sx={{mt: 2}} fullWidth>
                            <InputLabel>Характеристика</InputLabel>
                            <Select
                                value={characteristic.type.toString()}
                                label="Характеристика"
                                onChange={setCharacteristicType}
                            >
                                {characteristics!.data!.map(({id, title}) => {
                                    return (
                                        <MenuItem
                                            key={id}
                                            value={id}>
                                            {title}
                                        </MenuItem>
                                    );
                                })}
                            </Select>
                        </FormControl>
                        <FormControl sx={{mt: 2}} fullWidth>
                            <InputLabel>Сложность спас-броска</InputLabel>
                            <Select
                                value={characteristic.value.toString()}
                                label="Сложность спас-броска"
                                onChange={setCharacteristicValue}
                            >
                                {Object.entries(SavingThrowEnum).map(([key, value]) => {
                                    return (
                                        <MenuItem
                                            key={key}
                                            value={value}>
                                            {SavingThrowEnumMapper[value]}
                                        </MenuItem>
                                    );
                                })}
                            </Select>
                        </FormControl>
                        <Button sx={{mt: 2}} onClick={onTestPlayerClick}>Запросить</Button>
                    </>
                );
            }
            case MasterCommands.Kick: {
                return (
                    <>
                        <Typography id="modal-modal-title" variant="h6" component="h2">
                            Вы уверены, что хотите выгнать
                            игрока {getUserLoginById(command.userId)}?
                        </Typography>
                        <Button sx={{mt: 2}} onClick={onKickPlayerClick}>Выгнать</Button>
                    </>
                );
            }
            case MasterCommands.ChangeCharacteristics: {
                return (
                    <>
                        <Typography id="modal-modal-title" variant="h6" component="h2">
                            Изменить параметры у игрока {getUserLoginById(command.userId)}
                        </Typography>
                        <FormControl sx={{mt: 2}} fullWidth>
                            <InputLabel>Параметр</InputLabel>
                            <Select
                                value={changingParameter?.value}
                                label="Параметр"
                                onChange={_setChangingParameter}
                            >
                                {Object.entries(fillableFields).map(([key, value]) => {
                                    return (
                                        <MenuItem
                                            key={key}
                                            value={key}>
                                            {value.name}
                                        </MenuItem>
                                    );
                                })}
                            </Select>
                        </FormControl>
                        {changingParameter?.value !== null && changingParameter?.value !== undefined &&
                            <FormControl sx={{mt: 2}} fullWidth>
                                <Typography>
                                    Начальное значение
                                    <br/>
                                    {changingParameter.initialValue}
                                </Typography>
                                <TextField
                                    value={changingParameter.value.toString()}
                                    onChange={_setChangingParameterValue}
                                />
                                <Box>
                                    <Button sx={{mt: 2}} onClick={saveChangedParameter}>Изменить</Button>
                                </Box>
                            </FormControl>}
                    </>
                );
            }
            case null: {
                return (<React.Fragment/>);
            }
        }
    }

    async function saveChangedParameter(){
        const character = gameSessionFullState.gameSessionInfo?.characters?.find(c => c.ownerId === command?.userId)!;
        const newChar :Record<string, string> = {};
        newChar[changingParameter!.name] = changingParameter!.value;
        await api.changeCharacter(character.id, newChar)
        setChangingParameter(null);
        closeModal();
    }

    function _setChangingParameter(event: SelectChangeEvent) {
        const parameterName = event.target.value as keyof GameSessionCharacter;
        const userId = command!.userId;
        const character = gameSessionFullState.gameSessionInfo?.characters?.find(c => c.ownerId === userId)!;
        setChangingParameter({
            name: parameterName,
            initialValue: character[parameterName]?.toString()!,
            value: character[parameterName]?.toString()!
        });
    }

    function _setChangingParameterValue(event: React.ChangeEvent<HTMLTextAreaElement>) {
        const parameterValue = event.currentTarget.value;
        setChangingParameter({...changingParameter!, value: parameterValue});
    }

    function setCharacteristicType(event: SelectChangeEvent) {
        const type = event.target.value as unknown as CharacterCharacteristicIds;
        setCharacteristic({...characteristic, type});
    }

    function setCharacteristicValue(e: SelectChangeEvent) {
        const value = parseInt(e.target.value);
        setCharacteristic({...characteristic, value});
    }

    function startCommand(command: MasterCommands) {
        setCommand({type: command, userId: ref!.id});
        closeMenu();
    }

    async function onKickPlayerClick() {
        await api.kick(command!.userId);
        closeModal();
    }

    async function onTestPlayerClick() {
        await api.requestSaveThrow(command!.userId, characteristic.type, characteristic.value);
        closeModal();
    }

    function getUserLoginById(userId: string) {
        return gameSessionFullState.playerInfos[userId].login;
    }

    function closeModal() {
        setChangingParameter(null);
        setCommand(null);
    }

    function closeMenu() {
        setRef(null);
    }

    function onClick(e: React.MouseEvent<HTMLDivElement>) {
        setRef(e.currentTarget);
    }
}
