module.exports = {
    script: "serve",
    name:'MagicFront',
    env: {
        PM2_SERVE_PATH: 'build',
        PM2_SERVE_PORT: 3000,
        PM2_SERVE_SPA: 'true',
        NODE_ENV: "production",
    }
}
