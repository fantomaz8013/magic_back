import * as signalR from "@microsoft/signalr";
import token from "./token";
import userRegister from "./userRegister";
import user from "./user";
import {apiProxy} from "../env";
import {HttpMethods} from "./httpMethods";

let serverErrorHandler = (error?: string): void => {
    return;
};
function setServerErrorHandler(handler: typeof serverErrorHandler): void {
    serverErrorHandler = handler;
}
function request<T>(url: string, options?: RequestInit): Promise<T> {
    return fetch(apiProxy + url, options)
        .catch((error) => {
            if (!window.navigator.onLine) {
                serverErrorHandler("Не можем подключиться к серверу");
            } else {
                serverErrorHandler("Не можем подключиться к серверу. Попробуйте обновить страницу.");
            }

            throw error;
        })
        .then(async response => {
            if (response.status >= 200 && response.status < 300) {
                return response;
            }
            if (response.status === 401) {
                return response;
            }

            if (response.status >= 500) {
                serverErrorHandler();
            }

            throw response;
        })
        .then(value => {
            const response = value as Response;
            if (response && response.status >= 200 && response.status < 300) {
                if (response.status !== 204) {
                    return response.json();
                }
            }
            return value;
        });
}

function get<T>(url: string, options?: RequestInit): Promise<T> {
    return request<T>(url, options);
}

function post<T>(url: string, options?: RequestInit): Promise<T> {
    options = options || {};
    options.method = HttpMethods.POST;
    return request<T>(url, options);
}

function patch<T>(url: string, options?: RequestInit): Promise<T> {
    options = options || {};
    options.method = HttpMethods.PATCH;
    return request<T>(url, options);
}

function put<T>(url: string, options?: RequestInit): Promise<T> {
    options = options || {};
    options.method = HttpMethods.PUT;
    return request(url, options);
}

function deleteRequest<T>(url: string, options?: RequestInit): Promise<T> { /* delete - зарезервированное слово, поэтому так */
    options = options || {};
    options.method = HttpMethods.DELETE;
    return request(url, options);
}

function createRequestParams(body: Record<string, unknown> | string): RequestInit {
    return {
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(body)
    };
}

function createSignalRConnection(url: string, loggingLevel = signalR.LogLevel.None): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
        .withUrl(
            apiProxy + url,
            {
                // accessTokenFactory: () => {
                //     return apiJwtToken;
                // },
                transport: signalR.HttpTransportType.WebSockets
            }
        )
        .configureLogging(loggingLevel)
        .build();
}

const httpFetch = {
    token,
    user,
    userRegister,
    setServerErrorHandler,

    request,
    createRequestParams,

    get,
    post,
    patch,
    put,
    delete: deleteRequest,

    createSignalRConnection,
};

export default httpFetch;

