// @flow

import axios from 'axios';
import { type DarkSkyForcast } from './flowtypes';
import appConfig from '../../../../app.config';

export default async function getForcast(latitude:string|number, longitude:string|number) : DarkSkyForcast {
    const darkSkyUrl = appConfig.darkSkyConfig.url;
    const darkSkySecret = appConfig.darkSkyConfig.secret;
    const forcastUrl = `${darkSkyUrl}/forecast/${darkSkySecret}/${latitude},${longitude}`;
    const axiosResponse = await axios.get(forcastUrl);
    return axiosResponse.data;

}





