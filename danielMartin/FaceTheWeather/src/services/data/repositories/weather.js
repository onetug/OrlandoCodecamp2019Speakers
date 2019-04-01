// @flow

import Realm from 'realm';
import { DBSchema } from '../db';
import {type Location } from './location';
import { WeatherSchema } from '../schema';
export type Weather = {
    id: number,
    conditions: string,
    precipitationPercentage: string,
    temperature:number,
    location: Location
}


const db:DBSchema<Weather> = new DBSchema<Weather>(WeatherSchema.name);
export async function getLatestId() : Promise<number> {
    return await db.getNextId();
    
}