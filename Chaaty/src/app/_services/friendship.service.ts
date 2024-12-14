import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl } from '../app.config';
import { Observable } from 'rxjs';
import { Friendship } from '../_models/friendship';

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {

  constructor(private http: HttpClient) { }

  sendFriendRequest(reciver: string): Observable<any> {
    return this.http.post(apiUrl + "friendship/send-request/" + reciver, {});
  }

  acceptFriendRequest(id: number) {
    return this.http.post(`${apiUrl}friendship/accept-request/${id}`, {});
  }

  rejectFriendRequest(id: number): Observable<any> {
    return this.http.delete(`${apiUrl}friendship/reject-request/${id}`);
  }

  getFriends(): Observable<Friendship[]> {
    return this.http.get<Friendship[]>(`${apiUrl}friendship/my-friends`);
  }
  getFriendsRequest(): Observable<Friendship[]> {
    return this.http.get<Friendship[]>(`${apiUrl}friendship/requests`);
  }

}
