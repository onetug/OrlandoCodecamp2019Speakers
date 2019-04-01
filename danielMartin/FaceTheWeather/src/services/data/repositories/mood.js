// @flow

import Realm from 'realm';
import { type Weather, getLatestId as latestWeatherId } from './weather';
import { type Location, getLatestId as latestLocationId } from './location';
import { DBSchema } from '../db';
import { MoodSchema } from '../schema';

export type emotions = 'anger' | 'contempt' | 'disgust' | 'fear' | 'happiness' | 'neutral' | 'sadness' | 'suprise';
export type Mood = {
    id: number,
    weather: Weather,
    emotion: emotions,
    isPartial: boolean
}

const db: DBSchema<Mood> = new DBSchema<Mood>(MoodSchema.name);
export async function getLatestId(): Promise<number> {
    return await db.getNextId();
}

export async function getWeathersByEmotion(emotion: emotions): Promise<Weather[]> {
    const recs: Mood[] = await db.getObjects(`emotion = "${emotion}"`);
    return recs.map(t => t.weather);
}

export async function getCountOfMoodRecords(): Promise<number>{
    const recs = await db.getObjects();
    return recs.length;
}

export async function saveWeatherByEmotion(emotion: emotions,
    location: {
        city: string,
        state: string
    },
    temperature: number,
    conditions: string,
    precipitationPercentage: string
): Promise<void> {
    const weatherId = await latestWeatherId();
    const locationId = await latestLocationId();
    const weather: Weather = {
        id: weatherId,
        temperature,
        conditions,
        precipitationPercentage,
        location: {
            id: locationId,
            city: location.city,
            state: location.state
        }
    };
    const id = await getLatestId();
    const rec: Mood = {
        id,
        weather,
        emotion,
        isPartial:false
    }
    await db.write(rec);
}

