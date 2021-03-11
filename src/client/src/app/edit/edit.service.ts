import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { LayerGroup } from '../Project.type';

@Injectable({
	providedIn: 'root',
})
export class EditService {
	constructor(private http: HttpClient) {}

	async getRenderTrees(): Promise<LayerGroup[]> {
		return this.http.get<LayerGroup[]>(`${env.api.url}/rendertrees`).toPromise();
	}

	async addLayerToGroup(groupId: string) {
		return this.http.post(`${env.api.url}/rendertrees/${groupId}/layers`, undefined).toPromise();
	}
}
