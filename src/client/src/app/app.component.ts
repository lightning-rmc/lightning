import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { SubSink } from 'subsink';
import { EditService } from './edit/edit.service';
import { NodesService } from './nodes/nodes.service';
import { ProjectService } from './project.service';
import { Notification } from './shared/notifications/Notification.model';
import { NotificationService } from './shared/notifications/notification.service';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
	@ViewChild('toggleButton') toggleButton!: ElementRef;
	@ViewChild('menu') menu!: ElementRef;

	isMenuOpen = false;

	notifications: Notification[] = [];

	sink = new SubSink();

	constructor(
		private renderer: Renderer2,
		private projectService: ProjectService,
		private nodesService: NodesService,
		private editService: EditService,
		private notify: NotificationService
	) {
		this.renderer.listen('window', 'click', (e: Event) => {
			if (!this.toggleButton.nativeElement.contains(e.target) && !this.menu?.nativeElement.contains(e.target)) {
				this.isMenuOpen = false;
			}
		});
	}

	ngOnInit(): void {
		this.sink.sink = this.notify.onNewNotification$.subscribe((notification) => {
			this.notifications.push(notification);
			setTimeout(() => this.notifications.splice(this.notifications.indexOf(notification), 1), notification.duration || 5000);
		});

		this.sink.sink = this.nodesService.nodeConnected$.subscribe((node) => {
			this.notify.info('New node connected');
		});
	}

	async onSave(): Promise<void> {
		try {
			await this.projectService.saveProject();
			this.isMenuOpen = false;
		} catch (error) {
			this.notify.error('Could not save project on server');
		}
	}
}
