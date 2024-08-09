import React from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import {Divider, Modal} from "@mui/material";
import Box from "@mui/material/Box";
import {
    MasterCommandDetails,
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
    const [masterCommand, setMasterCommand] = React.useState<MasterCommandDetails | null>(null);
    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);
    const {data: currentUser} = useGetCurrentUserQuery();

    const isGameMaster = (currentUser && currentUser.data && playerInfos[currentUser.data.id]?.isMaster) || false;

    if (!isGameMaster) {
        return (
            <React.Fragment/>
        );
    }

    const userIdFromAnchor = anchorEl?.id;
    const userInfo = userIdFromAnchor && playerInfos[userIdFromAnchor]!;
    const isUserOnline = !userInfo || (userInfo.isOnline === undefined) || userInfo?.isOnline;

    return (
        <>
            <Menu
                anchorEl={anchorEl}
                open={!!anchorEl}
                onClose={closeModal}
            >
                {!isUserOnline && <>
                    <MenuItem
                        disabled={!userInfo.isOnline}
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


    function renderMasterCommandModal(masterCommand: MasterCommandDetails) {
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

    function renderCommandInModal(masterCommand: MasterCommandDetails) {
        const props: ModalProps = {onCloseModal: closeModal, userId: masterCommand.userId};
        switch (masterCommand.type) {
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
        setMasterCommand({type: e.currentTarget.id as MasterCommands, userId: userIdFromAnchor!});
        onClose();
    }

    function closeModal() {
        setMasterCommand(null);
        onClose();
    }
}
