// @flow

import { createStore } from 'redux';
import rootReducer from '../reducers';
import {type ReduxState } from '../flowTypes';

export function configureStore(initialState: ReduxState) {
    return createStore(
        rootReducer,
        initialState
    );
}