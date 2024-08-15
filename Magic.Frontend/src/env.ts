export const devMode = (process.env.NODE_ENV || 'development') === 'development';

export const baseProxy = devMode ? "https://localhost:7021/" : "https://to.do";
export const apiProxy = baseProxy + ("api/v1/");
