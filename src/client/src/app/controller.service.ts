import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment as env } from 'src/environments/environment';
import { NotificationService } from './shared/notifications/notification.service';

@Injectable({
	providedIn: 'root',
})
export class ControllerService {
	private connection: HubConnection;

	private _state?: ControllerState;

	public get state() {
		return this._state;
	}

	constructor(private notify: NotificationService) {
		this.connection = new HubConnectionBuilder().withUrl(`${env.controller.url}/hubs/controller`).build();

		this.connect();
	}

	private async connect() {
		this.connection.on('controllerStateUpdate', state => {
			console.log('controller state update');
		});

		try {
			console.log('Connection to SignalR Hub established');
			await this.connection.start();
		} catch (error) {
			console.error('Could not connect to controller hub', error);
			this.notify.error('Could not establish live connection to hub:\n' + error.message);
		}
	}

	public async setControllerState(state: 'LIVE' | 'NOTLIVE') {
		switch (state) {
			case 'LIVE':
				await this.connection.invoke('GoLive');
				break;
			case 'NOTLIVE':
				await this.connection.invoke('GoReady');
				break;
			default:
				console.error(`Unknown controller state: '${state}'`);
		}
	}
}



export enum ControllerState {
	Start,
	Ready,
	Live,
	Shutdown,
}
