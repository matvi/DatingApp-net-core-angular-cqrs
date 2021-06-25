import { Photo } from "./photo";

export interface Member {
    userId: number;
    userName: string;
    age: number;
    nickName: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photoUrl: string;
    photos: Photo[];
}