// @flow
import axios, { AxiosInstance } from 'axios';
import appConfig from '../../../../app.config';
import RNFetchBlob from 'rn-fetch-blob'
import { type AzureFacialRecogition } from './flowtypes';

export async function getEmotionFromImage(image: any): Promise<AzureFacialRecogition> {
    try {
        const axiosCall = await makeFetchCall("/face/v1.0/detect", [{ key: "returnFaceId", value: "false" }, { key: "returnFaceLandmarks", value: "false" }, { key: "returnFaceAttributes", value: "age,gender,smile,facialHair,headPose,glasses,emotion,hair,makeup,accessories,blur,exposure,noise" }], image);
        return axiosCall;
    }
    catch (exception) {
        if (exception.response)
            return exception.response;
        return exception;
    }

}

async function makeFetchCall(path: string, additionalParams?: { key: string, value: string }[], data: any): Promise<Response> {
    const baseURL = appConfig.azureCognativeService.uri;
    const azureKey = appConfig.azureCognativeService.key;

    let addParams = '';
    if (additionalParams && additionalParams.length > 0) {
        for (let i = 0; additionalParams.length > i; i++) {
            if (i > 0)
                addParams = `${addParams}&`;
            const item = additionalParams[i];
            addParams = `${addParams}${item.key}=${item.value}`;
        }
    }
    const url = `${baseURL}${path}?${addParams}`;
    const fetchResult = await RNFetchBlob.fetch("POST",
        url,
        {
            'Content-Type': 'application/octet-stream',
            'Ocp-Apim-Subscription-Key': azureKey
        },
        data);
    return JSON.parse(fetchResult.data);
}


export function extractEmotionFromFacialCall(object: ?AzureFacialRecogition[]): ?string {
    console.log(object);
    if (!object || object.length === 0)
        return null;
    const firstObject = object[0];
    if (!firstObject.faceAttributes)
        return null;
    const allEmotions = [
        { key: 'anger', value: firstObject.faceAttributes.emotion.anger },
        { key: 'contempt', value: firstObject.faceAttributes.emotion.contempt },
        { key: 'disgust', value: firstObject.faceAttributes.emotion.disgust },
        { key: 'fear', value: firstObject.faceAttributes.emotion.fear },
        { key: 'happiness', value: firstObject.faceAttributes.emotion.happiness },
        { key: 'neutral', value: firstObject.faceAttributes.emotion.neutral },
        { key: 'sadness', value: firstObject.faceAttributes.emotion.sadness },
        { key: 'surprise', value: firstObject.faceAttributes.emotion.surprise }
    ];
    console.log(allEmotions);
    const sortedEmotions = allEmotions.sort((a, b) => a.value < b.value ? 1 : -1);
    return sortedEmotions[0].key;
}