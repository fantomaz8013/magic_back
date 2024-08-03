Based on create-react-app + typescript + MUI

Requirements
```
yarn global add serve
yarn global add pm2
yarn install
```

Before deploying
```
yarn run build
```

Deploying
```
pm2 serve build 3000 --spa 
```
