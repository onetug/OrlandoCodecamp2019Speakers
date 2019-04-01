// @flow
import Realm from 'realm';
import { openSchema } from './schema';

export class DBSchema<T>{
    constructor(tableName:string) {
        this.dbName = tableName;
    }
    realm: Realm;
    dbName:string;

    initRealm = async () : Promise<Realm> => {
        if (!this.realm || !this.realm.isClosed)
            this.realm = await openSchema();
    }
    disposeRealm = async () : Promise<void> => {
        // await this.realm.close();
        // this.realm = null;
    }

    getObjects = async <T>(query?:string) : Promise<T[]> => {
        await this.initRealm();
        let data = this.realm.objects(this.dbName);
        if (query)
            data = data.filtered(query);
        return data;
    }
    getNextId = async () : Promise<number> => {
        await this.initRealm();
        const rec = this.realm.objects(this.dbName).sorted('id',true)[0];
        if (!rec)
            return 0;
        return rec.id + 1;
    }
    write = async <T>(dbObject:T) :Promise<void> => {
        await this.initRealm();
        this.realm.write(() => {
            this.realm.create(this.dbName,dbObject,true);
        });
    }

}



