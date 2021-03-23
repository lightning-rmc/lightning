import { HttpClient, HttpResponse } from '@angular/common/http';
import { TagContentType } from '@angular/compiler';
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

	async newProject() {
		return this.http.delete<void>(`${env.api.url}/project`).toPromise();
	}

	async exportProject() {
		return this.http.get(`${env.api.url}/project.xml`, { responseType: 'blob' }).toPromise();
	}
}
