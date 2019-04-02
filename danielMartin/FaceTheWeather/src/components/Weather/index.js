//@flow

import * as React from 'react';
import {
    Platform,
    StyleSheet,
    Text,
    View,
    Button
} from 'react-native';
import styles from '../../assets/Styles';
import LoadScreen from '../Controls/LoadScreen';
import { getCurrentCorrdinatesAsync } from '../../services/domain/currentLocation';
import { type CurrentGeoLocation } from '../../services/domain/currentLocation/flowtypes';
import getForcast from '../../services/api/darksky';
import { type DarkSkyForcast, type Currently } from '../../services/api/darksky/flowtypes';
import { getLocationByLatLongAsync, getLocationByAddress } from '../../services/api/mapping';
import { type AzureReverseAddress } from '../../services/api/mapping/flowtypes';
import { createGridRow } from '../Controls/gridDisplay';


type Props = {

}
type State = {
    position: ?CurrentGeoLocation,
    isLoading: boolean,
    forcast: ?DarkSkyForcast,
    location: ?AzureReverseAddress
}

export default class Weather extends React.Component<Props, State>{

    state = {
        position: null,
        isLoading: true,
        forcast: null,
        location: null
    }
    getCurrentStateWeatherData = () : {position:?CurrentGeoLocation, forcast:?DarkSkyForcast, location:?AzureReverseAddress} => {
        const { position, forcast, location } = this.state;
        return { position, forcast, location };
    }
    componentDidMount = async () => {
        const { forcast, location, position } = await this.loadLocalWeatherAsync();
        this.setState({ forcast, location, position, isLoading: false });
    }

    loadLocalWeatherAsync = async (): Promise<{
        position: ?CurrentGeoLocation,
        forcast: ?DarkSkyForcast,
        location: ?AzureReverseAddress
    }> => {
        const position = await getCurrentCorrdinatesAsync();
        if (!position)
            return {position, forcast:null, location:null};
        const forcast = await getForcast(position.coords.latitude, position.coords.longitude);
        const location = await getLocationByLatLongAsync(position.coords.latitude, position.coords.longitude);
        return {
            position,
            forcast,
            location
        };
    }
    reloadWeather = () => {
        this.setState({ isLoading: true });
        this.loadLocalWeatherAsync().then(t => {
            this.setState({
                isLoading: false,
                forcast: t.forcast,
                location: t.location,
                position: t.position
            })
        });
    }

    

    getForcastDisplay = (currentForcast: ?DarkSkyForcast): ?React.Node => {
        if (!currentForcast)
            return null;
        const currently = currentForcast.currently;
        return (
            <View style={{ flex: 1, flexDirection: 'column', padding:10 }}>
                {createGridRow("Current Condition", currently.summary)}
                {createGridRow("Temperature", currently.temperature)}
                {createGridRow("Feels Like", currently.apparentTemperature)}
                {createGridRow("Wind Speed", currently.windSpeed)}
                {createGridRow("Precipatation Percentage", currently.precipProbability)}
                {createGridRow("Precipatation Intensity", currently.precipIntensity)}
            </View>
        )
    }
    getLocationDisplay = (location: ?AzureReverseAddress): React.Node => {
        if (!location)
            return null;
        const addy = location.addresses[0].address;
        return (
            <View>
                <Text>Current Conditions at: </Text>
                <Text>{addy.streetNameAndNumber}</Text>
                <Text>{addy.municipality}, {addy.countrySubdivisionName}</Text>
            </View>
        )
    }
    getFullDisplay = (): React.Node => {
        const { isLoading, position, forcast, location } = this.state;
        if (isLoading)
            return <LoadScreen displayAsModal={false} />
        return (
            <>
                <View style={styles.container}>
                    {this.getLocationDisplay(location)}
                </View>
                <View style={styles.container}>
                    {this.getForcastDisplay(forcast)}
                </View>
                <View style={styles.container}>
                    <View>
                        <Button
                            title={"Reload Weather"}
                            onPress={this.reloadWeather}
                        />
                    </View>
                </View>
            </>
        );
    }

    render() {
        return (
            <View style={styles.rootContainer}>
                {this.getFullDisplay()}
            </View>
        )
    }

}