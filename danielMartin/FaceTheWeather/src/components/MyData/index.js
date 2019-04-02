// @flow


import * as React from 'react';
import {
    View,
    Text,
    TextInput,
    Button
} from 'react-native';

import LoadScreen from '../../components/Controls/LoadScreen';
import styles from '../../assets/Styles';
import { saveByEmotion, getByEmotion, getCountOfWeatherRecords } from '../../services/domain/mood';
import { type emotions } from '../../services/data/repositories/mood';
import { createGridRow } from '../Controls/gridDisplay';
import { type CurrentGeoLocation } from '../../services/domain/currentLocation/flowtypes';
import { type DarkSkyForcast, type Currently } from '../../services/api/darksky/flowtypes';
import { type AzureReverseAddress } from '../../services/api/mapping/flowtypes';
import Picker from '../Controls/Picker';
import Camera from '../Camera';

type Props = {
    getCurrentWeather: () =>?{ position: ?CurrentGeoLocation, forcast: ?DarkSkyForcast, location: ?AzureReverseAddress }
}
type State = {
    isLoading: boolean,
    weatherChoice: ?{
        temperature: string,
        precipitationPercentage: string,
        conditions: string,
        emotionTotal:number
    },
    selectedEmotion: ?emotions,
    totalRecords: number,
    showCamera:boolean
}

export default class MyData extends React.Component<Props, State> {

    state = {
        isLoading: true,
        weatherChoice: null,
        selectedEmotion: null,
        totalRecords: 0,
        showCamera:false
    }
    componentDidMount = async () => {
        const weatherChoice = await this.loadMyData('happiness');
        this.setState({ isLoading: false, weatherChoice, totalRecords: weatherChoice.totalRecords });
    }
    loadMyData = async (emotion: emotions): Promise<{ temperature: string, precipitationPercentage: string, conditions: string, totalRecords: number, emotionTotal:number }> => {
        const totalRecords = await getCountOfWeatherRecords();
        const emotionData = await getByEmotion(emotion);
        return { ...emotionData, totalRecords }
    }
    reloadData = (selectedEmotion: ?any) => {
        this.setState({ isLoading: true });
        const emotion: emotions = selectedEmotion || this.state.selectedEmotion || 'happiness';
        this.loadMyData(emotion).then(weatherChoice => {
            this.setState({ isLoading: false, weatherChoice, totalRecords: weatherChoice.totalRecords });
        });

    }
    saveCurrentWeatherAs = () => {
        const currentWeather = this.props.getCurrentWeather();
        if (!currentWeather)
            return;
        this.setState({ isLoading: true });
        if (!currentWeather.location) {
            this.setState({ isLoading: false });
            return;
        }
        const currentAddy = currentWeather.location.addresses[0].address;
        const location = { state: currentAddy.countrySubdivision, city: currentAddy.municipalitySubdivision };
        if (!currentWeather.forcast) {
            this.setState({ isLoading: false });
            return;
        }
        const currentConditions = currentWeather.forcast.currently;
        if (!this.state.selectedEmotion) {
            this.setState({ isLoading: false });
            return;
        }
        saveByEmotion(this.state.selectedEmotion,
            location,
            currentConditions.temperature,
            currentConditions.summary,
            currentConditions.precipProbability.toString()).then(async () => {
                await this.reloadData();
                this.setState({selectedEmotion:null})
            });
    }

    getEmotionPicker = (): React.Node => {
        return (
            <Picker
                options={
                    ['anger', 'contempt', 'disgust', 'fear', 'happiness', 'neutral', 'sadness', 'suprise'].map(t => {
                        const label = `${t.substring(0, 1).toUpperCase()}${t.substring(1)}`;
                        return {
                            value: t,
                            label
                        }
                    })
                }
                prompt={"Select Emotion"}
                onSelect={selectedEmotion => { this.setState({ selectedEmotion }, this.reloadData(selectedEmotion)); }}
                value={this.state.selectedEmotion}
                setValOnChange={false}
            />
        );
    }
    getDisplayData = (): React.Node => {
        if (this.state.isLoading)
            return <LoadScreen displayAsModal={false} />
        if (this.state.showCamera)
            return <Camera onSelectEmotion={selectedEmotion => this.setState({selectedEmotion, showCamera:false})} />
        return (
            <View style={styles.container}>
                <View style={styles.rootContainer}>
                    <View style={styles.container}>
                        <View style={{ flexDirection: 'column', padding: 10, flex:1 }}>
                            <View style={{ flexDirection: 'row', flex: .2, alignSelf:"flex-start", alignContent:'flex-start'}}>
                                    <Text style={{flexWrap:'wrap'}}>A total of {this.state.totalRecords} have been created.</Text>
                            </View>
                            <View style={{ flexDirection: 'row', flex: .2, alignSelf:'flex-start', alignContent:'flex-start'}}>
                                <Text>The following data is for {this.state.selectedEmotion ? this.state.selectedEmotion : 'happiness'}.</Text>
                            </View>
                            <View style={{ flexDirection: 'row', flex: 1, paddingTop:10 }}>
                                <View style={{ flex: 1, flexDirection: 'column', paddingRight: 10, paddingLeft: 10 }}>
                                    {createGridRow("Total Records", this.state.weatherChoice ? this.state.weatherChoice.emotionTotal.toString() : null)}
                                    {createGridRow("Temperature", this.state.weatherChoice ? this.state.weatherChoice.temperature : null)}
                                    {createGridRow("Conditions", this.state.weatherChoice ? this.state.weatherChoice.conditions : null)}
                                    {createGridRow("Precipitation %", this.state.weatherChoice ? this.state.weatherChoice.precipitationPercentage : null)}
                                </View>
                            </View>
                            <View style={{ flexDirection: 'row', flex: 1 }}>
                                <View style={{ flexDirection: 'row', flex: .5 }}>
                                    {this.getEmotionPicker()}
                                </View>
                                <View style={{ flexDirection: 'row', flex: .5 }}>
                                    <Button
                                        title={'Record Weather'}
                                        onPress={this.saveCurrentWeatherAs}
                                        disabled={!this.state.selectedEmotion}
                                    />
                                </View>
                            </View>
                            <View>
                                <Button 
                                    title={'Show Camera'}
                                    onPress={() => this.setState({showCamera:true})}
                                />
                            </View>
                        </View>
                    </View>
                </View>
            </View>
        );
    }

    render() {
        return (
            <View style={styles.rootContainer}>
                {this.getDisplayData()}
            </View>
        )
    }
}