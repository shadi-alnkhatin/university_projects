export interface User {

  id: string
  email: string
  firstName: string
  lastName: string
  age: string
  gender: string
  country: string
  photoUrl: string
  photos: any[]
}

export interface UserDetails {

  id: string;
  email: string;
  firstName: string;
  lastName: string;
  photoUrl: string;
  age: string;
  gender: string;
  country: string;
  city: string;
  lookingFor: string;
  bio: string;
  createdAt: Date;
  lastActive: Date;
  isThereFriendRequest: boolean;
  photos: any[];
  isSender: boolean
  friendshipStatus: string
  friendShipId: number
}
