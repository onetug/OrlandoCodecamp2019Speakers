// @flow

export type Coords =  {
    speed: number,
    longitude: number,
    latitude: number,
    accuracy: number,
    heading: number,
    altitude: number,
    altitudeAccuracy: number,
  };
  
  export type CurrentGeoLocation =  {
    coords: Coords,
    timestamp: number,
  };
  
  