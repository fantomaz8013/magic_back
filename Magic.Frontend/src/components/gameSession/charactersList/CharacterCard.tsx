import * as React from "react";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import CardMedia from "@mui/material/CardMedia";
import Typography from "@mui/material/Typography";
import InfoIcon from "@mui/icons-material/Info";
import Box from "@mui/material/Box";
import {
  CharacterCharacteristic,
  CharacterTemplateResponse,
} from "../../../models/response/characterTemplateResponse";
import Tooltip from "@mui/material/Tooltip";
import { useGetCharacteristicsQuery } from "../../../redux/api/characterApi";
import { className } from "./character.style";
import { IconBow, IconCross, IconSword, IconWand } from "@tabler/icons-react";
import { PlayerInfo } from "../GameSession";
import { baseProxy } from "../../../env";

interface DataCardType {
  user: PlayerInfo;
  isLocked: boolean;
  isGameMaster: boolean;
}
export interface CharacterTemplateProps {
  template: CharacterTemplateResponse;
  data: DataCardType;
}

const getIconClass = (classId: number): React.ReactNode => {
  switch (classId) {
    case 1:
      return <IconSword />;
    case 2:
      return <IconWand />;
    case 3:
      return <IconBow />;
    case 4:
      return <IconCross />;
    default:
      return <></>;
  }
};

export default function CharacterCard({
  template,
  data,
}: CharacterTemplateProps) {
  const { data: characteristics } = useGetCharacteristicsQuery();
  return (
    <Card
      style={{
        ...className.card,
        border: data.isLocked
          ? "1px solid rgba(255, 255, 255, 0.1)"
          : "1px solid rgba(255, 255, 255, 0.05)",
      }}
      className="card"
    >
      <div
        className={`${
          data.isLocked ? `cardDisable` : data.user?.login ? `cardActive` : ""
        }`}
      />
      {/* <CardMedia
            sx={{height: 150}}
            image={template.avatarUrL}
            title={template.name}
        /> */}
      {data.user?.login && (
        <div className="LockInfo">
          <span>Игрок </span>
          <span style={className.gamerName}>{data.user.login}</span>
        </div>
      )}
      {data.isGameMaster && !data.user?.login && (
        <div className="LockInfo">
          <span style={className.gamerExpectation}>Ожидание</span>
        </div>
      )}
      <CardMedia
        image={baseProxy + template.avatarUrL}
        sx={{ ...className.cardImage }}
        title={template.name}
      />
      <CardContent
        sx={{
          ...className.cardContent,
          opacity: data.isLocked ? "10%" : "100%",
        }}
      >
        <Typography sx={className.cardName}>{template.name}</Typography>
        <div style={className.cardClass}>
          <div style={className.cardClassName}>
            <div style={className.cardClassIcon}>
              {getIconClass(template.characterClass.id)}
            </div>
            <Typography style={className.cardClassTxt}>
              {template.characterClass.title}
            </Typography>
          </div>
          <div style={className.cardClassSkillContainer}>
            <CardMedia
              image={`/icon/attack.png`}
              sx={className.cardClassSkill}
            />
            <CardMedia
              image={`/icon/attack-splash.png`}
              sx={className.cardClassSkill}
            />
          </div>
        </div>
        <Box sx={className.characterList}>
          {characteristics &&
            characteristics.data &&
            characteristics.data.map(renderCharacteristics)}
        </Box>
        <Typography
          variant="body2"
          color="text.secondary"
          sx={className.cardDescription}
        >
          {template.description}
        </Typography>
      </CardContent>
    </Card>
  );

  function renderCharacteristics(c: CharacterCharacteristic) {
    return (
      <Box sx={className.characterItem} key={c.id}>
        <Tooltip title={c.description}>
          <Box sx={className.characterContainer}>
            <Typography sx={className.characterTxt}>
              {c.title.slice(0, 3)}
            </Typography>
            <Box sx={className.characterVal}>
              {template.characteristics[c.id]}
            </Box>
          </Box>
        </Tooltip>
      </Box>
    );
  }
}
