import { Injectable } from '@angular/core';
import { Node } from './models/Node.type';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment as env } from 'src/environments/environment';
import { NodeState } from './models/NodeState.type';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { NotificationService } from '../shared/notifications/notification.service';

@Injectable({
	providedIn: 'root',
})
export class NodesService {
	private connection: HubConnection;

	private nodeStateChangeSubject = new Subject<{ id: string; state: NodeState }>();
	public nodeStateChange$ = this.nodeStateChangeSubject.asObservable();

	private nodeConnectedSubject = new Subject<{ nodeId: string }>();
	public nodeConnected$ = this.nodeConnectedSubject.asObservable();

	constructor(private http: HttpClient, private notify: NotificationService) {
		this.connection = new HubConnectionBuilder().withUrl(`${env.controller.url}/hubs/nodes`).build();

		this.connect();
	}

	private async connect(): Promise<void> {
		try {
			console.log('Connection to SignalR Hub established');

			// Registering event handlers
			this.connection.on('NodeStateUpdateAsync', (nodeId: string, state: NodeState) => {
				this.nodeStateChangeSubject.next({ id: nodeId, state });
			});

			this.connection.on('NodeConnectedUpdateAsync', (nodeId: string) => {
				this.nodeConnectedSubject.next({ nodeId });
			});

			await this.connection.start();
		} catch (error) {
			console.error('Could not connect to nodes hub', error);
			this.notify.error('Could not establish live connection to hub:\n' + error.message);
		}
	}

	async getNodes(): Promise<Node[]> {
		const nodes = await this.http.get<Node[]>(`${env.api.url}/nodes`).toPromise();
		return nodes;
	}
}
