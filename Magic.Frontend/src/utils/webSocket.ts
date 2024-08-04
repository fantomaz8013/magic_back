import * as signalR from "@microsoft/signalr";
import {baseProxy} from "../env";

export function createSignalRConnection(
    url: string,
    token: string,
    loggingLevel = signalR.LogLevel.None): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
        .withUrl(
            baseProxy + url,
            {
                // accessTokenFactory: () => {
                //     return token;
                // },
                transport: signalR.HttpTransportType.WebSockets
            }
        )
        .configureLogging(loggingLevel)
        .build();
}
