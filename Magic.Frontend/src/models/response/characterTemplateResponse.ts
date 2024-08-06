export interface CharacterTemplateResponse {
    name: string;
    description: string;
    avatarUrL: string;
    characterClass: string;
    abilities: string;
    armor: string;
    characterRaceId: string;
    characterRace: string;
    maxHP: string;
    speed: string;
    initiative: string;
    characteristics: string;
}

export interface CharacterClass{
    title:string;
    characterCharacteristicId: number;
}
