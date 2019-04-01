// @flow

import * as React from 'react';
import {
    ActivityIndicator
} from 'react-native';
import Modal from '../Modal';

type Props = {
    isVisible:boolean,
    displayAsModal:boolean
}
type State = {

}

export default class LoadScreen extends React.Component<Props,State> {
    static defaultProps = {
        isVisible:true,
        displayAsModal:true
    }
    render(){
        if (this.props.displayAsModal)
            return(
                <Modal
                    isVisible={this.props.isVisible}
                >
                    <ActivityIndicator color={"#ffffff"} size={'large'} />
                </Modal>
            );
        return (
            <ActivityIndicator color={"#ffffff"} size={'large'} />
        )
    }
}