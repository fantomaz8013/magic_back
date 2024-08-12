import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './components/app/App';
import {BrowserRouter} from "react-router-dom";
import {store} from './redux/redux'
import {Provider} from 'react-redux'
import './index.css'
import './utils/fontImports'

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <Provider store={store}>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
    </Provider>
);
