export const baseProxy = process.env.MAGIC_PROXY || "https://localhost:7021/";
export const apiProxy = baseProxy + (process.env.MAGIC_API_PROXY || "api/v1/");
export const devMode = (process.env.NODE_ENV || 'development') === 'development';
