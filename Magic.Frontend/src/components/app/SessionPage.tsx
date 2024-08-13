import {useLocation, useNavigate} from "react-router-dom";
import Grid from "@mui/material/Grid";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
} from "@mui/material";
import Button from "@mui/material/Button";
import paths from "../../consts/paths";
import * as React from "react";
import {useEffect} from "react";
import {
    useCreateMutation,
    useDeleteRequestMutation,
    useEnterMutation,
    useListQuery,
} from "../../redux/api/gameSessionApi";
import {GameSessionResponse} from "../../models/response/gameSessionResponse";
import {useDispatch} from "react-redux";
import {AppDispatch} from "../../redux/redux";
import {clearGameSessionData} from "../../redux/slices/gameSessionSlice";
import className from "./session.style";
import {IconLockOpen, IconLock} from '@tabler/icons-react';
import {ContainerTable, HeaderTable, Row} from "./TableElement";

export default function SessionPage() {
    const navigate = useNavigate();
    const {data: list} = useListQuery();
    const [enterSession] = useEnterMutation();
    const [createSession] = useCreateMutation();
    const [deleteRequest] = useDeleteRequestMutation();
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        dispatch(clearGameSessionData());
    }, []);

    return (
        <Grid item xs={9}>
            <Grid item xs={12} style={className.block}>
                <ContainerTable style={className.tableWrapper}>
                    <Table stickyHeader aria-label="table">
                        <TableHead>
                            <HeaderTable/>
                        </TableHead>
                        <TableBody>
                            {list?.data?.map((r: GameSessionResponse) => {
                                return (
                                    <Row
                                        key={r.id}
                                        id={r.id}
                                        onClick={onJoinClick}
                                    >
                                        <TableCell component="td" scope="row" style={className.longTxt}>
                                            {r.title}
                                        </TableCell>
                                        <TableCell component="td" scope="row" style={className.longTxt}>
                                            {r.description}
                                        </TableCell>
                                        <TableCell component="td" scope="row" align="center">
                                            <div style={className.tableIconWrapper}>
                                                {r.currentUserCount}/{r.maxUserCount}
                                            </div>
                                        </TableCell>
                                        <TableCell component="td" scope="row" align="center">
                                            <div style={className.tableIconWrapper}>
                                                В игре
                                            </div>
                                        </TableCell>
                                        <TableCell component="td" scope="row" align="center">
                                            <div style={className.tableIconWrapper}>
                                                {true ? <IconLockOpen color="#2D572F"/> : <IconLock/>}
                                            </div>
                                        </TableCell>
                                    </Row>
                                );
                            })}
                        </TableBody>
                    </Table>
                </ContainerTable>
                <div>
                    <Button variant="contained" color="success" onClick={onCreateClick}>
                        Создать лобби
                    </Button>
                </div>
            </Grid>
        </Grid>
    );

    async function onJoinClick(e: React.MouseEvent<HTMLDivElement>) {
        const gameSessionId = e.currentTarget.id;
        await enterSession({gameSessionId}).then((r) => {
            r.data?.data && navigate(`${paths.game}/${gameSessionId}`);
        });
    }

    function onDeleteClick(e: React.MouseEvent<HTMLButtonElement>) {
        const gameSessionId = e.currentTarget.id;
        deleteRequest({gameSessionId});
    }

    function onCreateClick() {
        createSession({
            startDt: new Date().toJSON(),
            title: "Save Halsin! " + new Date().toJSON(),
            description: "default game",
            maxUserCount: 4,
        }).then((r) => {
            if (r.data?.isSuccess) navigate(`${paths.game}/${r.data!.data!.id}`);
        });
    }
}
