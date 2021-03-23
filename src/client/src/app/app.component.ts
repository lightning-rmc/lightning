import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { FileSaverService } from 'ngx-filesaver';
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

	isMenuOpen = true;

	notifications: Notification[] = [];

	sink = new SubSink();

	constructor(
		private renderer: Renderer2,
		private projectService: ProjectService,
		private nodesService: NodesService,
		private fileSaver: FileSaverService,
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

		this.sink.sink = this.nodesService.nodeConnected$.subscribe(({ nodeId }) => {
			this.notify.info('New node connected: ' + nodeId);
		});
	}

	async onSave(): Promise<void> {
		try {
			await this.projectService.saveProject();
			this.notify.info('Saved project successfully');
			this.isMenuOpen = false;
		} catch (error) {
			this.notify.error('Could not save project');
		}
	}

	async onNewProject() {
		try {
			await this.projectService.newProject();
			this.isMenuOpen = false;
		} catch (error) {
			this.notify.error('Could not create new project');
		}
	}

	async onExport() {
		try {
			const projectBlob = await this.projectService.exportProject();
			this.fileSaver.save(projectBlob, 'project.xml');
			this.isMenuOpen = false;
		} catch (error) {
			console.error(error);
			this.notify.error('Could not export project:\n' + error.message);
		}
	}

	async handleFileInput(files: FileList) {
		if (files.length === 1) {
			const file = files.item(0)!;
			const projectText = await file.text();
			try {
				await this.projectService.importProject(projectText);
				this.isMenuOpen = false;
			} catch (error) {
				console.error(error);
				this.notify.error('Could not import project, check server log for details');
			}
		}
	}
}
