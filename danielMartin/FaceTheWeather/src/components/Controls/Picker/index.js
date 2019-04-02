// @flow

import * as React from 'react';
import {
    Picker as ReactPicker,
    View,
    Text,
    TouchableOpacity,
    TouchableWithoutFeedback,
    Platform
} from 'react-native';
import Modal from '../Modal';
import styles from '../../../assets/Styles';

type Props = {
    options: Array<{
        label: string,
        value: any
    }>,
    onSelect: (?any) => void,
    prompt: string,
    value: ?any,
    disabled: boolean,
    setValOnChange: boolean
}
type State = {
    pickerVisible: boolean,
    value: ?string | ?number
}

export default class Picker extends React.Component<Props, State>{

    static defaultProps = {
        disabled: false,
        setValOnChange: true
    }
    state = {
        pickerVisible: false,
        value: this.props.value
    }
    selectItem = (value: string | number) => {
        this.setState({ pickerVisible: false });
        this.props.onSelect(value);
    }
    cancel = () => {
        this.setState({ value: null, pickerVisible: false });
    }
    changeSelection = (value: string | number) => {
        if (this.props.setValOnChange) {
            this.props.onSelect(value);
        }
        this.setState({ value });
    }
    getPrompt = () => {
        const currentVal = this.props.options.find(t => t.value === this.props.value);
        const valLabel = currentVal ? currentVal.label : '';
        return <Text>{`${this.props.prompt}: ${valLabel}`}</Text>
    }
    togglePicker = () => {
        if (!this.state.pickerVisible) {
            this.setState({ pickerVisible: true, value: this.props.value });
            return;
        }
        if (this.state.value !== this.props.value) {
            this.props.onSelect(this.state.value);
        }
        this.setState({ pickerVisible: false, value: null });
    }
    getOptions = (options: Array<{
        label: string,
        value: string | number
    }>): Array<React.Node> => {
        const optionList: Array<React.Node> = [];
        this.props.options.map((item) => {
            optionList.push(<ReactPicker.Item
                key={item.value}
                label={item.label}
                value={item.value}
                style={{ color: '#007AFF' }}
            />);
        });
        return optionList;
    }
    getIosPicker = (): React.Node => {
        return (
            <View>
                <View style={{ flex: 1 }}>
                    <TouchableOpacity onPress={this.togglePicker} disabled={this.props.disabled}>
                        <View style={{ flexDirection: "row", marginBottom: 10 }}>
                            {this.getPrompt()}
                        </View>
                    </TouchableOpacity>
                </View>

                <Modal isVisible={this.state.pickerVisible}>
                    <TouchableWithoutFeedback onPress={this.togglePicker} disabled={this.props.disabled} >
                        <View style={styles.pickerBottomModal}>
                            <View style={styles.pickerModalContent}>
                                <TouchableWithoutFeedback onPress={() => { }}>
                                    <View style={{ flex: 1 }}>
                                        <View style={{ paddingBottom: 10, paddingTop: 0, flexDirection:'row' }}>
                                            
                                                <TouchableOpacity onPress={this.cancel} style={styles.pickerCancelButtonContainer}>
                                                    <Text style={styles.pickerButtonCancel}>Cancel</Text>
                                                </TouchableOpacity>
                                            
                                            
                                                <TouchableOpacity onPress={this.togglePicker} style={styles.pickerDoneButtonContainer}>
                                                    <Text style={styles.pickerButtonCancel}>Done</Text>
                                                </TouchableOpacity>
                                            
                                        </View>
                                        <ReactPicker
                                            selectedValue={this.state.value || this.props.value}
                                            onValueChange={(value) => { this.setState({ value }); this.changeSelection(value); }}
                                        >
                                            {this.getOptions(this.props.options)}
                                        </ReactPicker>
                                    </View>
                                </TouchableWithoutFeedback>
                            </View>
                        </View>
                    </TouchableWithoutFeedback>
                </Modal>
            </View>
        )
    }
    render() {
        return this.getIosPicker();
    }
}
