import * as React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import {
    CharacterTemplate
} from "../../../models/response/characterTemplateResponse";


export interface CharacterTemplateProps {
    template: CharacterTemplate;
    onClick: (e: React.MouseEvent<HTMLDivElement>) => void;
}

export default function CharacterCard({template, onClick}: CharacterTemplateProps) {
    return (
        <Card onClick={onClick}>
            <CardMedia
                sx={{height: 150}}
                image={template.avatarUrL}
                title={template.name}
            />
        </Card>
    );
}
