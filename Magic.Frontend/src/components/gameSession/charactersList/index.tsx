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


export interface CharacterTemplateProps {
    template: CharacterTemplate;
    characteristics: CharacterCharacteristic[];
}

export default function CharacterCards({template, characteristics}: CharacterTemplateProps) {
    return (
        <Card sx={{maxWidth: 345}}>
            <CardMedia
                sx={{height: 140}}
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
                    RACE:{template.characterRace.title}
                    <br/>
                    CLASS:{template.characterClass.title}
                    <br/>
                    {characteristics.map(c => {
                        return (<span key={c.id}>
                            {c.title}:{template.characteristics[c.id]}
                            <Tooltip title={c.description}>
                                <InfoIcon fontSize={'small'}/>
                            </Tooltip>
                        </span>);
                    })}
                </Typography>
            </CardContent>
        </Card>
    );
}
