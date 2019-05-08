import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { IAccess } from '../models/access';

@Injectable({
  providedIn: 'root'
})
export class AccessService {
  serviceUrl = `${this.baseUrl}api/accesses`;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  list(ip: string, path: string, take: number, skip: number): Observable<IAccess[]> {    
    return this.http.get<IAccess[]>(`${this.serviceUrl}?ip=${ip}&path=${path}&take=${take}&skip=${skip}`);
  }
}
