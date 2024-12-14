import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User, UserDetails } from '../_models/user';
import { apiUrl } from '../app.config';
import { BehaviorSubject, Observable, map, of, shareReplay, tap } from 'rxjs';
import { PaginationResult } from '../_models/Pagination';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private members: User[] = [];
  private loaded = false;
  private paginationResult: PaginationResult<User[]> = new PaginationResult<User[]>();

  constructor(private http: HttpClient) { }

  loadMembers(page: number, itemPerPage: number) {
    // if (this.loaded) {
    //   console.log('Returning cached members');
    //   return of(this.members);
    // }
    let params = new HttpParams();
    if (page !== null && itemPerPage !== null) {
      params = params.append('PageNumber', page.toString());
      params = params.append('PageSize', itemPerPage.toString());

    }
    return this.http.get<User[]>(`${apiUrl}users/all`, { observe: 'response', params }).pipe(
      map(members => {
        this.paginationResult.result = members.body ?? [];
        if (members.headers.get('Pagination') !== null) {
          this.paginationResult.pagination = JSON.parse(members.headers.get('Pagination') ?? '')
        }
        return this.paginationResult;
      }),
    );

  }

  loadDetail(id: string | null): Observable<UserDetails> {
    console.log(id);
    return this.http.get<UserDetails>(apiUrl + "users/detail/" + id);
  }
  loadCurrentUserDetail(): Observable<UserDetails> {
    return this.http.get<UserDetails>(apiUrl + "users/my-details" )
  }
  updateUser(body: any) {
    return this.http.put(apiUrl + "users/update", body)
  }
  uploadPhoto(photo: File) {
    const formData = new FormData();
    formData.append('file', photo);
    return this.http.post(apiUrl + "users/upload/Photo", formData);
  }

  updateMainPhoto(id: number) {
    return this.http.put(apiUrl + "users/update/main-photo/" + id, {});
  }
  deletePhoto(id: number) {
    return this.http.delete(apiUrl + "users/delete/photo/" + id);
  }
}
