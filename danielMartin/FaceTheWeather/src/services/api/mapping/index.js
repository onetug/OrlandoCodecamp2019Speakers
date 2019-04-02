// @flow
import axios,{ AxiosInstance} from 'axios';
import appConfig from '../../../../app.config';
import {type AzureReverseAddress, type AzureAddressSearch } from './flowtypes';

function createUrl(path:string,query:string,additionalParams?:{key:string,value:string}[]): string {
    const azureMapUri = appConfig.azureMapService.uri;
    const azureMapKey = appConfig.azureMapService.key;
    let addParams = '';
    if (additionalParams && additionalParams.length > 0){
        additionalParams.forEach(item => {
            addParams = `${addParams}&${item.key}=${item.value}`;
        });
    }
    return `${azureMapUri}/${path}?api-version=1.0&subscription-key=${azureMapKey}&query=${query}${addParams}`;
}

export async function getLocationByLatLongAsync(latitude:number,longitude:number) : Promise<AzureReverseAddress>{
    const url = createUrl("/search/address/reverse/json",`${latitude},${longitude}`);
    const call = await axios.get(url);
    return call.data;
}

export async function getLocationByAddress(search:string) : Promise<AzureAddressSearch> {
    const url = createUrl("search/address/json",search,[{key:"typeahead",value:"true"}]);
    const call = await axios.get(url);
    return call.data;
}