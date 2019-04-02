// @flow

import * as React from 'react';
import {

} from 'react-native';
import RNModal from 'react-native-modal';

type Props = {
    isVisible:boolean,
    children:React.Node
}
type State = {

}

export default class Modal extends React.Component<Props,State>{
    static defaultProps = {
        isVisible:true
    }
    render(){
        return(
            <RNModal
                isVisible={this.props.isVisible}
                transparent
            >
                {this.props.children}
            </RNModal>
        );
    }
}