import { Component, OnInit } from '@angular/core';
import { ControllerService } from '../controller.service';
import { NotificationService } from '../shared/notifications/notification.service';
import { LiveService } from './live.service';

@Component({
	selector: 'app-live',
	templateUrl: './live.component.html',
	styleUrls: ['./live.component.scss'],
})
export class LiveComponent implements OnInit {
	constructor(private notify: NotificationService, private controller: ControllerService) {}

	ngOnInit(): void {}


	async setLive() {
		await this.controller.setControllerState('LIVE');
	}

	async setNotLive() {
		await this.controller.setControllerState('NOTLIVE');
	}
}
