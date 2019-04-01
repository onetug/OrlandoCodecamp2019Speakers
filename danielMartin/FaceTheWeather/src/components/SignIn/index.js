// @flow


import * as React from 'react';
import {
    View,
    Text,
    TextInput,
    Button
} from 'react-native';

import { updateUser, getUserInfoName } from '../../services/data/repositories/userInfo';
import LoadScreen from '../../components/Controls/LoadScreen';
import styles, { flattenedStylesheet } from '../../assets/Styles';
import Weather from '../Weather';
import MyData from '../MyData';
import { type CurrentGeoLocation } from '../../services/domain/currentLocation/flowtypes';
import { type DarkSkyForcast, type Currently } from '../../services/api/darksky/flowtypes';
import { type AzureReverseAddress } from '../../services/api/mapping/flowtypes';

type Props = {

}
type State = {
    userName: ?string,
    loading: boolean,
    firstName: ?string,
    lastName: ?string
}

export default class SignIn extends React.Component<Props, State>{
    state = {
        loading: true,
        userName: null,
        lastName: null,
        firstName: null
    }
    componentDidMount = async () => {
        const userName = await getUserInfoName();
        this.setState({ userName, loading: false });
    }
    saveName = (firstName: ?string, lastName: ?string) => {
        if (!firstName || !lastName)
            return;
        this.setState({ loading: true });
        updateUser(firstName, lastName).then(async () => {
            const userName = await getUserInfoName();
            this.setState({ userName, loading: false });
        });
    }

    getUserAddView = (): React.Node => {
        const { firstName, lastName } = this.state;
        return (
            <View style={{ flexDirection: 'column' }}>
                <View style={{ flex: .3, backgroundColor:'#efe', borderRadius:20, padding:5 }}>
                    <View style={{ paddingTop: 5, paddingBottom: 10, flex: .2, justifyContent: 'center', flexDirection: 'row' }}>
                        <Text style={styles.welcomeText}>Please enter your name to use this application</Text>
                    </View>
                    <View style={{ flex: .6, justifyContent: 'center' }}>
                        <View style={{ flexDirection: 'row' }}>
                            <Text style={styles.labelText}>First Name:</Text>
                            <TextInput style={styles.textInput} onChange={value => this.setState({ firstName: value.nativeEvent.text })} />
                        </View>
                        <View style={{ flexDirection: 'row' }}>
                            <Text style={styles.labelText}>Last Name:</Text>
                            <TextInput style={styles.textInput} onChange={value => this.setState({ lastName: value.nativeEvent.text })} />
                        </View>
                    </View>
                    <View style={{ flex: .2 }}>
                        <Button disabled={!firstName || firstName.length === 0 || !lastName || lastName.length === 0}
                            onPress={() => this.saveName(this.state.firstName, this.state.lastName)}
                            title={"Save Name"}
                        />
                    </View>
                </View>
            </View>
        );
    }
    _weatherView: ?{ getCurrentStateWeatherData: () => { position: ?CurrentGeoLocation, forcast: ?DarkSkyForcast, location: ?AzureReverseAddress } };
    getCurrentWeather = (): ?{ position: ?CurrentGeoLocation, forcast: ?DarkSkyForcast, location: ?AzureReverseAddress } => {
        if (!this._weatherView)
            return null;
        const currentWeatherState = this._weatherView.getCurrentStateWeatherData();
        return currentWeatherState;
    }
    render() {
        const { userName, loading } = this.state;
        if (loading)
            return <LoadScreen />
        if (userName)
            return (
                <View style={styles.rootContainer}>
                    <View style={{ ...flattenedStylesheet(styles.container), flex: .5 }}>
                        <View style={styles.rootContainer}>
                            <Text style={styles.welcomeText}>Welcome to Face the Weather</Text>
                            <Text style={styles.welcomeText}>@ {userName}</Text>
                        </View>
                    </View>
                    <View style={styles.container} >
                        <Weather ref={ref => this._weatherView = ref} />
                    </View>
                    <View style={styles.container} >
                        <MyData getCurrentWeather={this.getCurrentWeather} />
                    </View>
                </View>
            );
        return (
            <View style={styles.rootContainer}>
                <View style={{...flattenedStylesheet(styles.container), backgroundColor:'#fff' }}>
                    {this.getUserAddView()}
                </View>
            </View>
        );

    }
}