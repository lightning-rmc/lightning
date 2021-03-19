import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../shared/notifications/notification.service';
import { LiveService } from './live.service';

@Component({
	selector: 'app-live',
	templateUrl: './live.component.html',
	styleUrls: ['./live.component.scss'],
})
export class LiveComponent implements OnInit {
	constructor(private notify: NotificationService, private live: LiveService) {}

	ngOnInit(): void {}


	async setLive() {
		await this.live.setLiveState('LIVE');
	}

	async setNotLive() {
		await this.live.setLiveState('NOTLIVE');
	}
}
