import { Injectable } from '@angular/core';
import { Node } from './models/Node.type';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment as env } from 'src/environments/environment';
import { NodeState } from './models/NodeState.type';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class NodesService {
	private connection: HubConnection;

	private nodeStateChange = new Subject<{ id: string; state: NodeState }>();
	public nodeStateChange$ = this.nodeStateChange.asObservable();

	constructor(private http: HttpClient) {
		this.connection = new HubConnectionBuilder().withUrl(`${env.controller.url}/hubs/nodes`).build();

		this.connect();
	}

	private async connect(): Promise<void> {
		try {
			console.log('Connection to SignalR Hub established');

			// Registering event handlers
			this.connection.on('nodeStateUpdate', (nodeId: string, state: NodeState) => {
				this.nodeStateChange.next({ id: nodeId, state });
			});

			await this.connection.start();
		} catch (err) {
			console.error('Connection to SignalR Hub failed', err);
		}
	}

	async getNodes(): Promise<Node[]> {
		const nodes = await this.http.get<Node[]>(`${env.api.url}/nodes`).toPromise();
		return nodes;
	}
}
