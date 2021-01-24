import { Injectable } from '@angular/core';
import { Node } from './models/Node';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment as env } from 'src/environments/environment';
import { NodeState } from './models/NodeState';

@Injectable({
	providedIn: 'root',
})
export class NodesService {
	private connection: HubConnection;

	constructor() {
		this.connection = new HubConnectionBuilder().withAutomaticReconnect().withUrl(`${env.server.base}/hubs/nodes`).build();

		// this.connect();
	}

	nodes: Node[] = [
		{
			id: 'test1',
			name: 'Beamer links',
			state: NodeState.Edit,
		},
		{
			id: 'test2',
			name: 'Beamer mitte',
			state: NodeState.Error,
		},
		{
			id: 'test3',
			name: 'Beamer rechts',
			state: NodeState.Info,
		},
		{
			id: 'test4',
			name: 'Beamer hinten',
			state: NodeState.Info,
		},
		{
			id: 'test5',
			name: 'Node mit sehr langem Namen der wahrscheinlich nicht auf die UI passt',
			state: NodeState.Live,
		},
	];

	private async connect() {
		await this.connection.start();
		this.connection.on('nodeUpdate', (nodeId: string, state: any) => console.log({ nodeId, state }));
	}

	async getNodes(): Promise<Node[]> {
		return this.nodes;
	}
}
