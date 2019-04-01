// @flow

import Realm from 'realm';

export const LocationSchema = {
    name: 'Location',
    primaryKey: 'id',
    properties:{
        id: 'int',
        city: 'string',
        state: 'string'
    }
}

export const MoodSchema = {
    name: 'Mood',
    primaryKey: 'id',
    properties:{
        id: 'int',
        weather: 'Weather',
        emotion: 'string',
        isPartial: 'bool'
    }
}

export const UserInfoSchema = {
    name:'UserInfo',
    primaryKey: 'id',
    properties:{
        id: 'int',
        FirstName:'string',
        LastName:'string'
    }
}


export const WeatherSchema = {
    name: 'Weather',
    primaryKey: 'id',
    properties:{
        id: 'int',
        conditions: 'string',
        precipitationPercentage: 'string',
        temperature:'int',
        location: 'Location'
    }
}
export async function openSchema(): Realm{
    return await Realm.open({schema:[LocationSchema,MoodSchema,UserInfoSchema,WeatherSchema]});
}

