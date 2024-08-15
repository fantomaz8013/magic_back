import * as React from "react";
import Grid from "@mui/material/Unstable_Grid2";
import CharacterCard from "./CharacterCard";
import Button from "@mui/material/Button";
import { useGetCharacterTemplatesQuery } from "../../../redux/api/characterApi";
import { useGetCurrentUserQuery } from "../../../redux/api/userApi";
import { CharacterTemplateResponse } from "../../../models/response/characterTemplateResponse";
import Box from "@mui/material/Box";
import { useSelector } from "react-redux";
import { RootState } from "../../../redux/redux";
import { socket } from "../../../webSocket/webSocket";
import { Container } from "@mui/material";

export default function CharacterList() {
  const { data: characterTemplates } = useGetCharacterTemplatesQuery();
  const { data: currentUser } = useGetCurrentUserQuery();
  const playerInfos = useSelector(
    (state: RootState) => state.gameSession?.playerInfos
  );

  const isDataLoaded =
    currentUser &&
    currentUser.data &&
    playerInfos &&
    characterTemplates?.data != null;
  const isGameMaster =
    isDataLoaded && playerInfos[currentUser.data!.id].isMaster;
  const isAnyCharacterLocked =
    isDataLoaded &&
    Object.values(playerInfos).filter((p) => p.lockedCharacterId !== null)
      .length > 0;
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        width: "100%",
        height: "calc(100svh - 81px)",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          width: "100%",
        }}
      >
        {isGameMaster && (
          <Container maxWidth="xl" sx={{ justifyContent: "flex-end" }}>
            <Box sx={{ padding: "20px" }}>
              <Button
                onClick={socket?.startGame}
                disabled={!isAnyCharacterLocked}
              >
                START GAME
              </Button>
            </Box>
          </Container>
        )}
      </Box>
      <Box
        id="lol"
        sx={{
          display: "flex",
          width: "100%",
          justifyContent: "center",
        }}
      >
        <Grid container spacing={3}>
          {isDataLoaded &&
            characterTemplates.data?.map(renderCharacterTemplate)}
        </Grid>
      </Box>
    </Box>
  );

  function renderCharacterTemplate(t: CharacterTemplateResponse) {
    const lockInfo = Object.values(playerInfos!).filter(
      (p) => p.lockedCharacterId === t.id
    )?.[0];
    const isLocked =
      isGameMaster || (lockInfo && lockInfo.id !== currentUser!.data!.id);
    const data = {
      user: lockInfo,
      isLocked: isLocked,
      isGameMaster: !!isGameMaster,
    }

    return (
      <Grid key={t.name} xs={3}>
        <Button
          color="success"
          disabled={isLocked}
          id={t.id}
          sx={{
            padding: 0,
            textAlign: "left",
            width: "100%",
            textTransform: "none",
            maxWidth: "350px",
          }}
          onClick={_lock}
        >
          <CharacterCard
            template={t}
            data={data}
          />
        </Button>
      </Grid>
    );
  }

  async function _lock(e: React.MouseEvent<HTMLButtonElement>) {
    const characterId = e.currentTarget.id;
    const lockInfo = Object.values(playerInfos!).filter(
      (p) => p.lockedCharacterId === characterId
    )?.[0];
    const isLocked =
      isGameMaster || (lockInfo && lockInfo.id !== currentUser!.data!.id);

    const lockFunc = lockInfo
      ? isLocked
        ? socket!.lockCharacter
        : socket!.unlockCharacter
      : socket!.lockCharacter;

    await lockFunc(characterId);
  }
}
