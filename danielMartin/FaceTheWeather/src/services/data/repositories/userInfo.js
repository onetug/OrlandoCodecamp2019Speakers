// @flow

import Realm from 'realm';
import { DBSchema } from '../db';
import { UserInfoSchema } from '../schema';

export type UserInfo = {
    id:number,
    FirstName:string,
    LastName:string
}

const db:DBSchema<UserInfo> = new DBSchema<UserInfo>(UserInfoSchema.name);

export async function updateUser(firstName:string,lastName:string) : Promise<void> {
    const user:UserInfo = {id:1, FirstName:firstName,LastName:lastName};
    await db.write(user); 
}

export async function getUserInfoName() : Promise<string> {    
    const data = await db.getObjects();
    const userInfos:UserInfo[] = data;
    if (userInfos.length > 1)
        throw 'There are multiple entries in the user database, this should not happen';
    if (userInfos.length == 0)
        return '';
    const user = userInfos[0];
    const name = `${user.FirstName} ${user.LastName}`;
    await db.disposeRealm();
    return name;
}