import * as React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import InfoIcon from '@mui/icons-material/Info';
import Box from "@mui/material/Box";
import {
    CharacterCharacteristic,
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
        <Card sx={{maxWidth: 350, height: '70%'}}>
            <CardMedia
                sx={{height: 150}}
                image={template.avatarUrL}
                title={template.name}
            />
            <CardContent sx={{overflow: 'hidden', display: 'flex-column', height: '75%', overflowY: 'auto'}}>
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
                    <Box sx={{display: 'flex', flexDirection: 'column'}}>
                        {characteristics && characteristics.data && characteristics.data.map(renderCharacteristics)}
                    </Box>
                </Typography>
            </CardContent>
        </Card>
    );

    function renderCharacteristics(c: CharacterCharacteristic) {
        return (
            <Box sx={{
                display: 'flex',
                justifyContent: 'space-between'
            }} key={c.id}>
                <Box>
                    <Tooltip title={c.description}>
                        <InfoIcon fontSize={'small'}/>
                    </Tooltip>
                    {c.title}:
                </Box>
                <Box>{template.characteristics[c.id]}</Box>
            </Box>
        );
    }
}
