// @flow

import { configureStore } from './configureStore';
import {type ReduxState} from '../flowTypes';

let _instance = null;
export function getStore() {
    if (this._instance == null){
        this._instance = configureStore(getInitialState());
    }
    return this._instance;
}

function getInitialState(): ReduxState {
    return {

    }
} 