export const devMode = (process.env.NODE_ENV || 'development') === 'development';

export const baseProxy = devMode ? "http://localhost:5000/" : "https://to.do";
export const apiProxy = baseProxy + ("api/v1/");
