import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment as env } from 'src/environments/environment';
import { Layer, LayerGroup } from '../Project.type';

@Injectable({
	providedIn: 'root',
})
export class EditService {
	constructor(private http: HttpClient) {}

	async getRenderTrees(): Promise<LayerGroup[]> {
		return this.http.get<LayerGroup[]>(`${env.api.url}/rendertrees`).toPromise();
	}

	async getLayer(layerId: string): Promise<Layer> {
		return this.http.get<Layer>(`${env.api.url}/layers/${layerId}`).toPromise();
	}

	async addLayerToGroup(groupId: string): Promise<void> {
		return this.http.post<void>(`${env.api.url}/rendertrees/${groupId}/layers`, undefined).toPromise();
	}

	async addGroup(): Promise<void> {
		return this.http.post<void>(`${env.api.url}/rendertrees`, undefined).toPromise();
	}

	async setLayerSource(layerId: string, filename: string): Promise<void> {
		return this.http
			.put<void>(`${env.api.url}/layers/${layerId}/source`, {
				filename,
			})
			.toPromise();
	}
}
