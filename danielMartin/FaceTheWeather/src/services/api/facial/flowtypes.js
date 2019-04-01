export type Accessories =  {
    type: string,
    confidence?: number,
    " confidence": number,
  };
  
  export type Blur =  {
    blurLevel: string,
    value: number,
  };
  
  export type Emotion =  {
    anger: number,
    contempt: number,
    disgust: number,
    fear: number,
    happiness: number,
    neutral: number,
    sadness: number,
    surprise: number,
  };
  
  export type Exposure =  {
    exposureLevel: string,
    value: number,
  };
  
  export type FaceAttributes =  {
    age: number,
    gender: string,
    smile: number,
    facialHair: FacialHair,
    glasses: string,
    headPose: HeadPose,
    emotion: Emotion,
    hair: Hair,
    makeup: Makeup,
    occlusion: Occlusion,
    accessories: Accessories[],
    blur: Blur,
    exposure: Exposure,
    noise: Noise,
  };
  
  export type FaceLandmarks =  {
    pupilLeft: PupilLeft,
    pupilRight: PupilLeft,
    noseTip: PupilLeft,
    mouthLeft: PupilLeft,
    mouthRight: PupilLeft,
    eyebrowLeftOuter: PupilLeft,
    eyebrowLeftInner: PupilLeft,
    eyeLeftOuter: PupilLeft,
    eyeLeftTop: PupilLeft,
    eyeLeftBottom: PupilLeft,
    eyeLeftInner: PupilLeft,
    eyebrowRightInner: PupilLeft,
    eyebrowRightOuter: PupilLeft,
    eyeRightInner: PupilLeft,
    eyeRightTop: PupilLeft,
    eyeRightBottom: PupilLeft,
    eyeRightOuter: PupilLeft,
    noseRootLeft: PupilLeft,
    noseRootRight: PupilLeft,
    noseLeftAlarTop: PupilLeft,
    noseRightAlarTop: PupilLeft,
    noseLeftAlarOutTip: PupilLeft,
    noseRightAlarOutTip: PupilLeft,
    upperLipTop: PupilLeft,
    upperLipBottom: PupilLeft,
    underLipTop: PupilLeft,
    underLipBottom: PupilLeft,
  };
  
  export type FaceRectangle =  {
    width: number,
    height: number,
    left: number,
    top: number,
  };
  
  export type FacialHair =  {
    moustache: number,
    beard: number,
    sideburns: number,
  };
  
  export type Hair =  {
    bald: number,
    invisible: boolean,
    hairColor: HairColor[],
  };
  
  export type HairColor =  {
    color: string,
    confidence: number,
  };
  
  export type HeadPose =  {
    roll: number,
    yaw: number,
    pitch: number,
  };
  
  export type Makeup =  {
    eyeMakeup: boolean,
    lipMakeup: boolean,
  };
  
  export type Noise =  {
    noiseLevel: string,
    value: number,
  };
  
  export type Occlusion =  {
    foreheadOccluded: boolean,
    eyeOccluded: boolean,
    mouthOccluded: boolean,
  };
  
  export type PupilLeft =  {
    x: number,
    y: number,
  };
  
  export type AzureFacialRecogition =  {
    faceId: string,
    faceRectangle: FaceRectangle,
    faceLandmarks: FaceLandmarks,
    faceAttributes: FaceAttributes,
  };
  
  