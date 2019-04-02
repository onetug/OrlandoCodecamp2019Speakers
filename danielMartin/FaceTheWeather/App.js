// @flow

import * as React from 'react';
import {
    KeyboardAvoidingView
} from 'react-native';

import FtWCamera from './src/components/Camera';

import SignIn from './src/components/SignIn';

type Props = {};
type State = {

}
export default class App extends React.Component<Props, State> {
    state = {

    }
    componentDidMount = async () => {

    }
    render() {
        return (
            <KeyboardAvoidingView
                style={{ flex: 1, flexDirection: 'row' }}
                behavior="padding"
                enabled
            >
                <SignIn />
            </KeyboardAvoidingView>
        );
    }
}