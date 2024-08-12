import { useLocation, useNavigate } from "react-router-dom";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
} from "@mui/material";
import Button from "@mui/material/Button";
import paths from "../../consts/paths";
import * as React from "react";
import { useEffect } from "react";
import {
  useCreateMutation,
  useDeleteRequestMutation,
  useEnterMutation,
  useListQuery,
} from "../../redux/api/gameSessionApi";
import { GameSessionResponse } from "../../models/response/gameSessionResponse";
import { useGetCurrentUserQuery } from "../../redux/api/userApi";
import { useDispatch } from "react-redux";
import { AppDispatch } from "../../redux/redux";
import { clearGameSessionData } from "../../redux/slices/gameSessionSlice";
import className from "./session.style";
import { IconLockOpen, IconLock } from '@tabler/icons-react';
import { ContainerTable, HeaderTable, Row } from "./TableElement";

export default function SessionPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { data: list } = useListQuery();
  const [enterSession] = useEnterMutation();
  const [createSession] = useCreateMutation();
  const [deleteRequest] = useDeleteRequestMutation();
  const { data: currentUser } = useGetCurrentUserQuery();
  const dispatch = useDispatch<AppDispatch>();

  // useEffect(() => {
  //   if (location.pathname !== paths.home) {
  //     navigate(paths.home, { replace: true });
  //   }
  // }, [location, navigate]);

  useEffect(() => {
    dispatch(clearGameSessionData());
  }, []);

  return (
    <Grid item xs={9}>
      {/* <Card>
        <CardContent>
          <Typography variant="h5" component="div">
            Start your first game!
          </Typography>
          <Typography sx={{ mb: 1.5 }} color="text.secondary">
            short campaign designed for 4 players (30 mins.)
          </Typography>
          <Typography variant="body2">
            Start an adventure in DND stylistics.
            <br />
            Explore secrets and enjoy battle mechanics.
            <br />
            Save Halsin from prisoning in Crimson Castle.
          </Typography>
        </CardContent>
        <CardActions>
          <Button onClick={onCreateClick} size="small">
            Save Halsin!
          </Button>
        </CardActions>
      </Card>
      {list?.data?.map((r: GameSessionResponse) => {
        return (
          <Card key={r.id}>
            <CardContent>
              <Typography variant="h5" component="div">
                {r.title}
              </Typography>
              <Typography sx={{ mb: 1.5 }} color="text.secondary">
                For {r.maxUserCount} player(s)
              </Typography>
              <Typography variant="body2">{r.description}</Typography>
            </CardContent>
            <CardActions>
              {currentUser?.data?.id === r.creatorUserId ? (
                <>
                  <Button id={r.id} onClick={onJoinClick} size="small">
                    JOIN
                  </Button>
                  <Button id={r.id} onClick={onDeleteClick} size="small">
                    DELETE
                  </Button>
                </>
              ) : (
                <Button id={r.id} onClick={onAskToJoinClick} size="small">
                  ASK TO JOIN
                </Button>
              )}
            </CardActions>
          </Card>
        );
      })} */}
      <Grid item xs={12} style={className.block}>
        <ContainerTable style={className.tableWrapper}>
          <Table stickyHeader aria-label="table">
            <TableHead>{HeaderTable()}</TableHead>
            <TableBody>
              {list?.data?.map((r: GameSessionResponse) => {
                return (
                  <Row
                    key={r.id}
                    id={r.id}
                    onClick={(e) => onJoinClick(e)}
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
                        {true ? <IconLockOpen color="#2D572F" /> : <IconLock />}
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

  function onAskToJoinClick(e: React.MouseEvent<HTMLButtonElement>) {
    const gameSessionId = e.currentTarget.id;
    enterSession({ gameSessionId }).then((r) => {
      r.data?.data && navigate(`${paths.game}/${gameSessionId}`);
    });
  }

  function onJoinClick(e: any) {
    const gameSessionId = e.currentTarget.id;
    navigate(`${paths.game}/${gameSessionId}`);
  }

  function onDeleteClick(e: React.MouseEvent<HTMLButtonElement>) {
    const gameSessionId = e.currentTarget.id;
    deleteRequest({ gameSessionId });
  }

  function onCreateClick() {
    createSession({
      startDt: new Date().toJSON(),
      title: "Save Halsin!",
      description: "default game",
      maxUserCount: 4,
    }).then((r) => {
      if (r.data?.isSuccess) navigate(`${paths.game}/${r.data!.data!.id}`);
    });
  }
}
