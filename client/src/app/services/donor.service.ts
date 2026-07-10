import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Donor } from "../models/Donor.model";



@Injectable({
    providedIn: 'root',
})
export class DonorService {
    BASE_URL = 'http://localhost:8088/api/Donor';

    http: HttpClient = inject(HttpClient);
    constructor() { }

    getDonorsList(): Observable<Donor[]> {
        return this.http.get<Donor[]>(`${this.BASE_URL}/donorsList`);
    }

    getDonorById(id: string): Observable<Donor> {
        return this.http.get<Donor>(`${this.BASE_URL}/${id}`);
    }

     createDonor(donor:Donor): Observable<Donor> {
        return this.http.post<Donor>(`${this.BASE_URL}`, donor);
      }

    updateDonor(id: string, donor: Donor): Observable<Donor> {
        return this.http.put<Donor>(`${this.BASE_URL}/${id}`, donor);
    }

    deleteDonor(id: string): Observable<Donor> {
        return this.http.delete<Donor>(`${this.BASE_URL}/${id}`);
    }

}