import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class ProjectService {
	constructor(private http: HttpClient) {}

	async saveProject(): Promise<void> {
		return this.http.post<void>(`${env.api.url}/project/save`, undefined).toPromise();
	}
}
