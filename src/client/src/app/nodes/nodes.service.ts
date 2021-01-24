import { Injectable } from '@angular/core';
import { Node } from './models/Node';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class NodesService {

	private connection: HubConnection;

	constructor() {
		this.connection = new HubConnectionBuilder()
			.withAutomaticReconnect()
			.withUrl(`${env.server.base}/hubs/nodes`)
			.build();

		this.connect();
	}

	nodes: Node[] = [
		{
			id: 'test1',
			name: 'Beamer links',
			state: 'online',
		},
		{
			id: 'test2',
			name: 'Beamer mitte',
			state: 'offline',
		},
		{
			id: 'test3',
			name: 'Beamer rechts',
			state: 'error',
		},
		{
			id: 'test4',
			name: 'Beamer hinten',
			state: 'syncing',
		},
		{
			id: 'test5',
			name: 'Node mit sehr langem Namen der wahrscheinlich nicht auf die UI passt',
			state: 'syncing',
		},
	];


	private async connect() {
		await this.connection.start();
	}

	async getNodes(): Promise<Node[]> {
		return this.nodes;
	}
}
