Based on create-react-app + typescript + MUI + redux RTK + react router dom

Сначала устанавливаем все пакеты

**Requirements**
```
yarn install
```

Для локального запуска используем react-scripts

**DevRun**
```
yarn run start
```

Чтобы задеплоить делаем бандл, он будет находиться в /build/*

**Before deploying**
```
yarn run build
```

Эту папку деплоим с конфигом pm2, не забудь подменить url-ы на реальные

**Deploying**
```
pm2 start ecosystem.config.js
```
