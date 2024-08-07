import * as React from 'react';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import InfoIcon from '@mui/icons-material/Info';
import {
    CharacterCharacteristic, CharacterCharacteristics,
    CharacteristicsMapper,
    CharacterTemplate
} from "../../../models/response/characterTemplateResponse";
import Tooltip from "@mui/material/Tooltip";
import {useGetCharacteristicsQuery} from "../../../redux/toolkit/api/characterApi";


export interface CharacterTemplateProps {
    template: CharacterTemplate;
}

export default function CharacterCard({template}: CharacterTemplateProps) {
    const {data: characteristics} = useGetCharacteristicsQuery();

    return (
        <Card sx={{maxWidth: 350, height:'70%'}}>
            <CardMedia
                sx={{height: 150}}
                image={template.avatarUrL}
                title={template.name}
            />
            <CardContent>
                <Typography gutterBottom variant="h5" component="div">
                    {template.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {template.description}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    RACE: {template.characterRace.title}
                    <br/>
                    CLASS: {template.characterClass.title}
                    <br/>
                    {characteristics && characteristics.data && characteristics.data.map(renderCharacteristics)}
                </Typography>
            </CardContent>
        </Card>
    );

    function renderCharacteristics(c: CharacterCharacteristic) {
        return (
            <span key={c.id}>
                {c.title}:{template.characteristics[c.id]}
                <Tooltip title={c.description}>
                    <InfoIcon fontSize={'small'}/>
                </Tooltip>
            </span>
        );
    }
}
