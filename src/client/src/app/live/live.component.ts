import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../shared/notifications/notification.service';

@Component({
	selector: 'app-live',
	templateUrl: './live.component.html',
	styleUrls: ['./live.component.scss'],
})
export class LiveComponent implements OnInit {
	constructor(private notify: NotificationService) {}

	ngOnInit(): void {}


	logError(): void {
		this.notify.error('This is an error');
	}

	logInfo(): void {
		this.notify.info('This is an info');
	}
}
