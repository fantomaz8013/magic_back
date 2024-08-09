import React, {useEffect, useState} from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import {Divider, Modal} from "@mui/material";
import Box from "@mui/material/Box";
import {
    MasterCommands,
    MasterToPlayerCommandProps,
} from "./MasterUtils.types";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {useGetCurrentUserQuery} from "../../../redux/toolkit/api/userApi";
import {ModalStyle} from "./MasterUtils.utils";
import {RequestSaveThrowModal} from "./commandModals/RequestSaveThrowModal";
import {KickModal} from "./commandModals/KickModal";
import {ChangeCharacteristicsModal} from "./commandModals/ChangeCharacteristicsModal";
import {ModalProps} from "./commandModals/commandModals.types";


export function MasterUtils({anchorEl, onClose}: MasterToPlayerCommandProps) {
    const [masterCommand, setMasterCommand] = React.useState<MasterCommands | null>(null);
    const [userId, setUserId] = useState<string|null>(null);

    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);
    const {data: currentUser} = useGetCurrentUserQuery();

    const isGameMaster = (currentUser && currentUser.data && playerInfos[currentUser.data.id]?.isMaster) || false;

    useEffect(()=>{
        if(anchorEl){
            setUserId(anchorEl.id);
        }
    },[anchorEl]);

    if (!isGameMaster) {
        return (
            <React.Fragment/>
        );
    }

    const userInfo = userId ? playerInfos[userId]! : null;
    const isUserOnline = userInfo?.isOnline;

    return (
        <>
            <Menu
                anchorEl={anchorEl}
                open={!!anchorEl}
                onClose={closeModal}
            >
                {!isUserOnline && <>
                    <MenuItem
                        disabled={!isUserOnline}
                        id={"Offline"}>
                        Игрок вне сети
                    </MenuItem>
                    <Divider/>
                </>}
                <MenuItem
                    disabled={!isUserOnline}
                    id={MasterCommands.Kick}
                    onClick={onStartCommandClick}>
                    Выгнать
                </MenuItem>
                <MenuItem
                    disabled={!isUserOnline}
                    id={MasterCommands.RequestSaveThrow}
                    onClick={onStartCommandClick}>
                    Запросить спас-бросок
                </MenuItem>
                <MenuItem
                    disabled={!isUserOnline}
                    id={MasterCommands.ChangeCharacteristics}
                    onClick={onStartCommandClick}>
                    Изменить характеристики
                </MenuItem>
            </Menu>
            {masterCommand && renderMasterCommandModal(masterCommand)}
        </>
    );


    function renderMasterCommandModal(masterCommand: MasterCommands) {
        return (
            <Modal
                open={true}
                onClose={closeModal}
            >
                <Box sx={ModalStyle}>
                    {renderCommandInModal(masterCommand)}
                </Box>
            </Modal>
        );
    }

    function renderCommandInModal(masterCommand: MasterCommands) {
        const props: ModalProps = {onCloseModal: closeModal, userId: userId!};
        switch (masterCommand) {
            case MasterCommands.RequestSaveThrow: {
                return (<RequestSaveThrowModal {...props}/>);
            }
            case MasterCommands.Kick: {
                return (<KickModal {...props}/>);
            }
            case MasterCommands.ChangeCharacteristics: {
                return (<ChangeCharacteristicsModal {...props}/>);
            }
            case null: {
                return (<React.Fragment/>);
            }
        }
    }

    function onStartCommandClick(e: React.MouseEvent<HTMLLIElement>) {
        setMasterCommand(e.currentTarget.id as MasterCommands);
        onClose();
    }

    function closeModal() {
        setMasterCommand(null);
        onClose();
    }
}
