module.exports = {
    script: "serve",
    env: {
        PM2_SERVE_PATH: 'build',
        PM2_SERVE_PORT: 3000,
        NODE_ENV: "development",
        MAGIC_API_PROXY: "api/v1/",
        MAGIC_PROXY: "https://localhost:7021/"
    }
}
