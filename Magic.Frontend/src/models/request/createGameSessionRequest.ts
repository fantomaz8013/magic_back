export interface CreateGameSessionRequest {
    title: string;
    description: string;
    maxUserCount: number;
    startDt: string;
}
