// @flow

import { RNCamera } from 'react-native-camera';
import * as React from 'react';
import {
    Dimensions,
    Text,
    StyleSheet,
    View,
    TouchableOpacity,
    ScrollView
} from 'react-native';

import { getEmotionFromImage, extractEmotionFromFacialCall } from '../../services/api/facial/emotion';
import { type emotions } from '../../services/data/repositories/mood';
import LoadScreen from '../Controls/LoadScreen';


type Props = {
    onSelectEmotion: (emotion: ?emotions) => void

}
type State = {
    azureData: ?string,
    emotion: ?emotions,
    isLoading: boolean,
    snapDisabled:boolean
}

export default class FtWCamera extends React.Component<Props, State> {
    state = {
        azureData: null,
        emotion: null,
        isLoading: false,
        snapDisabled:false
    }
    camera: ?RNCamera;
    takePicture = async () => {

        const options = { quality: 1, base64: true };
        if (this.camera == null)
            return;
        this.setState({snapDisabled:true});
        try {
            if (this.camera == null)
                return;
            const data = await this.camera.takePictureAsync(options);
            this.setState({ isLoading: true });
            const azureData = await getEmotionFromImage(data.base64);
            const emotion = extractEmotionFromFacialCall(azureData);
            // $FlowFixMe 'This is returning as a string but it requires an enum
            this.setState({ azureData, emotion, isLoading: false , snapDisabled:false});
        }
        catch (exception) {
            this.setState({ isLoading: false, snapDisabled:false });
        }
    }

    getResetButtons = (): React.Node => {
        return (
            <View>
                <View style={{ flex: .3, flexDirection: 'row', }}>
                    <TouchableOpacity
                        onPress={() => this.setState({ azureData: null, emotion: null })}
                        style={styles.reset}
                    >
                        <Text style={{ fontSize: 14 }}> Reset </Text>
                    </TouchableOpacity>
                    <TouchableOpacity
                        onPress={() => this.props.onSelectEmotion(this.state.emotion)}
                        style={styles.reset}
                    >
                        <Text style={{ fontSize: 14 }}> Select Emotion </Text>
                    </TouchableOpacity>
                </View>
            </View>
        );
    }
    render = () => {
        if (this.state.isLoading)
            return <LoadScreen displayAsModal={false} />
        if (this.state.azureData) {
            return (
                <ScrollView>
                    {this.getResetButtons()}
                    <View>
                        <Text>Your current higest emotion is: {this.state.emotion}.</Text>
                    </View>
                    <View>
                        <Text>
                            {JSON.stringify(this.state.azureData, undefined, 2)}
                        </Text>
                    </View>
                    {this.state.azureData && this.state.azureData.length > 10 && this.getResetButtons()}
                </ScrollView>
            );
        }

        return (
            <View style={styles.container}>
                <RNCamera
                    ref={ref => {
                        this.camera = ref;
                    }}
                    style={styles.preview}
                    type={RNCamera.Constants.Type.front}
                    flashMode={RNCamera.Constants.FlashMode.off}
                    permissionDialogTitle={'Permission to use camera'}
                    permissionDialogMessage={'We need your permission to use your camera phone'}
                    onGoogleVisionBarcodesDetected={({ barcodes }) => {
                        console.log(barcodes)
                    }}
                />
                <View style={{ flex: 0, flexDirection: 'row', justifyContent: 'center', }}>
                    <TouchableOpacity
                        onPress={this.takePicture}
                        style={this.state.snapDisabled ? styles.captureDisabled : styles.capture}
                        disabled={this.state.snapDisabled}
                    >
                        <Text style={this.state.snapDisabled ? styles.snapTextDisabled : styles.snapText}> SNAP </Text>
                    </TouchableOpacity>
                </View>
            </View>
        );
    }
}
const styles = StyleSheet.create({
    container: {
        flex: 1,
        flexDirection: 'row',
        backgroundColor: 'black'
    },
    preview: {
        flex: 1,
    },
    capture: {
        flex: 0,
        backgroundColor: '#fff',
        borderRadius: 5,
        padding: 15,
        paddingHorizontal: 20,
        alignSelf: 'center',
        margin: 20
    },
    captureDisabled:{
        flex: 0,
        backgroundColor: '#ccc',
        borderRadius: 5,
        padding: 15,
        paddingHorizontal: 20,
        alignSelf: 'center',
        margin: 20
    },
    snapText:{
        fontSize:14
    },
    snapTextDisabled:{
        fontSize:14,
        color:'#fff'

    },
    reset: {
        flex: 0,
        backgroundColor: '#fff',
        borderRadius: 5,
        padding: 15,
        paddingHorizontal: 20,
        margin: 20,
        alignSelf: 'center'
    }
});


