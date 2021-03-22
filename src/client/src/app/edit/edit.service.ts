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
		return this.http.get<LayerGroup[]>(`${env.api.url}/layergroups`).toPromise();
	}

	async getLayer(layerId: string): Promise<Layer> {
		return this.http.get<Layer>(`${env.api.url}/layers/${layerId}`).toPromise();
	}

	async addLayerToGroup(groupId: string): Promise<object> {
		return this.http.post(`${env.api.url}/layergroups/${groupId}/layers`, undefined).toPromise();
	}

	async addGroup(): Promise<object> {
		return this.http.post(`${env.api.url}/layergroups`, undefined).toPromise();
	}

	async setLayerSource(layerId: string, filename: string): Promise<object> {
		return this.http
			.put(`${env.api.url}/layers/${layerId}/source`, {
				filename,
			})
			.toPromise();
	}

	async deleteLayer(layerId: string): Promise<object> {
		return this.http.delete(`${env.api.url}/layers/${layerId}`).toPromise();
	}

	async deleteGroup(groupId: string): Promise<object> {
		return this.http.delete(`${env.api.url}/layergroups/${groupId}`).toPromise();
	}
}
