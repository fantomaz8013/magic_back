import * as React from 'react';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
import {GameSessionCharacter} from "../../../models/websocket/gameStartedInfo";
import Typography from "@mui/material/Typography";
import {baseProxy} from "../../../env";


export interface CharacterTemplateProps {
    character: GameSessionCharacter;
    onClick: (e: React.MouseEvent<HTMLDivElement>) => void;
}

export default function CharacterLeftMenu({character, onClick}: CharacterTemplateProps) {
    return (
        <Card onClick={onClick} id={character.ownerId}>
            <Typography>
                {character.currentHP}/{character.maxHP}
            </Typography>
            <CardMedia
                sx={{height: 150}}
                image={baseProxy + character.avatarUrL}
                title={character.name}
            />
        </Card>
    );
}
