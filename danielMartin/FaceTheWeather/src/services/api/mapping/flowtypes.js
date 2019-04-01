// @flow

export type Address =  {
    buildingNumber: string,
    streetNumber: string,
    routeNumbers: any[],
    street: string,
    streetName: string,
    streetNameAndNumber: string,
    countryCode: string,
    countrySubdivision: string,
    countrySecondarySubdivision: string,
    countryTertiarySubdivision: string,
    municipality: string,
    postalCode: string,
    municipalitySubdivision: string,
    country: string,
    countryCodeISO3: string,
    freeformAddress: string,
    boundingBox: BoundingBox,
    countrySubdivisionName: string,
  };
  
  export type Addresses =  {
    address: Address,
    position: string,
  };
  
  export type BoundingBox =  {
    northEast: string,
    southWest: string,
    entity: string,
  };
  
  export type AzureReverseAddress =  {
    summary: Summary,
    addresses: Addresses[],
  };
  
  
  export type EntryPoints =  {
    type: string,
    position: Position,
  };
  
  export type Position =  {
    lat: number,
    lon: number,
  };
  
  export type Results =  {
    type: string,
    id: string,
    score: number,
    address: Address,
    position: Position,
    viewport: Viewport,
    entryPoints?: EntryPoints[],
  };
  
  export type AzureAddressSearch =  {
    summary: Summary,
    results: Results[],
  };
  
  export type Summary =  {
    query: string,
    queryType: string,
    queryTime: number,
    numResults: number,
    offset: number,
    totalResults: number,
    fuzzyLevel: number,
  };
  
  export type Viewport =  {
    topLeftPoint: Position,
    btmRightPoint: Position,
  };
  
  
  
  